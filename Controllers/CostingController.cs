using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rajby_web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rajby_web.Controllers
{
  [Authorize]
  public class CostingController : Controller
  {
    private readonly RajbyTextileContext context;

    public CostingController(RajbyTextileContext context)
    {
      this.context = context;
    }

    // Action method to display the list of costing with date filter and pagination
    public async Task<IActionResult> List(DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 20)
    {
      // If no dates are provided, set the default to the last year's start date and today's date
      if (!startDate.HasValue)
      {
        startDate = DateTime.Now.AddYears(-1);
      }

      if (!endDate.HasValue)
      {
        endDate = DateTime.Now;
      }

      // Build the query with filters
      var precostingQuery = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate) // Date range filter
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

      // Total record count
      int totalRecords = await precostingQuery.CountAsync();

      // Apply pagination
      var precostingList = await precostingQuery
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      // Pass data to the view
      ViewData["StartDate"] = startDate.Value.ToString("yyyy-MM-dd");
      ViewData["EndDate"] = endDate.Value.ToString("yyyy-MM-dd");
      ViewData["PageNumber"] = pageNumber;
      ViewData["PageSize"] = pageSize;
      ViewData["TotalRecords"] = totalRecords;

      return View(precostingList);
    }
  }
}
