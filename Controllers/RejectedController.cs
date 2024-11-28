using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  [Authorize]
  public class RejectedController : Controller
  {
    private readonly RajbyTextileContext _context;

    public RejectedController(RajbyTextileContext context)
    {
      _context = context;
    }

    public IActionResult List()
    {
      // Get the start and end dates for the current month
      var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      var endDate = startDate.AddMonths(1).AddDays(-1);

      // Fetch data and project it into PreCostingViewModel
      var precostingList = _context.CmsPreCostings
          .Where(costing => costing.CostingDate >= startDate && costing.CostingDate <= endDate) // Filter for the current month
          .Where(costing => costing.Approvalstatus == "Rejected") // Only show rejected items
          .Join(_context.LmsSetArticles,
              costing => costing.ArticleId,
              article => article.ArticleId,
              (costing, article) => new { costing, article })
          .Join(_context.SetBuyers,
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

    [HttpPost]
    public JsonResult UpdatePrices(int costingId, decimal minPrice, decimal sellPrice)
    {
      try
      {
        // Fetch the record from the database using the costing ID
        var costing = _context.CmsPreCostings.FirstOrDefault(c => c.CostingId == costingId);

        if (costing == null)
        {
          return Json(new { success = false, message = "Costing not found." });
        }

        // Update the fields
        costing.MinexpectedPrice = (float?)minPrice;
        costing.SellPrice = (float?)sellPrice;

        // Change the status to "Accepted"
        costing.Approvalstatus = "Requested";

        // Save changes to the database
        _context.SaveChanges();

        return Json(new { success = true, message = "Prices updated and status set to Accepted successfully." });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "An error occurred: " + ex.Message });
      }
    }
  }
  }
