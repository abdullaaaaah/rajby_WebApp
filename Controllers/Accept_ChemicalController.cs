using Microsoft.AspNetCore.Mvc;
using Rajby_web.Encryption;
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

      // Fetching only approved statuses in the LINQ query
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
          where d.Status == "Approved" // Filter only approved statuses
          orderby r.DocDt descending
          select new ChemicalViewModel
          {
            RequisitionDetId = d.RequisitionDetId,
            RequisitionId = r.RequisitionId,
            EncryptedRequisitionNumber = EncryptionHelper.Encrypt(r.RequisitionId.ToString()),
            DocId = r.DocId,
            DocDt = r.DocDt,
            DeptId = r.DeptId,
            StoreId = r.StoreId,
            Comments = r.Comments,
            DeptGroup = dp.DeptDet,
            RDComment = d.Comments,
            EncryptedItemId = EncryptionHelper.Encrypt(i.ItemId.ToString()),
            ItemName = i.ItemName,
            UOMName = suo.SetsetupName,
            AvailableQty = (decimal?)d.QtyToProcure,
            Status = d.Status
          };

      // Group by Document Id
      var groupedData = chemicalData.GroupBy(d => d.DocId).ToList();

      // Pagination
      var paginatedData = groupedData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

      // Pass pagination info to the view
      ViewData["TotalPages"] = (int)Math.Ceiling((double)groupedData.Count / pageSize);
      ViewData["CurrentPage"] = page;
      ViewData["TotalItems"] = groupedData.Count;

      return View(paginatedData);
    }



  }
}
