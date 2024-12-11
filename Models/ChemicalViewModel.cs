namespace Rajby_web.Models
{
  public class ChemicalViewModel
  {
    public long RequisitionId { get; set; }

    public string? DocId { get; set; }

    public DateTime? DocDt { get; set; }

    public long? DeptId { get; set; }

    public long? StoreId { get; set; }

    public string? Comments { get; set; }
    public string? EncryptedItemId { get; set; }


    // Properties from VsetDepartment (customized for your query)
    public string? DeptGroup { get; set; } // Mapped from "deptDet + ' - ' + deptGrp"

    public long SetsetupId { get; set; }
    public string UOMName { get; set; } = null!;
    // New properties as per your query
    public string? RDComment { get; set; } // Comments from PmsRequisitionDetCD
    public string? EncryptedRequisitionNumber { get; set; } // Comments from PmsRequisitionDetCD

    public long? ItemId { get; set; } // ItemId from PmsRequisitionDetCD
    public decimal? AvailableQty { get; set; } // AvailableQty from PmsRequisitionDetCD\
    public decimal? QtyToProcure { get; set; } // AvailableQty from PmsRequisitionDetCD\
   
    public string? ItemName { get; set; }

    public long RequisitionDetId { get; set; }

    public string? Status { get; set; }


  }
}
