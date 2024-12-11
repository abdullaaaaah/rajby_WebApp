using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rajby_web.Encryption;
using Rajby_web.Models;

namespace Rajby_web.Controllers
{
  public class Initial_Approved_SparesController : Controller
  {
    private readonly RajbyTextileContext _context;

    public Initial_Approved_SparesController(RajbyTextileContext context)
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
    public JsonResult Approve(int[] requisitionIds)
    {
      if (requisitionIds == null || requisitionIds.Length == 0)
      {
        return Json(new { success = false, message = "No requisition selected for approval." });
      }

      try
      {
        requisitionIds = requisitionIds.Distinct().ToArray();
        var currentUser = User.Identity.Name; // Get the current logged-in user
        var machineName = Environment.MachineName; // Get the computer name
        var currentDate = DateTime.Now; // Current timestamp

        foreach (var requisitionId in requisitionIds)
        {
          // Fetch the requisition
          var requisition = _context.PmsRequisitions.FirstOrDefault(r => r.RequisitionId == requisitionId);

          if (requisition != null)
          {
            // Update requisition approval details
            requisition.ApprovedBy = null;
            requisition.ApprovedOn = null;
            requisition.ApprovedComp = null;

            // Fetch requisition details
            var requisitionDetails = _context.PmsRequisitionDetGsps
                                             .Where(rd => rd.RequisitionId == requisitionId)
                                             .ToList();

            foreach (var detail in requisitionDetails)
            {
                // Insert a history record for each requisition detail
                var history = new PmsRequisitionHistory
                {
                  RequisitionId = requisition.RequisitionId,
                  StatusChangedBy = currentUser,
                  StatusChangedComp = machineName,
                  StatusChangedDate = currentDate,
                  Status = "Request Reword" // Original status before nulling
                };
                _context.PmsRequisitionHistories.Add(history);
              

              // Update the status field in requisition details to null
              detail.Status = null;
            }
          }
        }

        // Save changes once for all updates and additions
        _context.SaveChanges();

        return Json(new { success = true, message = "Requisition(s) Rewreved successfully, and history recorded." });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = "An error occurred: " + ex.Message });
      }
    }



  }
}
