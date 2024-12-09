using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 20)
    {
      // Total records count for "Accepted" only
      int totalRecords = await context.CmsPreCostings
          .Where(costing => costing.Approvalstatus == "Accepted") // Filter for only "Accepted" records
          .CountAsync();

      // Get the start date for 3 months ago
      var startDate = DateTime.Now.AddMonths(-3).AddDays(1 - DateTime.Now.Day); // Start of 3 months ago

      // Get the end date for the current month
      var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)); // End of the current month

      // Fetch data and project it into PreCostingViewModel with the same filters
      var precostingQuery = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate) // Filter for the last 3 months
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
              });

      // Apply pagination
      var precostingList = await precostingQuery
          .Skip((pageNumber - 1) * pageSize)  // Skip records for the current page
          .Take(pageSize)                    // Take only the records for the current page
          .ToListAsync();

      // Pass data to the view
      ViewData["PageNumber"] = pageNumber;
      ViewData["PageSize"] = pageSize;
      ViewData["TotalRecords"] = totalRecords;

      // Calculate total pages for pagination
      ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

      return View(precostingList); // Pass the list to the view
    }
  }
}
