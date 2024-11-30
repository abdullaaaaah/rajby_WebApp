using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class CmsPreCosting
{
    public long CostingId { get; set; }

    public string CostingNumber { get; set; } = null!;

    public string? DocPrefix { get; set; }

    public string? DocSuffix { get; set; }

    public DateTime CostingDate { get; set; }

    public long? ArticleId { get; set; }

    public string ArticleCode { get; set; } = null!;

    public string? BuyerId { get; set; }

    public string? Style { get; set; }

    public string? Pireference { get; set; }

    public float? OrderQty { get; set; }

    public long? ExportTermsId { get; set; }

    public float? TotalEnds { get; set; }

    public float? Reed { get; set; }

    public int? EndsperDent { get; set; }

    public double? ReedWidth { get; set; }

    public double? EndsperInch { get; set; }

    public double? PicksperInch { get; set; }

    public double? PicksForCostDetail { get; set; }

    public double? ReedSpace { get; set; }

    public string? Color { get; set; }

    public double? Weight { get; set; }

    public double? Width { get; set; }

    public int? Weave1 { get; set; }

    public int? Weave2 { get; set; }

    public string? TwillDirection { get; set; }

    public string? WpQuality1 { get; set; }

    public string? WpSupplier1 { get; set; }

    public float? WpCount1 { get; set; }

    public float? WpRatio1 { get; set; }

    public float? WpEpi1 { get; set; }

    public double? WpFmtrLbs1 { get; set; }

    public double? WpWastage1 { get; set; }

    public double? WpYarnCons1 { get; set; }

    public float? WpRateLbs1 { get; set; }

    public DateTime? WpRateDt1 { get; set; }

    public float? WpCostMtr1 { get; set; }

    public float? WpPolyCount1 { get; set; }

    public string? WpQuality2 { get; set; }

    public string? WpSupplier2 { get; set; }

    public float? WpCount2 { get; set; }

    public float? WpRatio2 { get; set; }

    public float? WpEpi2 { get; set; }

    public double? WpFmtrLbs2 { get; set; }

    public double? WpWastage2 { get; set; }

    public double? WpYarnCons2 { get; set; }

    public float? WpRateLbs2 { get; set; }

    public DateTime? WpRateDt2 { get; set; }

    public float? WpCostMtr2 { get; set; }

    public float? WpPolyCount2 { get; set; }

    public string? WpQuality3 { get; set; }

    public string? WpSupplier3 { get; set; }

    public float? WpCount3 { get; set; }

    public float? WpRatio3 { get; set; }

    public float? WpEpi3 { get; set; }

    public double? WpFmtrLbs3 { get; set; }

    public double? WpWastage3 { get; set; }

    public double? WpYarnCons3 { get; set; }

    public float? WpRateLbs3 { get; set; }

    public DateTime? WpRateDt3 { get; set; }

    public float? WpCostMtr3 { get; set; }

    public float? WpPolyCount3 { get; set; }

    public string? WpQuality4 { get; set; }

    public string? WpSupplier4 { get; set; }

    public float? WpCount4 { get; set; }

    public float? WpRatio4 { get; set; }

    public float? WpEpi4 { get; set; }

    public double? WpFmtrLbs4 { get; set; }

    public double? WpWastage4 { get; set; }

    public double? WpYarnCons4 { get; set; }

    public float? WpRateLbs4 { get; set; }

    public DateTime? WpRateDt4 { get; set; }

    public float? WpCostMtr4 { get; set; }

    public float? WpPolyCount4 { get; set; }

    public string? WtQuality1 { get; set; }

    public string? WtSupplier1 { get; set; }

    public float? WtCount1 { get; set; }

    public float? WtRatio1 { get; set; }

    public float? WtPpi1 { get; set; }

    public double? WtFmtrLbs1 { get; set; }

    public double? WtWastage1 { get; set; }

    public double WtYarnCons1 { get; set; }

    public float? WtRateLbs1 { get; set; }

    public DateTime? WtRateDt1 { get; set; }

    public float? WtCostMtr1 { get; set; }

    public float? WtPolyCount1 { get; set; }

    public string? WtQuality2 { get; set; }

    public string? Wtsupplier2 { get; set; }

    public float? WtCount2 { get; set; }

    public float? WtRatio2 { get; set; }

    public float? WtPpi2 { get; set; }

    public double? WtFmtrLbs2 { get; set; }

    public double? WtWastage2 { get; set; }

    public double? WtYarnCons2 { get; set; }

    public float? WtRateLbs2 { get; set; }

    public DateTime? WtRateDt2 { get; set; }

    public float? WtCostMtr2 { get; set; }

    public float? WtPolyCount2 { get; set; }

    public string? WtQuality3 { get; set; }

    public string? Wtsupplier3 { get; set; }

    public float? WtCount3 { get; set; }

    public float? WtRatio3 { get; set; }

    public float? WtPpi3 { get; set; }

    public double? WtFmtrLbs3 { get; set; }

    public double WtWastage3 { get; set; }

    public double? WtYarnCons3 { get; set; }

    public float? WtRateLbs3 { get; set; }

    public DateTime? WtRateDt3 { get; set; }

    public float? WtCostMtr3 { get; set; }

    public float? WtPolyCount3 { get; set; }

    public string? WtQuality4 { get; set; }

    public string? Wtsupplier4 { get; set; }

    public float? WtCount4 { get; set; }

    public float? WtRatio4 { get; set; }

    public float? WtPpi4 { get; set; }

    public double? WtFmtrLbs4 { get; set; }

    public double? WtWastage4 { get; set; }

    public double? WtYarnCons4 { get; set; }

    public float? WtRateLbs4 { get; set; }

    public DateTime? WtRateDt4 { get; set; }

    public float? WtCostMtr4 { get; set; }

    public float? WtPolyCount4 { get; set; }

    public float? YarnCostMtr { get; set; }

    public float? OtherConvCost { get; set; }

    public float? DyesizeCost { get; set; }

    public float? DepMachine { get; set; }

    public float? DepAdmin { get; set; }

    public float? FinCostNewMach { get; set; }

    public float? SpFinDyeCost { get; set; }

    public float? TotalProdCost { get; set; }

    public float? Fcpcnt { get; set; }

    public float? FinanceCost { get; set; }

    public float? ClearForwardCost { get; set; }

    public float? ExportRchgsTxs { get; set; }

    public float? ExpSpecificCost { get; set; }

    public float? CostExclInsFreight { get; set; }

    public float? Freight { get; set; }

    public float? Insurance { get; set; }

    public float? CommMerchandiser { get; set; }

    public float? TotalExportCost { get; set; }

    public float? ExchangeRateCurr { get; set; }

    public int? Crdays { get; set; }

    public int? DaysinYear { get; set; }

    public float? Margin { get; set; }

    public float? ExchangeRateCost { get; set; }

    public float? SellPrice { get; set; }

    public float? CommPcnt { get; set; }

    public float? CommExport { get; set; }

    public float? ThreePpcnt { get; set; }

    public float? Comm3rdParty { get; set; }

    public float? PmpcntUs { get; set; }

    public float? ProfitMarginUs { get; set; }

    public float? PmpcntPkr { get; set; }

    public float? ProfitMarginPkr { get; set; }

    public float? PrftPcnt { get; set; }

    public float? MinPrftPcnt { get; set; }

    public float? ExpectedPrice { get; set; }

    public float? MinexpectedPrice { get; set; }

    public long? Locked { get; set; }

    public bool? DeleteTag { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateOn { get; set; }

    public string? CreateComp { get; set; }

    public string? ModifyBy { get; set; }

    public DateTime? ModifyOn { get; set; }

    public string? ModifyComp { get; set; }

    public int? Unitid { get; set; }

    public double? WstgPcnt { get; set; }

    public long? CurrencyId { get; set; }

    public double? WstgPcntWt { get; set; }

    public string? LocExp { get; set; }

    public double? WpCushionPcnt { get; set; }

    public double? WtCushionPcnt { get; set; }

    public double? WarpBpcnt { get; set; }

    public double? WeftBpcnt { get; set; }

    public double? CstTotalOrderQty { get; set; }

    public long? LengthUomid { get; set; }

    public double? ActPriceWdProfit { get; set; }

    public string? Remarks { get; set; }

    public long? FabricTypeId { get; set; }

    public double? MaxCushion { get; set; }

    public double? MinCushion { get; set; }

    public double? MinPricePcnt { get; set; }

    public double? MaxPricePcnt { get; set; }

    public double? ProfitPcnt { get; set; }

    public string? Approvalstatus { get; set; }

    public string? Status { get; set; }

    public double? ApprovedPrice { get; set; }

    public long? CommentsId { get; set; }

    public virtual LmsSetArticle? Article { get; set; }

    public virtual SetBuyer? Buyer { get; set; }

    public virtual ICollection<CmsApprovalHistory> CmsApprovalHistories { get; set; } = new List<CmsApprovalHistory>();

    public virtual SetSetup? ExportTerms { get; set; }
}
