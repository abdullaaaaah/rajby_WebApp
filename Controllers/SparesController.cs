using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;
using Microsoft.EntityFrameworkCore; // For EF Core support
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

    public IActionResult List()
    {
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
          .ToList();

      return View(requisitions);
    }
  }
}
