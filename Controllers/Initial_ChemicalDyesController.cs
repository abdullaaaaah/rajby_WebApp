using Microsoft.AspNetCore.Mvc;
using Rajby_web.Encryption;
using Rajby_web.Models;
using System.Linq;

namespace Rajby_web.Controllers
{
  public class Initial_ChemicalDyesController : Controller
  {
    private readonly RajbyTextileContext _context;

    public Initial_ChemicalDyesController(RajbyTextileContext context)
    {
      this._context = context;
    }

    public IActionResult List(int page = 1, int pageSize = 20)
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
      where d.Status == null // Filter for NULL status
          && r.DocDt >= threeMonthsAgo // Filter for the last three months
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
        EncryptedItemId = EncryptionHelper.Encrypt(i.ItemId.ToString()),
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

    [HttpPost]
    public JsonResult Approve(int[] requisitionIds)
    {
      // Ensure unique IDs to prevent duplicate processing
      if (requisitionIds == null || requisitionIds.Length == 0)
      {
        return Json(new { success = false, message = "No requisition selected for approval." });
      }

      requisitionIds = requisitionIds.Distinct().ToArray();
      var currentUser = @User.Identity.Name;  // Get the current logged-in user
      var machineName = Environment.MachineName;  // Get the computer name
      var currentDate = DateTime.Now;  // Current timestamp

      foreach (var requisitionId in requisitionIds)
      {
        // Fetch the requisition
        var requisition = _context.PmsRequisitionCds.FirstOrDefault(r => r.RequisitionId == requisitionId);

        if (requisition != null)
        {
          // Update requisition approval details
          requisition.ApprovedBy = currentUser;
          requisition.ApprovedOn = currentDate;
          requisition.ApprovedComp = machineName;

          // Fetch requisition details (child records)
          var requisitionDetails = _context.PmsRequisitionDetCds
                                           .Where(rd => rd.RequisitionId == requisitionId)
                                           .Distinct()
                                           .ToList();

          foreach (var detail in requisitionDetails)
          {
            // Insert a new history record for each requisition detail (no check for existing history)
            var history = new PmsChemicalHistory
            {
              RequisitionId = requisition.RequisitionId,
              RequisitionDetId = detail.RequisitionDetId,
              PreviousQuantity = detail.QtyToProcure, // Assuming 'QtyToProcure' exists in details
              StatusChangedBy = currentUser,
              StatusChangedComp = machineName,
              StatusChangedDate = currentDate,
              Status = "Requested" // Adjust status as required
            };

            // Add the new history record
            _context.PmsChemicalHistories.Add(history);

            // Update the status field in requisition details
            detail.Status = "Requested";
          }
        }
      }

      // Save changes once for all updates and additions
      _context.SaveChanges();

      return Json(new { success = true, message = "Requisition(s) approved successfully, and history recorded." });
    }
  }
}


  

