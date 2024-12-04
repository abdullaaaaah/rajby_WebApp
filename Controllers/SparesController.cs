using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

    public IActionResult List(int pageNumber = 1, int pageSize = 10)
    {
      // Calculate the total number of records
      var totalRecords = context.PmsRequisitions
          .Join(context.VsetDepartments,
              r => r.DeptId,
              d => d.DeptId,
              (r, d) => new { r, d }) // Anonymous object for intermediate join
          .Join(context.SetSetups, // Joining SetSetup table
              rd => rd.r.ReqTypeId,  // Assuming ReqTypeId in PmsRequisitions matches SetSetupId
              s => s.SetsetupId,
              (rd, s) => new RequisitionViewModel
              {
                RequisitionId = rd.r.RequisitionId,
                DocId = rd.r.DocId,
                DocDt = rd.r.DocDt,
                DeptId = rd.r.DeptId,
                ReqTypeId = rd.r.ReqTypeId,
                Comments = rd.r.Comments,
                DeptGrp = rd.d.DeptDet + " - " + rd.d.DeptGrp,
                SetsetupName = s.SetsetupName // Get the SetSetup name
              })
          .Where(r => r.DocDt >= DateTime.Now.AddMonths(-3))
          .Count();

      // Fetch paginated requisitions
      var requisitions = context.PmsRequisitions
          .Join(context.VsetDepartments,
              r => r.DeptId,
              d => d.DeptId,
              (r, d) => new { r, d }) // Anonymous object for intermediate join
          .Join(context.SetSetups, // Joining SetSetup table
              rd => rd.r.ReqTypeId,  // Assuming ReqTypeId in PmsRequisitions matches SetSetupId
              s => s.SetsetupId,
              (rd, s) => new RequisitionViewModel
              {
                RequisitionId = rd.r.RequisitionId,
                DocId = rd.r.DocId,
                DocDt = rd.r.DocDt,
                DeptId = rd.r.DeptId,
                ReqTypeId = rd.r.ReqTypeId,
                Comments = rd.r.Comments,
                DeptGrp = rd.d.DeptDet + " - " + rd.d.DeptGrp,
                SetsetupName = s.SetsetupName // Get the SetSetup name
              })
          .Where(r => r.DocDt >= DateTime.Now.AddMonths(-3))
          .OrderByDescending(r => r.DocDt)
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      // Calculate total pages
      var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

      // Pass pagination data to the view
      ViewData["PageNumber"] = pageNumber;
      ViewData["PageSize"] = pageSize;
      ViewData["TotalRecords"] = totalRecords;
      ViewData["TotalPages"] = totalPages;

      return View(requisitions);
    }

  }
}
