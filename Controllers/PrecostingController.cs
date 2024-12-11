using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rajby_web.Encryption;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  [Authorize]
  public class PrecostingController : Controller
  {
    private readonly RajbyTextileContext context;

    public PrecostingController(RajbyTextileContext context)
    {
      this.context = context;
    }

    public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 20)
    {

      // Get the start date for 3 months ago
      var startDate = DateTime.Now.AddMonths(-3).AddDays(1 - DateTime.Now.Day); // Start of 3 months ago

      // Get the end date for the current month
      var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)); // End of the current month

      // Define the base query for the PreCosting list
      var precostingQuery = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate)
          .Where(costing => costing.Approvalstatus == "Requested") 
          .Join(context.LmsSetArticles,
              costing => costing.ArticleId,
              article => article.ArticleId,
              (costing, article) => new { costing, article })
          .Join(context.SetBuyers,
              combined => combined.costing.BuyerId,
              buyer => buyer.BuyerId,
              (combined, buyer) => new { combined, buyer })
          .Join(context.SetSetups.Where(s => s.SetsetupSegid == "currency"),
              combined => combined.combined.costing.CurrencyId,
              setup => setup.SetsetupId,
              (combined, setup) => new PreCostingViewModel
              {
                CostingId = combined.combined.costing.CostingId,
                CostingIdEncrypted = EncryptionHelper.Encrypt(combined.combined.costing.CostingId.ToString()),
                CostingNumber = combined.combined.costing.CostingNumber,
                CostingNumberEncrypted = EncryptionHelper.Encrypt(combined.combined.costing.CostingNumber),
                CostingDate = combined.combined.costing.CostingDate,
                MinExpectedPrice = combined.combined.costing.MinexpectedPrice,
                SellPrice = combined.combined.costing.SellPrice,
                MerchandiserSuggestPrice = combined.combined.costing.MerchandiserSuggestPrice,
                CreatedBy = combined.combined.costing.CreateBy,
                ApprovalStatus = combined.combined.costing.Approvalstatus,
                ArticleCode = combined.combined.article.ArticleCode,
                BuyerName = combined.buyer.BuyerName,
                OrderQty = combined.combined.costing.OrderQty,
                SetsetupName = setup.SetsetupName
              });

      int totalRecords = await precostingQuery.CountAsync();

      // Apply pagination
      var precostingList = await precostingQuery
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      // Pass data to the view
      ViewData["PageNumber"] = pageNumber;
      ViewData["PageSize"] = pageSize;
      ViewData["TotalRecords"] = totalRecords;

      // Calculate total pages for pagination
      ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

      return View(precostingList); // Pass the list to the View
    }




    [HttpGet]
    public IActionResult GetApprovalComments()
    {
      // Assuming you have a DbContext object to interact with the database
      var comments = context.SetSetups
                             .Where(s => s.SetsetupSegid == "Approval Comments" && s.Active == true) // Filter by "Approval Comments" and active status
                             .Select(s => new { s.SetsetupId, s.SetsetupName })
                             .ToList();

      return Json(comments);
    }


    [HttpPost]
    public IActionResult UpdateApprovalStatusAndPrice(int costingId, string action, long? commentsId, decimal? approvedPrice, double? newValue)
    {
      try
      {
        var precosting = context.CmsPreCostings.FirstOrDefault(x => x.CostingId == costingId);
        if (precosting == null)
        {
          return Json(new { success = false, message = "Costing not found." });
        }

        float? previousSellPrice = precosting.SellPrice;

        // Handle the approval status update (Reject or Accept)
        if (action == "Reject")
        {
          precosting.Approvalstatus = "Rejected";
        }
        else if (action == "Accept")
        {
          // Check if ApprovedPrice is greater than MinExpectedPrice
          if (approvedPrice.HasValue && approvedPrice > (decimal)precosting.MinexpectedPrice)
          {
            return Json(new { success = false, message = "The approved price cannot be greater than the minimum expected price." });
          }

          precosting.Approvalstatus = "Accepted";

          // Assign approvedPrice if it has a value, otherwise null
          if (approvedPrice.HasValue)
          {
            precosting.MinexpectedPrice = (float?)approvedPrice.Value;
          }

          // Assign commentsId, even if it is null (blank comments allowed)
          precosting.CommentsId = commentsId;
        }
        else
        {
          return Json(new { success = false, message = "Invalid action." });
        }

        // If a new value for the approved price is provided, check and update
        if (newValue.HasValue)
        {
          // Ensure the new value is less than or equal to the Min Expected Price
          if (newValue > precosting.MinexpectedPrice)
          {
            return Json(new { success = false, message = "The suggested price must be less than or equal to the minimum expected price." });
          }

          // Update the ApprovedPrice and SellPrice
          precosting.ApprovedPrice = (float)newValue;
          precosting.SellPrice = (float)newValue; // Explicit cast from double to float
        }

        context.SaveChanges();

        var approvalHistory = new CmsApprovalHistory
        {
          CostingId = costingId,
          SellPrice = previousSellPrice,
          ApprovedPrice = precosting.SellPrice,
          CommentsId = commentsId,
          Approvalstatus = precosting.Approvalstatus,
          StatusChangedBy = User?.Identity?.Name,
          StatusChangedOn = DateTime.Now,
          StatusChangedComp = Environment.MachineName
        };

        context.CmsApprovalHistories.Add(approvalHistory);
        context.SaveChanges();

        // If price was updated, trigger profit margin calculation
        if (newValue.HasValue)
        {
          context.Database.ExecuteSqlRaw("EXEC [dbo].[sp_CalculateProfitMargin] @p_CostingId, @p_NewSellPrice",
              new SqlParameter("@p_CostingId", costingId),
              new SqlParameter("@p_NewSellPrice", Convert.ToSingle(newValue)));  // Use Convert.ToSingle() for better handling of casting
        }

        return Json(new { success = true, message = "Approval status and price updated successfully.", newStatus = precosting.Approvalstatus });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = $"An error occurred: {ex.Message}", innerException = ex.InnerException?.Message });
      }
    }










  }

}
