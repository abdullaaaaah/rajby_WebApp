using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rajby_web.Encryption;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  [Authorize]
  public class MarketingSendApprovalController : Controller
  {
    private readonly RajbyTextileContext context;

    public MarketingSendApprovalController(RajbyTextileContext context)
    {
      this.context = context;
    }

    public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 20)
    {
      // Total records count
      

      // Get the logged-in user's username
      string currentUser = User.Identity.Name;

      // Get the start and end dates for the last two months
      var endDate = DateTime.Now.AddDays(-1); // End date is yesterday
      var startDate = endDate.AddMonths(-3).AddDays(1); // Start date is two months ago from yesterday

      // Build the query with filters
      var precostingQuery = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate)
          .Where(costing => costing.Approvalstatus == null )
          .Where(costing => costing.CreateBy == currentUser) // Filter by logged-in user
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
                CreatedBy = combined.combined.costing.CreateBy,
                ApprovalStatus = combined.combined.costing.Approvalstatus,
                ArticleCode = combined.combined.article.ArticleCode,
                BuyerName = combined.buyer.BuyerName,
                OrderQty = combined.combined.costing.OrderQty, // Map the OrderQty
                SetsetupName = setup.SetsetupName // Include SetsetupName from setSetup
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

      return View(precostingList);
    }

    [HttpPost]
    public IActionResult UpdateSuggestedPriceAndApprovalStatus(int costingId, float suggestedPrice, int? commentsId)
    {
      // Find the PreCosting object based on costingId
      var precosting = context.CmsPreCostings.SingleOrDefault(p => p.CostingId == costingId);
      if (precosting == null)
      {
        return Json(new { success = false, message = "Costing record not found." });
      }

      // Validate if suggested price is less than the minimum price
      if (suggestedPrice > precosting.MinexpectedPrice)
      {
        return Json(new { success = false, message = "Suggested price cannot be greater than the minimum price." });
      }

      // Store the previous sell price for history (as float)
      float previousSellPrice = (float)precosting.SellPrice;

      // Update the suggested price and approval status to "Requested"
      precosting.MerchandiserSuggestPrice = suggestedPrice;
      precosting.Approvalstatus = "Requested";  // Set the status to "Requested"
      precosting.CommentsId = commentsId;

      // Save changes to the PreCosting
      context.SaveChanges();

      // Add the approval history record
      var approvalHistory = new CmsApprovalHistory
      {
        CostingId = costingId,
        SellPrice = previousSellPrice,
        ApprovedPrice = precosting.MerchandiserSuggestPrice,
        CommentsId = commentsId,
        Approvalstatus = precosting.Approvalstatus,
        StatusChangedBy = User?.Identity?.Name,
        StatusChangedOn = DateTime.Now,
        StatusChangedComp = Environment.MachineName
      };

      // Add the history record to the database
      context.CmsApprovalHistories.Add(approvalHistory);
      context.SaveChanges();

      return Json(new { success = true, message = "Suggested price sent successfully and approval status set to 'Requested'." });
    }




  }
}
