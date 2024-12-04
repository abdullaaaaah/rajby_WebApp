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
    public string DeptGrp { get; set; } = null!;

  }
}
