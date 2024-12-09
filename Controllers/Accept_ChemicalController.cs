using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  public class Accept_ChemicalController : Controller
  {
    private readonly RajbyTextileContext _context;

    public Accept_ChemicalController(RajbyTextileContext context)
    {
        this._context = context;
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
               join sg in _context.SetSetups on sd.SetsetupId equals sg.SetsetupId
               join dept in _context.SetDepartments on sd.SetsetupId equals dept.DetId
               select new
               {
                 DeptId = dept.DeptId,
                 DeptDet = sd.SetsetupName,
                 DeptGrp = sg.SetsetupName
               }) on r.DeptId equals dp.DeptId
          join suo in _context.SetSetups on d.Uomid equals suo.SetsetupId
          join i in _context.SetItemCds on d.ItemId equals i.ItemId
          where d.Status == null
                && r.DocDt >= threeMonthsAgo
          orderby r.DocDt descending
          select new ChemicalViewModel
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

      // Apply Pagination Logic to grouped data
      var totalItems = groupedData.Count;
      var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

      // Paginate the grouped data
      var paginatedGroups = groupedData
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();

      // Pass pagination data to the View
      ViewData["TotalPages"] = totalPages;
      ViewData["CurrentPage"] = page;
      ViewData["TotalItems"] = totalItems;
      ViewData["PageSize"] = pageSize;

      return View(paginatedGroups);
    }

  }
}
