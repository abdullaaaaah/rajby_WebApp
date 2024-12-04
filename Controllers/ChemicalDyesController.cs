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

    public IActionResult List(int page = 1, int pageSize = 10)
    {
      var threeMonthsAgo = DateTime.Now.AddMonths(-3);

      // Fetching the data
      var chemicalData = (from req in _context.PmsRequisitionCds
                          join dept in _context.VsetDepartments
                          on req.DeptId equals dept.DeptId
                          join setup in _context.SetSetups
                          on req.ReqTypeId equals setup.SetsetupId
                          where req.DocDt >= threeMonthsAgo
                          select new ChemicalViewModel
                          {
                            RequisitionId = req.RequisitionId,
                            DocId = req.DocId,
                            DocDt = req.DocDt,
                            DeptId = req.DeptId,
                            StoreId = req.StoreId,
                            Comments = req.Comments,
                            DeptGroup = dept.DeptDet + " - " + dept.DeptGrp,
                            SetsetupName = setup.SetsetupName
                          })
                          .OrderByDescending(r => r.DocDt);

      // Pagination
      var paginatedData = chemicalData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      // Pass grouped data to the view
      var groupedData = paginatedData.GroupBy(d => d.DocId).ToList();
      ViewData["TotalPages"] = (int)Math.Ceiling((double)chemicalData.Count() / pageSize);
      ViewData["CurrentPage"] = page;

      return View(groupedData);
    }
  }
}
