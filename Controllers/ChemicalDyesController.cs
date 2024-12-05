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

      // Fetching the data with the updated LINQ query
      var chemicalData =
      from r in _context.PmsRequisitionCds
      join d in _context.PmsRequisitionDetCds on r.RequisitionId equals d.RequisitionId
      join dp in
          (from sd in _context.SetSetups
           join sg in _context.SetSetups on sd.SetsetupId equals sg.SetsetupId // Adjusted the join condition
           join dept in _context.SetDepartments on sd.SetsetupId equals dept.DetId
           select new
           {
             DeptId = dept.DeptId,
             DeptDet = sd.SetsetupName,  // DeptDet from SetSetup
             DeptGrp = sg.SetsetupName  // DeptGrp from SetSetup
           }) on r.DeptId equals dp.DeptId
      join suo in _context.SetSetups on d.Uomid equals suo.SetsetupId
      join i in _context.SetItemCds on d.ItemId equals i.ItemId
      orderby r.DocDt descending
      select new ChemicalViewModel
      {
        RequisitionId = r.RequisitionId,
        DocId = r.DocId,
        DocDt = r.DocDt,
        DeptId = r.DeptId,
        StoreId = r.StoreId,
        Comments = r.Comments,
        DeptGroup = dp.DeptDet + " - " + dp.DeptGrp, 
        RDComment = d.Comments,
        ItemName = i.ItemName,
        UOMName = suo.SetsetupName,
        AvailableQty = (decimal?)d.QtyToProcure
      };

      // Grouping by DocId
      var groupedData = chemicalData.GroupBy(d => d.DocId).ToList();

      // Pagination applied to grouped data
      var paginatedData = groupedData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      // Pass pagination info to the view
      ViewData["TotalPages"] = (int)Math.Ceiling((double)groupedData.Count / pageSize);
      ViewData["CurrentPage"] = page;

      return View(paginatedData);
    }

  }
}
