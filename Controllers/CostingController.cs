using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rajby_web.Models;
using System;

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

    // Action method to display the list of costing with date filter
    public IActionResult List(DateTime? startDate, DateTime? endDate)
    {
      // If no dates are provided, set the default to the current month's start and end dates
      if (!startDate.HasValue)
      {
        startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      }

      if (!endDate.HasValue)
      {
        endDate = startDate.Value.AddMonths(1).AddDays(-1);
      }

      // Fetch data based on the provided or default date range and the "Requested" approval status
      var precostingList = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate) // Filter by date range
          .Where(costing => costing.Approvalstatus == "Requested") // Only show "Requested" approval status
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

      // Pass the filtered list and the dates to the view
      ViewData["StartDate"] = startDate.Value.ToString("yyyy-MM-dd");
      ViewData["EndDate"] = endDate.Value.ToString("yyyy-MM-dd");

      return View(precostingList);
    }
  }
}
