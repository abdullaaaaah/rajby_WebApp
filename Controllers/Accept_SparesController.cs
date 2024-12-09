using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  public class Accept_SparesController : Controller
  {
    private readonly RajbyTextileContext _context;

    public Accept_SparesController(RajbyTextileContext context)
    {
      this._context = context;
    }
    public IActionResult List(int page = 1, int pageSize = 10)
    {
      var threeMonthsAgo = DateTime.Now.AddMonths(-3);

      // Fetching the data with the updated LINQ query
      var chemicalData =
      from r in _context.PmsRequisitions
      join d in _context.PmsRequisitionDetGsps on r.RequisitionId equals d.RequisitionId
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
      join suo in _context.SetSetups on d.UomId equals suo.SetsetupId
      join i in _context.SetItemCds on d.ItemId equals i.ItemId
      where d.Status == null // Filter for NULL status
          && r.DocDt >= threeMonthsAgo // Filter for the last three months
      orderby r.DocDt descending
      select new RequisitionViewModel
      {
        RequisitionDetId = d.RequisitionDetId,
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

      ViewData["TotalPages"] = (int)Math.Ceiling((double)groupedData.Count / pageSize);
      ViewData["CurrentPage"] = page;
      ViewData["TotalItems"] = groupedData.Count;

      return View(paginatedData);
    }
  }
}
