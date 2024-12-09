namespace Rajby_web.Models
{
  public class RequisitionViewModel
  {
    //pmsRequisition
    public long RequisitionId { get; set; }
    public string? DocId { get; set; }
    public DateTime? DocDt { get; set; }
    public long? DeptId { get; set; }
    public long? StoreId { get; set; }
    public string? Comments { get; set; }
    //VsetDepartment
    public string? DeptGroup { get; set; } // Mapped from "deptDet + ' - ' + deptGrp"
    public long SetsetupId { get; set; }
    public string UOMName { get; set; } = null!;

    public long? ReqTypeId { get; set; }


    // New properties as per your query
    public string? RDComment { get; set; } // Comments from PmsRequisitionDetGsp

    public long? ItemId { get; set; } // ItemId from PmsRequisitionDetGspD
    public decimal? AvailableQty { get; set; } // AvailableQty from PmsRequisitionDetGsp\
    public decimal? QtyToProcure { get; set; } // AvailableQty from PmsRequisitionDetGsp\

    public string? ItemName { get; set; }

    public long RequisitionDetId { get; set; }
    public string? Status { get; set; }



    //VsetDepartment
    public string DeptGrp { get; set; } = null!;
    public string SetsetupName { get; set; } = null!;
    public decimal? SuggestedQuantity { get; set; }


  }
}
