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

    public IActionResult List()
    {
      // Get the start date for 3 months ago
      var startDate = DateTime.Now.AddMonths(-3).AddDays(1 - DateTime.Now.Day); // Start of 3 months ago

      // Get the end date for the current month
      var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)); // End of the current month

      var precostingList = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate)
          .Where(costing => costing.Approvalstatus == "Requested")
          .Join(context.LmsSetArticles,
              costing => costing.ArticleId,
              article => article.ArticleId,
              (costing, article) => new { costing, article })
          .Join(context.SetBuyers,
              combined => combined.costing.BuyerId,
              buyer => buyer.BuyerId,
              (combined, buyer) => new PreCostingViewModel
              {
                CostingId = combined.costing.CostingId,
                CostingIdEncrypted = EncryptionHelper.Encrypt(combined.costing.CostingId.ToString()),
                CostingNumber = combined.costing.CostingNumber,
                CostingNumberEncrypted = EncryptionHelper.Encrypt(combined.costing.CostingNumber),
                CostingDate = combined.costing.CostingDate,
                MinExpectedPrice = combined.costing.MinexpectedPrice,
                SellPrice = combined.costing.SellPrice,
                CreatedBy = combined.costing.CreateBy,
                ApprovalStatus = combined.costing.Approvalstatus,
                ArticleCode = combined.article.ArticleCode,
                BuyerName = buyer.BuyerName
              })
          .ToList();

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

    //[HttpPost]
    //public IActionResult UpdateApprovalStatus(int costingId, string action, long? commentsId, decimal? approvedPrice)
    //{
    //  try
    //  {
    //    var precosting = context.CmsPreCostings.FirstOrDefault(x => x.CostingId == costingId);
    //    if (precosting == null)
    //    {
    //      return Json(new { success = false, message = "Costing not found." });
    //    }

    //    float? previousSellPrice = precosting.SellPrice;

    //    if (action == "Reject")
    //    {
    //      precosting.Approvalstatus = "Rejected";
    //    }
    //    else if (action == "Accept")
    //    {
    //      // No validation for either approvedPrice or commentsId, both can be null
    //      precosting.Approvalstatus = "Accepted";

    //      // Assign approvedPrice if it has a value, otherwise null
    //      if (approvedPrice.HasValue)
    //      {
    //        precosting.SellPrice = (float?)approvedPrice.Value;
    //      }

    //      // Assign commentsId, even if it is null (blank comments allowed)
    //      precosting.CommentsId = commentsId;
    //    }

    //    else
    //    {
    //      return Json(new { success = false, message = "Invalid action." });
    //    }

    //    context.SaveChanges();

    //    var approvalHistory = new CmsApprovalHistory
    //    {
    //      CostingId = costingId,
    //      SellPrice = previousSellPrice,
    //      ApprovedPrice = precosting.SellPrice,
    //      CommentsId = commentsId,
    //      Approvalstatus = precosting.Approvalstatus,
    //      StatusChangedBy = User?.Identity?.Name,
    //      StatusChangedOn = DateTime.Now,
    //      StatusChangedComp = Environment.MachineName
    //    };

    //    context.CmsApprovalHistories.Add(approvalHistory);
    //    context.SaveChanges();

    //    return Json(new { success = true, message = "Approval status updated successfully.", newStatus = precosting.Approvalstatus });
    //  }
    //  catch (Exception ex)
    //  {
    //    return Json(new { success = false, message = $"An error occurred: {ex.Message}", innerException = ex.InnerException?.Message });
    //  }
    //}


    //[HttpPost]
    //public JsonResult UpdateApprovedPrice(long costingId, double newValue)
    //{
    //  try
    //  {
    //    // Find the costing record in the database
    //    var costing = context.CmsPreCostings.FirstOrDefault(c => c.CostingId == costingId);

    //    if (costing != null)
    //    {
    //      // Check if the new approved price is greater than or equal to the MinExpectedPrice
    //      if (newValue < costing.MinexpectedPrice)
    //      {
    //        return Json(new { success = false, message = "The approved price must be greater than or equal to the minimum expected price." });
    //      }

    //      // Update the ApprovedPrice
    //      costing.ApprovedPrice = newValue;

    //      // Update the SellPrice to match the ApprovedPrice (convert newValue to float)
    //      costing.SellPrice = (float)newValue;  // Explicit cast from double to float

    //      // Save the changes to the database
    //      context.SaveChanges();

    //      // Call the stored procedure after updating the price (ensure proper conversion to float)
    //      context.Database.ExecuteSqlRaw("EXEC [dbo].[sp_CalculateProfitMargin] @p_CostingId, @p_NewSellPrice",
    //          new SqlParameter("@p_CostingId", costingId),
    //          new SqlParameter("@p_NewSellPrice", Convert.ToSingle(newValue)));  // Use Convert.ToSingle() for better handling of casting

    //      // Return success response
    //      return Json(new { success = true, message = "Price updated successfully and profit margin calculated." });
    //    }

    //    // If costing not found, return failure message
    //    return Json(new { success = false, message = "Costing record not found." });
    //  }
    //  catch (Exception ex)
    //  {
    //    // Return error message in case of exception
    //    return Json(new { success = false, message = "An error occurred while updating the price: " + ex.Message });
    //  }
    //}



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
          // No validation for either approvedPrice or commentsId, both can be null
          precosting.Approvalstatus = "Accepted";

          // Assign approvedPrice if it has a value, otherwise null
          if (approvedPrice.HasValue)
          {
            precosting.SellPrice = (float?)approvedPrice.Value;
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
          // Ensure the new value is greater than or equal to the MinExpectedPrice
          if (newValue < precosting.MinexpectedPrice)
          {
            return Json(new { success = false, message = "The approved price must be greater than or equal to the minimum expected price." });
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
