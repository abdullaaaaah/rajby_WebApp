using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rajby_web.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Rajby_web.Controllers
{
  [Authorize]
  public class SparesController : Controller
  {
    private readonly RajbyTextileContext _context;

    public SparesController(RajbyTextileContext context)
    {
      this._context = context;
    }


    public IActionResult List(int page = 1, int pageSize = 10)
    {
      var month = DateTime.Now.AddMonths(-3);

      // Fetching the data with the updated LINQ query
      var chemicalData =
     from r in _context.PmsRequisitions
     join d in _context.PmsRequisitionDetGsps on r.RequisitionId equals d.RequisitionId
     join dp in
         (from sd in _context.SetSetups
          join sg in _context.SetSetups on sd.SetsetupId equals sg.SetsetupId
          join dept in _context.SetDepartments on sd.SetsetupId equals dept.DetId
          select new
          {
            DeptId = dept.DeptId,
            DeptDet = sd.SetsetupName,  // DeptDet from SetSetup
            DeptGrp = sg.SetsetupName  // DeptGrp from SetSetup
          }) on r.DeptId equals dp.DeptId
     join suo in _context.SetSetups on d.UomId equals suo.SetsetupId
     join i in _context.SetItemCds on d.ItemId equals i.ItemId
     where (d.Status == "Requested" &&
            (r.ApprovedBy != null && r.DocDt >= month && d.Status != "Approved"))
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
       DeptGroup = dp.DeptDet,
       RDComment = d.Comments,
       ItemName = i.ItemName,
       UOMName = suo.SetsetupName,
       AvailableQty = (decimal?)d.QtyToProcure,
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

    [HttpPost]
    public JsonResult Approve(int[] requisitionIds, int[] requisitionDetIds, Dictionary<int, decimal?> suggestedQuantities)
    {
      // Check if any requisition details were selected
      if (requisitionDetIds == null || requisitionDetIds.Length == 0)
      {
        return Json(new { success = false, message = "No requisition details selected for approval." });
      }

      var currentUser = User.Identity.Name;  // Get the current logged-in user
      var machineName = Environment.MachineName;  // Get the computer name
      var currentDate = DateTime.Now;  // Current timestamp

      // Filter unique RequisitionDetIds
      requisitionDetIds = requisitionDetIds.Distinct().ToArray();

      // Fetch and process requisition details
      var requisitionDetails = _context.PmsRequisitionDetGsps
          .Where(d => requisitionDetIds.Contains((int)d.RequisitionDetId)) // Explicit cast to match int[].
          .ToList();

      foreach (var detail in requisitionDetails)
      {
        // Store the initial QtyToProcure value for history logging
        var initialQtyToProcure = detail.QtyToProcure;

        // Check if a suggested quantity exists for this requisition detail
        if (suggestedQuantities.ContainsKey((int)detail.RequisitionDetId))
        {
          decimal? suggestedQty = suggestedQuantities[(int)detail.RequisitionDetId];

          if (suggestedQty.HasValue)
          {
            // Update QtyToProcure and AvailableQty with suggested quantity
            detail.QtyToProcure = (int)suggestedQty.Value;  // Assuming QtyToProcure and AvailableQty are integers
            detail.Qty = (int)suggestedQty.Value;  // Update AvailableQty similarly
          }
        }

        // Update status and add history only if not already processed
        if (detail.Status != "Approved")
        {
          // Update status to "Approved"
          detail.Status = "Approved";

          // Add to history
          var history = new PmsRequisitionHistory
          {
            RequisitionId = (long)detail.RequisitionId,
            RequisitionDetId = detail.RequisitionDetId,
            PreviousQuantity = initialQtyToProcure,  // Use the initial quantity for history
            ApprovedQuantity = (double?)(suggestedQuantities.ContainsKey((int)detail.RequisitionDetId) ? suggestedQuantities[(int)detail.RequisitionDetId] : null),
            StatusChangedBy = currentUser,
            StatusChangedComp = machineName,
            StatusChangedDate = currentDate,
            Status = "Approved"
          };

          _context.PmsRequisitionHistories.Add(history);  // Add history record

          // Update PmsRequisition for approval details
          var requisitionCD = _context.PmsRequisitions
              .FirstOrDefault(r => r.RequisitionId == detail.RequisitionId);

          if (requisitionCD != null)
          {
            requisitionCD.ApprovedBy2 = currentUser;
            requisitionCD.ApprovedOn2 = currentDate;
            requisitionCD.ApprovedComp2 = machineName;

            _context.PmsRequisitions.Update(requisitionCD);  // Ensure the changes are tracked.
          }
        }
      }

      // Save changes to the database
      try
      {
        _context.SaveChanges();
        return Json(new { success = true, message = "Selected requisition details approved successfully." });
      }
      catch (Exception ex)
      {
        // Handle errors and rollback if necessary
        return Json(new { success = false, message = "An error occurred while processing the approval: " + ex.Message });
      }
    }


  }
}
