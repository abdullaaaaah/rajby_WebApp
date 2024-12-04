using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;
using Microsoft.EntityFrameworkCore; // For EF Core support

namespace Rajby_web.Controllers
{
  [Authorize]
  public class SparesController : Controller
  {
    private readonly RajbyTextileContext context;

    public SparesController(RajbyTextileContext context)
    {
      this.context = context;
    }

    public IActionResult List()
    {
      // Fetch department data
      var departments = context.VsetDepartments
          .ToDictionary(d => d.DeptId, d => d.DeptDet + " - " + d.DeptGrp);

      // Calculate the cutoff date for the last 3 months
      var threeMonthsAgo = DateTime.Now.AddMonths(-3);

      // Fetch and filter requisition data
      var requisitions = context.PmsRequisitions
          .Where(r => r.DocDt >= threeMonthsAgo) // Only include records from the last 3 months
          .Select(r => new RequisitionViewModel
          {
            RequisitionId = r.RequisitionId,
            DocId = r.DocId,
            DocDt = r.DocDt,
            DeptId = r.DeptId,
            StoreId = r.StoreId,
            Comments = r.Comments,
            DeptGrp = departments.ContainsKey((long)r.DeptId) ? departments[(long)r.DeptId] : null
          })
          .ToList();

      return View(requisitions);
    }

  }
}
