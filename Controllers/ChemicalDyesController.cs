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
      var chemicalData = (from req in _context.PmsRequisitions
                          join dept in _context.VsetDepartments
                          on req.DeptId equals dept.DeptId
                          join reqDet in _context.PmsRequisitionDetGsps
                          on req.RequisitionId equals reqDet.RequisitionId into reqDetJoin
                          from rd in reqDetJoin.DefaultIfEmpty()
                          join setup in _context.SetSetups
                          on rd.UomId equals setup.SetsetupId into setupJoin
                          from s in setupJoin.DefaultIfEmpty()
                          join item in _context.SetItemCds
                          on rd.ItemId equals item.ItemId into itemJoin
                          from i in itemJoin.DefaultIfEmpty()
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
                            RDComment = rd.Comments,
                            ItemId = rd.ItemId,
                            SetsetupName = s.SetsetupName,  // UOM Name
                            AvailableQty = (decimal?)rd.AvailableQty,
                            ItemName = i.ItemName  // Item Name
                          })
                        .OrderByDescending(r => r.DocDt)
                        .ToList();
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
