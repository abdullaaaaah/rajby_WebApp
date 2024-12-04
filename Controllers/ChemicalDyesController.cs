using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;
using System;
using System.Linq;

namespace Rajby_web.Controllers
{
  [Authorize]
  public class ChemicalDyesController : Controller
  {
    private readonly RajbyTextileContext _context;

    public ChemicalDyesController(RajbyTextileContext context)
    {
      _context = context;
    }

    // Action to fetch and display data from the last 3 months
    public IActionResult List()
    {
      var threeMonthsAgo = DateTime.Now.AddMonths(-3);

      // Query to fetch data
      var chemicalData = (from req in _context.PmsRequisitionCds
                          join dept in _context.VsetDepartments
                          on req.DeptId equals dept.DeptId
                          where req.DocDt >= threeMonthsAgo
                          select new ChemicalViewModel
                          {
                            RequisitionId = req.RequisitionId,
                            DocId = req.DocId,
                            DocDt = req.DocDt,
                            DeptId = req.DeptId,
                            StoreId = req.StoreId,
                            Comments = req.Comments,
                            DeptGroup = dept.DeptDet + " - " + dept.DeptGrp
                          }).OrderByDescending(r => r.DocDt).ToList();

      return View(chemicalData);
    }
  }
}
