namespace Rajby_web.Models
{
  public class PreCostingViewModel
  {
    // Fields from cmsPreCosting
    public long CostingId { get; set; } // Primary Key
    public string ?CostingIdEncrypted { get; set; } // Encrypted CostingId
    public string CostingNumber { get; set; } = null!;
    public DateTime CostingDate { get; set; }
    public float? MinExpectedPrice { get; set; }
    public float? SellPrice { get; set; }
    public string? CreatedBy { get; set; }
    public string? ApprovalStatus { get; set; }
    public double? ApprovedPrice { get; set; }

    // Field from lms_setArticle
    public string? ArticleCode { get; set; }

    // Field from setBuyer
    public string? BuyerName { get; set; }
    public string? CostingNumberEncrypted { get; set; }

  }
}
