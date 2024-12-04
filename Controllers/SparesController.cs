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
              (r, d) => new RequisitionViewModel
              {
                RequisitionId = r.RequisitionId,
                DocId = r.DocId,
                DocDt = r.DocDt,
                DeptId = r.DeptId,
                StoreId = r.StoreId,
                Comments = r.Comments,
                DeptGrp = d.DeptDet + " - " + d.DeptGrp
              })
          .Where(r => r.DocDt >= DateTime.Now.AddMonths(-3))
          .Count();

      // Fetch paginated requisitions
      var requisitions = context.PmsRequisitions
          .Join(context.VsetDepartments,
              r => r.DeptId,
              d => d.DeptId,
              (r, d) => new RequisitionViewModel
              {
                RequisitionId = r.RequisitionId,
                DocId = r.DocId,
                DocDt = r.DocDt,
                DeptId = r.DeptId,
                StoreId = r.StoreId,
                Comments = r.Comments,
                DeptGrp = d.DeptDet + " - " + d.DeptGrp
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
