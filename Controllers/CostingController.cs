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
    public async Task<IActionResult> List(
    DateTime? startDate,
    DateTime? endDate,
    string merchandiser,
    string articleCode,
    string buyerName,
    string costingNumber,
    int pageNumber = 1,
    int pageSize = 20)
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

      // Build the query with filters
      var precostingQuery = context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate) // Date range filter
          .Where(costing => costing.CurrencyId != null) // Ensure there's a currency ID for joining
          .Join(context.SetSetups.Where(s => s.SetsetupSegid == "currency"),
                costing => costing.CurrencyId,
                setup => setup.SetsetupId,
                (costing, setup) => new { costing, setup }); // Join with SetSetup

      // Apply additional filters
      if (!string.IsNullOrEmpty(merchandiser))
      {
        precostingQuery = precostingQuery.Where(combined => combined.costing.CreateBy == merchandiser);
      }

      if (!string.IsNullOrEmpty(costingNumber))
      {
        precostingQuery = precostingQuery.Where(combined => combined.costing.CostingNumber.Contains(costingNumber));
      }

      if (!string.IsNullOrEmpty(articleCode))
      {
        precostingQuery = precostingQuery.Where(combined => combined.costing.ArticleCode == articleCode);
      }

      if (!string.IsNullOrEmpty(buyerName))
      {
        precostingQuery = precostingQuery
            .Join(context.SetBuyers,
                  combined => combined.costing.BuyerId,
                  buyer => buyer.BuyerId,
                  (combined, buyer) => new { combined.costing, combined.setup, buyer })
            .Where(combined => combined.buyer.BuyerName == buyerName)
            .Select(combined => new { combined.costing, combined.setup }); // Preserve currency join
      }

      // Extract dynamic filter options
      var dynamicFilterData = await precostingQuery
          .Select(combined => new
          {
            Merchandiser = combined.costing.CreateBy,
            ArticleCode = combined.costing.ArticleCode,
            BuyerName = combined.costing.Buyer.BuyerName // Lazy-loaded navigation property
          })
          .ToListAsync();

      var merchandiserList = dynamicFilterData
          .Where(d => !string.IsNullOrEmpty(d.Merchandiser))
          .Select(d => d.Merchandiser)
          .Distinct()
          .ToList();

      var articleList = dynamicFilterData
          .Where(d => !string.IsNullOrEmpty(d.ArticleCode))
          .Select(d => d.ArticleCode)
          .Distinct()
          .ToList();

      var buyerList = dynamicFilterData
          .Where(d => !string.IsNullOrEmpty(d.BuyerName))
          .Select(d => d.BuyerName)
          .Distinct()
          .ToList();

      // Use lazy loading for related entities and select necessary fields
      var precostingDetails = precostingQuery
          .Select(combined => new PreCostingViewModel
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
            ArticleCode = combined.costing.ArticleCode,
            OrderQty = combined.costing.OrderQty,
            BuyerName = combined.costing.Buyer.BuyerName, // Lazy-loaded navigation property
            SetsetupName = combined.setup.SetsetupName // Include SetsetupName from SetSetup
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
