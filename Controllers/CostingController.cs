using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rajby_web.Encryption;
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
    public async Task<IActionResult> List(DateTime? startDate, DateTime? endDate, string merchandiser, int pageNumber = 1, int pageSize = 20)
    {
      // Default to last year's start date and today's date if no dates are provided
      if (!startDate.HasValue)
      {
        startDate = DateTime.Now.AddYears(-1);
      }

      if (!endDate.HasValue)
      {
        endDate = DateTime.Now;
      }

      // Get the list of available merchandisers for the dropdown
      var merchandiserList = await context.CmsPreCostings
          .Select(c => c.CreateBy) // Assuming 'CreateBy' is the merchandiser
          .Distinct()
          .ToListAsync();

      // Build the query with filters
      var precostingQuery = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate); // Date range filter

      // Apply merchandiser filter if provided
      if (!string.IsNullOrEmpty(merchandiser))
      {
        precostingQuery = precostingQuery.Where(costing => costing.CreateBy == merchandiser);
      }

      var precostingDetails = precostingQuery
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
              });

      // Total record count
      int totalRecords = await precostingDetails.CountAsync();

      // Apply pagination
      var precostingList = await precostingDetails
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

      // Pass data to the view
      ViewData["StartDate"] = startDate.Value.ToString("yyyy-MM-dd");
      ViewData["EndDate"] = endDate.Value.ToString("yyyy-MM-dd");
      ViewData["PageNumber"] = pageNumber;
      ViewData["PageSize"] = pageSize;
      ViewData["TotalRecords"] = totalRecords;
      ViewData["SelectedMerchandiser"] = merchandiser;
      ViewData["Merchandisers"] = merchandiserList;

      // Calculate total pages for pagination
      ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

      return View(precostingList);
    }
  }
}
