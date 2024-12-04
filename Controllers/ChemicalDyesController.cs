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

      // Fetching the data and applying pagination with a join to SetSetups table
      var chemicalData = (from req in _context.PmsRequisitionCds
                          join dept in _context.VsetDepartments
                          on req.DeptId equals dept.DeptId
                          join setup in _context.SetSetups  // Joining the SetSetups table
                          on req.ReqTypeId equals setup.SetsetupId  // Assuming ReqTypeId matches SetsetupId
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
                            SetsetupName = setup.SetsetupName // Get the SetSetup name
                          })
                          .OrderByDescending(r => r.DocDt);

      // Paginate the results
      var paginatedData = chemicalData.Skip((page - 1) * pageSize).Take(pageSize).ToList();
      var totalRecords = chemicalData.Count();

      // Set pagination information
      ViewData["TotalPages"] = (int)Math.Ceiling((double)totalRecords / pageSize);
      ViewData["CurrentPage"] = page;

      return View(paginatedData);
    }
  }
}
