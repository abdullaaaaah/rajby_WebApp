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

    // Properties from VsetDepartment (customized for your query)
    public string? DeptGroup { get; set; } // Mapped from "deptDet + ' - ' + deptGrp"
  }
}
