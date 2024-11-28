using System;
using System.Collections.Generic;

namespace Rajby_web.Models;

public partial class LmsSetArticle
{
    public long ArticleId { get; set; }

    public DateTime ArticleDate { get; set; }

    public string ArticleCode { get; set; } = null!;

    public string? Modification { get; set; }

    public string? ArticleSubCode { get; set; }

    public string? ArticleFor { get; set; }

    public string? ArticleDesc { get; set; }

    public bool? Main { get; set; }

    public string? MainCode { get; set; }

    public string? Reference { get; set; }

    public string? QualityCode { get; set; }

    public string? FinishCode { get; set; }

    public int? Comp1Pcnt { get; set; }

    public string? Comp1Code { get; set; }

    public int? Comp2Pcnt { get; set; }

    public string? Comp2Code { get; set; }

    public int? Comp3Pcnt { get; set; }

    public string? Comp3Code { get; set; }

    public int? Comp4Pcnt { get; set; }

    public string? Comp4Code { get; set; }

    public string? WpYarnId1 { get; set; }

    public string SplrWpCount1 { get; set; } = null!;

    public string? WarpCount1 { get; set; }

    public float? Total1Warps { get; set; }

    public string? WpQuality1 { get; set; }

    public float? Count1 { get; set; }

    public string? WpYarnId2 { get; set; }

    public string? SplrWpCount2 { get; set; }

    public string? WarpCount2 { get; set; }

    public float? Total2Warps { get; set; }

    public string? WpQuality2 { get; set; }

    public float? Count2 { get; set; }

    public string? WpYarnId3 { get; set; }

    public string? SplrWpCount3 { get; set; }

    public string? WarpCount3 { get; set; }

    public float? Total3Warps { get; set; }

    public string? WpQuality3 { get; set; }

    public float? Count3 { get; set; }

    public string? WpYarnId4 { get; set; }

    public string? SplrWpCount4 { get; set; }

    public string? WarpCount4 { get; set; }

    public float? Total4Warps { get; set; }

    public string? WpQuality4 { get; set; }

    public float? Count4 { get; set; }

    public string? WtYarnId1 { get; set; }

    public string? SplrWtCount1 { get; set; }

    public string? WeftCount1 { get; set; }

    public float? Total1Weft { get; set; }

    public string? WtQuality1 { get; set; }

    public float? Count5 { get; set; }

    public string? WtYarnId2 { get; set; }

    public string? SplrWtCount2 { get; set; }

    public string? WeftCount2 { get; set; }

    public float? Total2Weft { get; set; }

    public string? WtQuality2 { get; set; }

    public float? Count6 { get; set; }

    public string? WtYarnId3 { get; set; }

    public string? SplrWtCount3 { get; set; }

    public string? WeftCount3 { get; set; }

    public float? Total3Weft { get; set; }

    public string? WtQuality3 { get; set; }

    public float? Count7 { get; set; }

    public string? WtYarnId4 { get; set; }

    public string? SplrWtCount4 { get; set; }

    public string? WeftCount4 { get; set; }

    public float? Total4Weft { get; set; }

    public string? WtQuality4 { get; set; }

    public float? Count8 { get; set; }

    public float? ReedNumber { get; set; }

    public double? ReedWidth { get; set; }

    public string? GroundYarns { get; set; }

    public double? MechPickDensity { get; set; }

    public double? ActualWeftDensity { get; set; }

    public string? DyeingProcess { get; set; }

    public string? LotNumber { get; set; }

    public string? BeamNumber { get; set; }

    public string? LoomNumber { get; set; }

    public int? Weave1 { get; set; }

    public int? Weave2 { get; set; }

    public double? CalcWeave1 { get; set; }

    public double? CalcWeave2 { get; set; }

    public string? TwillDirection { get; set; }

    public double? WarpLength { get; set; }

    public double? FinishMtr { get; set; }

    public float? GreigeWeight { get; set; }

    public string? GreigeWidth { get; set; }

    public float? GreigeEnds { get; set; }

    public float? GreigePicks { get; set; }

    public float? FinishWeight { get; set; }

    public string? FinishWidth { get; set; }

    public float? FinishWeight1 { get; set; }

    public string? FinishWidth1 { get; set; }

    public float? FinishEnds { get; set; }

    public float? FinishPicks { get; set; }

    public float? WashWeight { get; set; }

    public string? WashWidth { get; set; }

    public float? WashEnds { get; set; }

    public float? WashPicks { get; set; }

    public string? WashWarpShrinkage { get; set; }

    public string? WashWeftShrinkage { get; set; }

    public string? WashSkewShrinkage { get; set; }

    public string? SelvedgeType { get; set; }

    public string? SelvedgeWeave { get; set; }

    public double? SelvedgeEnds { get; set; }

    public bool? DeleteTag { get; set; }

    /// <summary>
    /// 1= Lock 0= Unlock
    /// </summary>
    public long? LockStatus { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateOn { get; set; }

    public string? CreateComp { get; set; }

    public string? ModifyBy { get; set; }

    public DateTime? ModifyOn { get; set; }

    public string? ModifyComp { get; set; }

    public double? BoilGreigeWeight { get; set; }

    public string? BoilGreigeWidth { get; set; }

    public double? BoilGreigeEnds { get; set; }

    public double? BoilGreigePicks { get; set; }

    public string? BoilGreigeWarpShrinkage { get; set; }

    public string? BoilGreigeWeftShrinkage { get; set; }

    public string? BoilGreigeSkewShrinkage { get; set; }

    public double? BoilwashWeight { get; set; }

    public string? BoilwashWidth { get; set; }

    public double? BoilwashEnds { get; set; }

    public double? BoilwashPicks { get; set; }

    public string? BoilWarpShrinkage { get; set; }

    public string? BoilWeftShrinkage { get; set; }

    public string? BoilSkewShrinkage { get; set; }

    public int? UnitId { get; set; }

    public string? Remarks { get; set; }

    public long? DyeProcessId { get; set; }

    /// <summary>
    /// S = Sample, P=Production
    /// </summary>
    public string? ArticleType { get; set; }

    public long? DyeMainProcessId { get; set; }

    public double? TotalEndsS { get; set; }

    public double? Ppis { get; set; }

    public double? WpCushionPcnt { get; set; }

    public double? WtCushionPcnt { get; set; }

    public string? ColorRefNo { get; set; }

    public bool? InvolvedinCosting { get; set; }

    public long? ArticleCategoryId { get; set; }

    public long? Finishtypeid { get; set; }

    public long? WideArticleId { get; set; }

    public bool? Overhead { get; set; }

    public long? CoatingTypeId { get; set; }

    public string? Stno { get; set; }

    public long? FinishtypeSubid { get; set; }

    public long? FabricDesignId { get; set; }

    public double? DyeSizeCost { get; set; }

    public double? CrimpingPcnt { get; set; }

    public double? FinishingPcnt { get; set; }

    public long? FinishTypeCategoryId { get; set; }

    public double? FinishTypeCategoryCost { get; set; }

    public double? FinishTypeCategoryCushion { get; set; }

    public double? PicksForCostDetail { get; set; }

    public long? FabricTypeid { get; set; }

    public string? Hscode { get; set; }

    public string? HssubCode { get; set; }

    public string? OtherRemarks { get; set; }

    public double? WarpWstPcnt { get; set; }

    public double? WeftWstPcnt { get; set; }

    public double? ShrinkagePcnt { get; set; }

    public string? ServiceCode { get; set; }

    public string? BoilOffLabStretch { get; set; }

    public double? CutableWidth { get; set; }

    public string? CompositionString { get; set; }

    public virtual ICollection<CmsPreCosting> CmsPreCostings { get; set; } = new List<CmsPreCosting>();
}
