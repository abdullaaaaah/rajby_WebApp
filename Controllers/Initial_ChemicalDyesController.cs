using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  public class Initial_ChemicalDyesController : Controller
  {
    private readonly RajbyTextileContext _context;

    public Initial_ChemicalDyesController(RajbyTextileContext context)
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

      // Pass pagination info to the view
      ViewData["TotalPages"] = (int)Math.Ceiling((double)groupedData.Count / pageSize);
      ViewData["CurrentPage"] = page;

      return View(paginatedData);
    }

    [HttpPost]
    public JsonResult Approve(int[] requisitionIds, int[] requisitionDetIds)
    {
      if (requisitionIds == null || requisitionIds.Length == 0 || requisitionDetIds == null || requisitionDetIds.Length == 0)
      {
        return Json(new { success = false, message = "No requisition or requisition details selected for approval." });
      }

      var currentUser = User.Identity.Name;  // Get the current logged-in user
      var machineName = Environment.MachineName;  // Get the computer name
      var currentDate = DateTime.Now;  // Current timestamp

      // Filter unique RequisitionDetIds
      requisitionDetIds = requisitionDetIds.Distinct().ToArray();
      requisitionIds = requisitionIds.Distinct().ToArray();

      // Fetch and process requisition details
      var requisitionDetails = _context.PmsRequisitionDetCds
          .Where(d => requisitionDetIds.Contains((int)d.RequisitionDetId)) // Explicit cast to match int[].
          .ToList();

      foreach (var detail in requisitionDetails)
      {
        // Update status and add history only if not already processed
        if (detail.Status != "Requested")
        {
          // Update status
          detail.Status = "Requested";

          // Add to history
          var history = new PmsChemicalHistory
          {
            RequisitionId = (long)detail.RequisitionId,
            RequisitionDetId = detail.RequisitionDetId,
            PreviousQuantity = detail.QtyToProcure,
            StatusChangedBy = currentUser,
            StatusChangedComp = machineName,
            StatusChangedDate = currentDate,
            Status = "Requested"
          };

          _context.PmsChemicalHistories.Add(history);
        }
      }

      // Fetch and process parent requisitions
      var requisitions = _context.PmsRequisitions
          .Where(r => requisitionIds.Contains((int)r.RequisitionId))
          .ToList();

      foreach (var requisition in requisitions)
      {
        // Update status for parent requisition if not already requested
        if (requisition.ApprovedBy != "Requested")
        {
          requisition.ApprovedBy = "Requested";
          // Optionally, add history for parent requisition here if needed.
        }
      }

      // Save changes to the database
      _context.SaveChanges();

      return Json(new { success = true, message = "Selected requisition details and parent requisitions approved successfully." });
    }


  }
}
