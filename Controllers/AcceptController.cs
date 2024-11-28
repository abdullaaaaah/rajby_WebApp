using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  [Authorize]
  public class AcceptController : Controller
  {
      private readonly RajbyTextileContext context;

      public AcceptController(RajbyTextileContext context)
      {
        this.context = context;
      }
      public IActionResult List()
      {
        // Get the start and end dates for the current month
        var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        // Fetch data and project it into PreCostingViewModel
        var precostingList = context.CmsPreCostings
            .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate) // Filter for the current month
            .Where(costing => costing.Approvalstatus == "Accepted") // Only show Accepted items
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
                  CostingNumber = combined.costing.CostingNumber,
                  CostingDate = combined.costing.CostingDate,
                  MinExpectedPrice = combined.costing.MinexpectedPrice,
                  SellPrice = combined.costing.SellPrice,
                  CreatedBy = combined.costing.CreateBy,
                  ApprovalStatus = combined.costing.Approvalstatus,
                  ArticleCode = combined.article.ArticleCode,
                  BuyerName = buyer.BuyerName
                })
            .ToList();

        return View(precostingList); // Pass the list to the view
      }
    }
  }

