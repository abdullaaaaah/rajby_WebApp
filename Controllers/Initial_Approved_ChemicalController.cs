using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rajby_web.Encryption;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  public class Initial_Approved_ChemicalController : Controller
  {
    private readonly RajbyTextileContext _context;

    public Initial_Approved_ChemicalController(RajbyTextileContext context)
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
            DeptDet = sd.SetsetupName,  // DeptDet from SetSetup
            DeptGrp = sg.SetsetupName  // DeptGrp from SetSetup
          }) on r.DeptId equals dp.DeptId
     join suo in _context.SetSetups on d.Uomid equals suo.SetsetupId
     join i in _context.SetItemCds on d.ItemId equals i.ItemId
     where (d.Status == "Requested" &&
            (r.ApprovedBy != null && r.DocDt >= month && d.Status != "Approved"))
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
       EncryptedItemId = EncryptionHelper.Encrypt(i.ItemId.ToString()),
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
    public JsonResult Revert(int[] requisitionIds)
    {
      if (requisitionIds == null || requisitionIds.Length == 0)
      {
        return Json(new { success = false, message = "No requisition selected for revert." });
      }

      try
      {
        // Remove duplicate requisition IDs
        requisitionIds = requisitionIds.Distinct().ToArray();

        // Get common details for history logging
        var currentUser = User.Identity.Name ?? "System"; // Default to "System" if user is null
        var machineName = Environment.MachineName; // Get the computer name
        var currentDate = DateTime.Now; // Current timestamp

        // Fetch all relevant requisitions at once
        var requisitions = _context.PmsRequisitions
                                   .Where(r => requisitionIds.Any(id => id == r.RequisitionId))
                                   .ToList();


        if (!requisitions.Any())
        {
          return Json(new { success = false, message = "No valid requisitions found for the given IDs." });
        }

        
        // Fetch requisition details in bulk
        var requisitionDetails = _context.PmsRequisitionDetCds
                                         .Where(rd => requisitionIds.Any(id => id == rd.RequisitionId))
                                         .ToList();

        // Prepare history records
        var histories = new List<PmsChemicalHistory>();

        foreach (var detail in requisitionDetails)
        {
          // Create a history record for each requisition detail
          histories.Add(new PmsChemicalHistory
          {
            RequisitionId = (long)detail.RequisitionId,
            RequisitionDetId = detail.RequisitionDetId, // Ensure this is set correctly
            StatusChangedBy = currentUser,
            StatusChangedComp = machineName,
            StatusChangedDate = currentDate,
            Status = "Request Revert" // Log the revert status
          });

          // Update the status field in requisition details to null
          detail.Status = null;
        }

        // Reset requisition approval details
        foreach (var requisition in requisitions)
        {
          requisition.ApprovedBy = null;
          requisition.ApprovedOn = null;
          requisition.ApprovedComp = null;
        }

        // Add all history records in bulk
        if (histories.Any())
        {
          _context.PmsChemicalHistories.AddRange(histories);
        }

        // Save all changes in one transaction
        _context.SaveChanges();

        return Json(new { success = true, message = "Requisition(s) reverted successfully, and history recorded." });
      }
      catch (DbUpdateException dbEx)
      {
        var innerException = dbEx.InnerException?.Message;
        return Json(new { success = false, message = "Database update error: " + innerException });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "An error occurred: " + ex.Message });
      }
    }


  }
}
