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
    public async Task<IActionResult> List(DateTime? startDate, DateTime? endDate, string merchandiser, string articleCode, string buyerName, string costingNumber, int pageNumber = 1, int pageSize = 20)
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

      // Get the list of available merchandisers, article codes, and buyer names (for filters)
      var merchandiserList = await context.CmsPreCostings
          .Select(c => c.CreateBy)
          .Distinct()
          .ToListAsync();

      var articleList = await context.LmsSetArticles
          .Select(a => a.ArticleCode)
          .Distinct()
          .ToListAsync();

      var buyerList = await context.SetBuyers
          .Select(b => b.BuyerName)
          .Distinct()
          .ToListAsync();

      // Build the query with filters
      var precostingQuery = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate); // Date range filter

      // Apply additional filters
      if (!string.IsNullOrEmpty(merchandiser))
      {
        precostingQuery = precostingQuery.Where(costing => costing.CreateBy == merchandiser);
      }

      if (!string.IsNullOrEmpty(costingNumber))
      {
        precostingQuery = precostingQuery.Where(costing => costing.CostingNumber.Contains(costingNumber));
      }

      if (!string.IsNullOrEmpty(articleCode))
      {
        precostingQuery = precostingQuery.Where(costing => costing.ArticleCode == articleCode);
      }

      if (!string.IsNullOrEmpty(buyerName))
      {
        // Join CmsPreCosting with SetBuyer to filter by BuyerName
        precostingQuery = precostingQuery
            .Join(context.SetBuyers,
                costing => costing.BuyerId,  // CmsPreCosting's BuyerId
                buyer => buyer.BuyerId,      // SetBuyer's BuyerId
                (costing, buyer) => new { costing, buyer })  // Select the combined result
            .Where(combined => combined.buyer.BuyerName == buyerName)  // Filter by BuyerName
            .Select(combined => combined.costing);  // Return only the CmsPreCosting records
      }


      // Use lazy loading for related entities by only selecting necessary fields and allowing EF to load related entities automatically.
      var precostingDetails = precostingQuery
          .Include(costing => costing.Article) // Lazy load Article navigation property
          .Include(costing => costing.Buyer)  // Lazy load Buyer navigation property
          .Select(costing => new PreCostingViewModel
          {
            CostingId = costing.CostingId,
            CostingIdEncrypted = EncryptionHelper.Encrypt(costing.CostingId.ToString()),
            CostingNumber = costing.CostingNumber,
            CostingNumberEncrypted = EncryptionHelper.Encrypt(costing.CostingNumber),
            CostingDate = costing.CostingDate,
            MinExpectedPrice = costing.MinexpectedPrice,
            SellPrice = costing.SellPrice,
            CreatedBy = costing.CreateBy,
            ApprovalStatus = costing.Approvalstatus,
            ArticleCode = costing.Article.ArticleCode, // Lazy-loaded navigation property
            BuyerName = costing.Buyer.BuyerName // Lazy-loaded navigation property
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
      ViewData["SelectedArticleCode"] = articleCode;
      ViewData["SelectedBuyerName"] = buyerName;
      ViewData["SelectedCostingNumber"] = costingNumber;
      ViewData["Merchandisers"] = merchandiserList;
      ViewData["Articles"] = articleList;
      ViewData["Buyers"] = buyerList;

      // Calculate total pages for pagination
      ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);

      return View(precostingList);
    }

  }

}
