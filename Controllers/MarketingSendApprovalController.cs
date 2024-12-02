using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public IActionResult List()
    {
      // Get the logged-in user's username
      string currentUser = User.Identity.Name;

      // Get the start and end dates for the last two months
      var endDate = DateTime.Now.AddDays(-1); // End date is yesterday
      var startDate = endDate.AddMonths(-2).AddDays(1); // Start date is two months ago from yesterday

      // Get the list of precosting records for the logged-in user with the requested approval status
      var precostingList = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate)
          .Where(costing => costing.Approvalstatus == "Requested")
          .Where(costing => costing.CreateBy == currentUser) // Filter by logged-in user
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
  }

}
