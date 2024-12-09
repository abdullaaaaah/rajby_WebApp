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
      var month = DateTime.Now.AddMonths(-3);

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
                 DeptDet = sd.SetsetupName, // DeptDet from SetSetup
                 DeptGrp = sg.SetsetupName // DeptGrp from SetSetup
               }) on r.DeptId equals dp.DeptId
          join suo in _context.SetSetups on d.Uomid equals suo.SetsetupId
          join i in _context.SetItemCds on d.ItemId equals i.ItemId
          where (d.Status == "Requested" ||
                 (r.ApprovedBy != null && r.DocDt >= month && d.Status != "Approved") ||
                 d.Status == "Approved") // Include logic for Approved status
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
            DeptGroup = dp.DeptDet,
            RDComment = d.Comments,
            ItemName = i.ItemName,
            UOMName = suo.SetsetupName,
            AvailableQty = (decimal?)d.QtyToProcure,
            Status = d.Status // Map status directly into the view model
          };

      // Grouping by DocId
      var groupedData = chemicalData.GroupBy(d => d.DocId).ToList();

      // Pagination applied to grouped data
      var paginatedData = groupedData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      // Pass pagination info to the view
      ViewData["TotalPages"] = (int)Math.Ceiling((double)groupedData.Count / pageSize);
      ViewData["CurrentPage"] = page;
      ViewData["TotalItems"] = groupedData.Count;

      return View(paginatedData);
    }


  }
}
