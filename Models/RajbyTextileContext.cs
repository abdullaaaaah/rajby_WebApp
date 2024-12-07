using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Rajby_web.Models;

public partial class RajbyTextileContext : DbContext
{
    public RajbyTextileContext()
    {
    }

    public RajbyTextileContext(DbContextOptions<RajbyTextileContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CmsApprovalHistory> CmsApprovalHistories { get; set; }

    public virtual DbSet<CmsPreCosting> CmsPreCostings { get; set; }

    public virtual DbSet<LmsSetArticle> LmsSetArticles { get; set; }

    public virtual DbSet<PmsChemicalHistory> PmsChemicalHistories { get; set; }

    public virtual DbSet<PmsRequisition> PmsRequisitions { get; set; }

    public virtual DbSet<PmsRequisitionCd> PmsRequisitionCds { get; set; }

    public virtual DbSet<PmsRequisitionDetCd> PmsRequisitionDetCds { get; set; }

    public virtual DbSet<PmsRequisitionDetGsp> PmsRequisitionDetGsps { get; set; }

    public virtual DbSet<PmsRequisitionHistory> PmsRequisitionHistories { get; set; }

    public virtual DbSet<SetBuyer> SetBuyers { get; set; }

    public virtual DbSet<SetDepartment> SetDepartments { get; set; }

    public virtual DbSet<SetItemCd> SetItemCds { get; set; }

    public virtual DbSet<SetSetup> SetSetups { get; set; }

    public virtual DbSet<SmsUser> SmsUsers { get; set; }

    public virtual DbSet<VsetDepartment> VsetDepartments { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CmsApprovalHistory>(entity =>
        {
            entity.HasKey(e => e.AutoGenId);

            entity.ToTable("cmsApprovalHistory");

            entity.Property(e => e.Approvalstatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusChangedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusChangedComp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusChangedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Comments).WithMany(p => p.CmsApprovalHistories).HasForeignKey(d => d.CommentsId);

            entity.HasOne(d => d.Costing).WithMany(p => p.CmsApprovalHistories)
                .HasForeignKey(d => d.CostingId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CmsPreCosting>(entity =>
        {
            entity.HasKey(e => e.CostingId);

            entity.ToTable("cmsPreCosting", tb => tb.HasTrigger("trg_cms_Precosting_Updated"));

            entity.HasIndex(e => e.ArticleId, "IX_cmsPreCosting_ArticleId");

            entity.HasIndex(e => e.BuyerId, "IX_cmsPreCosting_BuyerId");

            entity.HasIndex(e => e.CurrencyId, "IX_cmsPreCosting_CurrencyId");

            entity.HasIndex(e => e.ExportTermsId, "IX_cmsPreCosting_ExportTermsId");

            entity.HasIndex(e => e.FabricTypeId, "IX_cmsPreCosting_FabricTypeId");

            entity.HasIndex(e => e.LengthUomid, "IX_cmsPreCosting_LengthUOMId");

            entity.HasIndex(e => new { e.WpSupplier1, e.WpSupplier2, e.WpSupplier3, e.WpSupplier4 }, "IX_cmsPreCosting_Supplier");

            entity.HasIndex(e => e.CostingNumber, "NC_IX_CostingNumber");

            entity.Property(e => e.CostingId).HasColumnName("costingId");
            entity.Property(e => e.Approvalstatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ArticleCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("articleCode");
            entity.Property(e => e.ArticleId).HasColumnName("articleId");
            entity.Property(e => e.BuyerId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("buyerId");
            entity.Property(e => e.ClearForwardCost).HasColumnName("clearForwardCost");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("color");
            entity.Property(e => e.Comm3rdParty).HasColumnName("comm3rdParty");
            entity.Property(e => e.CommExport).HasColumnName("commExport");
            entity.Property(e => e.CommMerchandiser).HasColumnName("commMerchandiser");
            entity.Property(e => e.CommPcnt).HasColumnName("commPcnt");
            entity.Property(e => e.CostExclInsFreight).HasColumnName("costExclInsFreight");
            entity.Property(e => e.CostingDate)
                .HasColumnType("datetime")
                .HasColumnName("costingDate");
            entity.Property(e => e.CostingNumber)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("costingNumber");
            entity.Property(e => e.Crdays).HasColumnName("crdays");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createBy");
            entity.Property(e => e.CreateComp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateOn)
                .HasColumnType("datetime")
                .HasColumnName("createOn");
            entity.Property(e => e.CstTotalOrderQty).HasColumnName("cstTotalOrderQty");
            entity.Property(e => e.CurrencyId).HasColumnName("currencyId");
            entity.Property(e => e.DaysinYear).HasColumnName("daysinYear");
            entity.Property(e => e.DeleteTag).HasColumnName("deleteTag");
            entity.Property(e => e.DepAdmin).HasColumnName("depAdmin");
            entity.Property(e => e.DepMachine).HasColumnName("depMachine");
            entity.Property(e => e.DocPrefix)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("docPrefix");
            entity.Property(e => e.DocSuffix)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("docSuffix");
            entity.Property(e => e.DyesizeCost).HasColumnName("dyesizeCost");
            entity.Property(e => e.EndsperDent).HasColumnName("endsperDent");
            entity.Property(e => e.EndsperInch).HasColumnName("endsperInch");
            entity.Property(e => e.ExpSpecificCost).HasColumnName("expSpecificCost");
            entity.Property(e => e.ExpectedPrice).HasColumnName("expectedPrice");
            entity.Property(e => e.ExportRchgsTxs).HasColumnName("exportRChgsTxs");
            entity.Property(e => e.ExportTermsId).HasColumnName("exportTermsId");
            entity.Property(e => e.Fcpcnt).HasColumnName("FCPcnt");
            entity.Property(e => e.FinCostNewMach).HasColumnName("finCostNewMach");
            entity.Property(e => e.FinanceCost).HasColumnName("financeCost");
            entity.Property(e => e.Freight).HasColumnName("freight");
            entity.Property(e => e.Insurance).HasColumnName("insurance");
            entity.Property(e => e.LengthUomid).HasColumnName("LengthUOMId");
            entity.Property(e => e.LocExp)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Locked).HasColumnName("locked");
            entity.Property(e => e.ModifyBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modifyBy");
            entity.Property(e => e.ModifyComp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modifyComp");
            entity.Property(e => e.ModifyOn)
                .HasColumnType("datetime")
                .HasColumnName("modifyOn");
            entity.Property(e => e.OrderQty).HasColumnName("orderQty");
            entity.Property(e => e.OtherConvCost).HasColumnName("otherConvCost");
            entity.Property(e => e.PicksperInch).HasColumnName("picksperInch");
            entity.Property(e => e.Pireference)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PIReference");
            entity.Property(e => e.PmpcntPkr).HasColumnName("PMPcntPKR");
            entity.Property(e => e.PmpcntUs).HasColumnName("PMPcntUS");
            entity.Property(e => e.ProfitMarginPkr).HasColumnName("ProfitMarginPKR");
            entity.Property(e => e.ProfitMarginUs).HasColumnName("profitMarginUS");
            entity.Property(e => e.ProfitMarginUsd).HasColumnName("ProfitMarginUSD");
            entity.Property(e => e.ProfitPcntPkr).HasColumnName("ProfitPcntPKR");
            entity.Property(e => e.ProfitPcntUsd).HasColumnName("ProfitPcntUSD");
            entity.Property(e => e.Reed).HasColumnName("reed");
            entity.Property(e => e.ReedSpace).HasColumnName("reedSpace");
            entity.Property(e => e.ReedWidth).HasColumnName("reedWidth");
            entity.Property(e => e.SellPrice).HasColumnName("sellPrice");
            entity.Property(e => e.SpFinDyeCost).HasColumnName("spFinDyeCost");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Style)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("style");
            entity.Property(e => e.ThreePpcnt).HasColumnName("threePPcnt");
            entity.Property(e => e.TotalEnds).HasColumnName("totalEnds");
            entity.Property(e => e.TotalExportCost).HasColumnName("totalExportCost");
            entity.Property(e => e.TotalProdCost).HasColumnName("totalProdCost");
            entity.Property(e => e.TwillDirection)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("twillDirection");
            entity.Property(e => e.Unitid).HasColumnName("unitid");
            entity.Property(e => e.WarpBpcnt).HasColumnName("WarpBPcnt");
            entity.Property(e => e.Weave1).HasColumnName("weave1");
            entity.Property(e => e.Weave2).HasColumnName("weave2");
            entity.Property(e => e.WeftBpcnt).HasColumnName("WeftBPcnt");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.Width).HasColumnName("width");
            entity.Property(e => e.WpCostMtr1).HasColumnName("wpCostMtr1");
            entity.Property(e => e.WpCostMtr2).HasColumnName("wpCostMtr2");
            entity.Property(e => e.WpCostMtr3).HasColumnName("wpCostMtr3");
            entity.Property(e => e.WpCostMtr4).HasColumnName("wpCostMtr4");
            entity.Property(e => e.WpCount1).HasColumnName("wpCount1");
            entity.Property(e => e.WpCount2).HasColumnName("wpCount2");
            entity.Property(e => e.WpCount3).HasColumnName("wpCount3");
            entity.Property(e => e.WpCount4).HasColumnName("wpCount4");
            entity.Property(e => e.WpCushionPcnt).HasColumnName("wpCushionPcnt");
            entity.Property(e => e.WpEpi1).HasColumnName("wpEPI1");
            entity.Property(e => e.WpEpi2).HasColumnName("wpEPI2");
            entity.Property(e => e.WpEpi3).HasColumnName("wpEPI3");
            entity.Property(e => e.WpEpi4).HasColumnName("wpEPI4");
            entity.Property(e => e.WpFmtrLbs1).HasColumnName("wpFMtrLbs1");
            entity.Property(e => e.WpFmtrLbs2).HasColumnName("wpFMtrLbs2");
            entity.Property(e => e.WpFmtrLbs3).HasColumnName("wpFMtrLbs3");
            entity.Property(e => e.WpFmtrLbs4).HasColumnName("wpFMtrLbs4");
            entity.Property(e => e.WpPolyCount1).HasColumnName("wpPolyCount1");
            entity.Property(e => e.WpPolyCount2).HasColumnName("wpPolyCount2");
            entity.Property(e => e.WpPolyCount3).HasColumnName("wpPolyCount3");
            entity.Property(e => e.WpPolyCount4).HasColumnName("wpPolyCount4");
            entity.Property(e => e.WpQuality1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wpQuality1");
            entity.Property(e => e.WpQuality2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wpQuality2");
            entity.Property(e => e.WpQuality3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wpQuality3");
            entity.Property(e => e.WpQuality4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wpQuality4");
            entity.Property(e => e.WpRateDt1)
                .HasColumnType("datetime")
                .HasColumnName("wpRateDt1");
            entity.Property(e => e.WpRateDt2)
                .HasColumnType("datetime")
                .HasColumnName("wpRateDt2");
            entity.Property(e => e.WpRateDt3)
                .HasColumnType("datetime")
                .HasColumnName("wpRateDt3");
            entity.Property(e => e.WpRateDt4)
                .HasColumnType("datetime")
                .HasColumnName("wpRateDt4");
            entity.Property(e => e.WpRateLbs1).HasColumnName("wpRateLbs1");
            entity.Property(e => e.WpRateLbs2).HasColumnName("wpRateLbs2");
            entity.Property(e => e.WpRateLbs3).HasColumnName("wpRateLbs3");
            entity.Property(e => e.WpRateLbs4).HasColumnName("wpRateLbs4");
            entity.Property(e => e.WpRatio1).HasColumnName("wpRatio1");
            entity.Property(e => e.WpRatio2).HasColumnName("wpRatio2");
            entity.Property(e => e.WpRatio3).HasColumnName("wpRatio3");
            entity.Property(e => e.WpRatio4).HasColumnName("wpRatio4");
            entity.Property(e => e.WpSupplier1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("wpSupplier1");
            entity.Property(e => e.WpSupplier2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("wpSupplier2");
            entity.Property(e => e.WpSupplier3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("wpSupplier3");
            entity.Property(e => e.WpSupplier4)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("wpSupplier4");
            entity.Property(e => e.WpWastage1).HasColumnName("wpWastage1");
            entity.Property(e => e.WpWastage2).HasColumnName("wpWastage2");
            entity.Property(e => e.WpWastage3).HasColumnName("wpWastage3");
            entity.Property(e => e.WpWastage4).HasColumnName("wpWastage4");
            entity.Property(e => e.WpYarnCons1).HasColumnName("wpYarnCons1");
            entity.Property(e => e.WpYarnCons2).HasColumnName("wpYarnCons2");
            entity.Property(e => e.WpYarnCons3).HasColumnName("wpYarnCons3");
            entity.Property(e => e.WpYarnCons4).HasColumnName("wpYarnCons4");
            entity.Property(e => e.WstgPcnt).HasColumnName("wstgPcnt");
            entity.Property(e => e.WstgPcntWt).HasColumnName("wstgPcntWt");
            entity.Property(e => e.WtCostMtr1).HasColumnName("wtCostMtr1");
            entity.Property(e => e.WtCostMtr2).HasColumnName("wtCostMtr2");
            entity.Property(e => e.WtCostMtr3).HasColumnName("wtCostMtr3");
            entity.Property(e => e.WtCostMtr4).HasColumnName("wtCostMtr4");
            entity.Property(e => e.WtCount1).HasColumnName("wtCount1");
            entity.Property(e => e.WtCount2).HasColumnName("wtCount2");
            entity.Property(e => e.WtCount3).HasColumnName("wtCount3");
            entity.Property(e => e.WtCount4).HasColumnName("wtCount4");
            entity.Property(e => e.WtCushionPcnt).HasColumnName("wtCushionPcnt");
            entity.Property(e => e.WtFmtrLbs1).HasColumnName("wtFMtrLbs1");
            entity.Property(e => e.WtFmtrLbs2).HasColumnName("wtFMtrLbs2");
            entity.Property(e => e.WtFmtrLbs3).HasColumnName("wtFMtrLbs3");
            entity.Property(e => e.WtFmtrLbs4).HasColumnName("wtFMtrLbs4");
            entity.Property(e => e.WtPolyCount1).HasColumnName("wtPolyCount1");
            entity.Property(e => e.WtPolyCount2).HasColumnName("wtPolyCount2");
            entity.Property(e => e.WtPolyCount3).HasColumnName("wtPolyCount3");
            entity.Property(e => e.WtPolyCount4).HasColumnName("wtPolyCount4");
            entity.Property(e => e.WtPpi1).HasColumnName("wtPPI1");
            entity.Property(e => e.WtPpi2).HasColumnName("wtPPI2");
            entity.Property(e => e.WtPpi3).HasColumnName("wtPPI3");
            entity.Property(e => e.WtPpi4).HasColumnName("wtPPI4");
            entity.Property(e => e.WtQuality1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wtQuality1");
            entity.Property(e => e.WtQuality2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wtQuality2");
            entity.Property(e => e.WtQuality3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wtQuality3");
            entity.Property(e => e.WtQuality4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wtQuality4");
            entity.Property(e => e.WtRateDt1)
                .HasColumnType("datetime")
                .HasColumnName("wtRateDt1");
            entity.Property(e => e.WtRateDt2)
                .HasColumnType("datetime")
                .HasColumnName("wtRateDt2");
            entity.Property(e => e.WtRateDt3)
                .HasColumnType("datetime")
                .HasColumnName("wtRateDt3");
            entity.Property(e => e.WtRateDt4)
                .HasColumnType("datetime")
                .HasColumnName("wtRateDt4");
            entity.Property(e => e.WtRateLbs1).HasColumnName("wtRateLbs1");
            entity.Property(e => e.WtRateLbs2).HasColumnName("wtRateLbs2");
            entity.Property(e => e.WtRateLbs3).HasColumnName("wtRateLbs3");
            entity.Property(e => e.WtRateLbs4).HasColumnName("wtRateLbs4");
            entity.Property(e => e.WtRatio1).HasColumnName("wtRatio1");
            entity.Property(e => e.WtRatio2).HasColumnName("wtRatio2");
            entity.Property(e => e.WtRatio3).HasColumnName("wtRatio3");
            entity.Property(e => e.WtRatio4).HasColumnName("wtRatio4");
            entity.Property(e => e.WtSupplier1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("wtSupplier1");
            entity.Property(e => e.WtWastage1).HasColumnName("wtWastage1");
            entity.Property(e => e.WtWastage2).HasColumnName("wtWastage2");
            entity.Property(e => e.WtWastage3).HasColumnName("wtWastage3");
            entity.Property(e => e.WtWastage4).HasColumnName("wtWastage4");
            entity.Property(e => e.WtYarnCons1).HasColumnName("wtYarnCons1");
            entity.Property(e => e.WtYarnCons2).HasColumnName("wtYarnCons2");
            entity.Property(e => e.WtYarnCons3).HasColumnName("wtYarnCons3");
            entity.Property(e => e.WtYarnCons4).HasColumnName("wtYarnCons4");
            entity.Property(e => e.Wtsupplier2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("wtsupplier2");
            entity.Property(e => e.Wtsupplier3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("wtsupplier3");
            entity.Property(e => e.Wtsupplier4)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("wtsupplier4");
            entity.Property(e => e.YarnCostMtr).HasColumnName("yarnCostMtr");

            entity.HasOne(d => d.Article).WithMany(p => p.CmsPreCostings)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_cmsPreCosting_lms_setArticle");

            entity.HasOne(d => d.Buyer).WithMany(p => p.CmsPreCostings)
                .HasForeignKey(d => d.BuyerId)
                .HasConstraintName("FK_cmsPreCosting_setBuyer");

            entity.HasOne(d => d.ExportTerms).WithMany(p => p.CmsPreCostings)
                .HasForeignKey(d => d.ExportTermsId)
                .HasConstraintName("FK_cmsPreCosting_setSetup");
        });

        modelBuilder.Entity<LmsSetArticle>(entity =>
        {
            entity.HasKey(e => e.ArticleId);

            entity.ToTable("lms_setArticle", tb => tb.HasTrigger("trg_lms_setarticle_Identify_Updated_Columns"));

            entity.HasIndex(e => e.ArticleId, "IX_lms_setArticle").IsUnique();

            entity.HasIndex(e => e.Finishtypeid, "IX_lms_setArticle_FinishTypeId");

            entity.HasIndex(e => e.UnitId, "IX_lms_setArticle_UnitId");

            entity.HasIndex(e => new { e.WpYarnId1, e.WpYarnId2, e.WpYarnId3, e.WpYarnId4 }, "IX_lms_setArticle_Wpyarnid");

            entity.HasIndex(e => new { e.WtYarnId1, e.WtYarnId2, e.WtYarnId3, e.WtYarnId4 }, "IX_lms_setArticle_WtYarnId");

            entity.HasIndex(e => e.DeleteTag, "NC_IX_DeleteTagInclFlds");

            entity.HasIndex(e => e.ArticleCode, "UNQ_ACode").IsUnique();

            entity.HasIndex(e => e.ArticleCode, "UNQ_ArticleCode").IsUnique();

            entity.Property(e => e.ArticleId).HasColumnName("articleId");
            entity.Property(e => e.ActualWeftDensity).HasColumnName("actualWeftDensity");
            entity.Property(e => e.ArticleCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("articleCode");
            entity.Property(e => e.ArticleDate)
                .HasColumnType("datetime")
                .HasColumnName("articleDate");
            entity.Property(e => e.ArticleDesc)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("articleDesc");
            entity.Property(e => e.ArticleFor)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("articleFor");
            entity.Property(e => e.ArticleSubCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("articleSubCode");
            entity.Property(e => e.ArticleType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("S = Sample, P=Production");
            entity.Property(e => e.BeamNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilGreigeSkewShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilGreigeWarpShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilGreigeWeftShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilGreigeWidth)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilOffLabStretch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilSkewShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilWarpShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilWeftShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BoilwashWidth)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CalcWeave1).HasColumnName("calcWeave1");
            entity.Property(e => e.CalcWeave2).HasColumnName("calcWeave2");
            entity.Property(e => e.ColorRefNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("colorRefNo");
            entity.Property(e => e.Comp1Code)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("comp1Code");
            entity.Property(e => e.Comp1Pcnt).HasColumnName("comp1Pcnt");
            entity.Property(e => e.Comp2Code)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("comp2Code");
            entity.Property(e => e.Comp2Pcnt).HasColumnName("comp2Pcnt");
            entity.Property(e => e.Comp3Code)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("comp3Code");
            entity.Property(e => e.Comp3Pcnt).HasColumnName("comp3Pcnt");
            entity.Property(e => e.Comp4Code)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("comp4Code");
            entity.Property(e => e.Comp4Pcnt).HasColumnName("comp4Pcnt");
            entity.Property(e => e.CompositionString)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Count1).HasColumnName("count1");
            entity.Property(e => e.Count2).HasColumnName("count2");
            entity.Property(e => e.Count3).HasColumnName("count3");
            entity.Property(e => e.Count4).HasColumnName("count4");
            entity.Property(e => e.Count5).HasColumnName("count5");
            entity.Property(e => e.Count6).HasColumnName("count6");
            entity.Property(e => e.Count7).HasColumnName("count7");
            entity.Property(e => e.Count8).HasColumnName("count8");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createBy");
            entity.Property(e => e.CreateComp)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CreateOn)
                .HasColumnType("datetime")
                .HasColumnName("createOn");
            entity.Property(e => e.CutableWidth).HasColumnName("cutableWidth");
            entity.Property(e => e.DeleteTag).HasColumnName("deleteTag");
            entity.Property(e => e.DyeMainProcessId).HasColumnName("dyeMainProcessId");
            entity.Property(e => e.DyeProcessId).HasColumnName("dyeProcessId");
            entity.Property(e => e.DyeingProcess)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("dyeingProcess");
            entity.Property(e => e.FinishCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("finishCode");
            entity.Property(e => e.FinishEnds).HasColumnName("finishEnds");
            entity.Property(e => e.FinishMtr).HasColumnName("finishMtr");
            entity.Property(e => e.FinishPicks).HasColumnName("finishPicks");
            entity.Property(e => e.FinishWeight).HasColumnName("finishWeight");
            entity.Property(e => e.FinishWeight1).HasColumnName("finishWeight1");
            entity.Property(e => e.FinishWidth)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("finishWidth");
            entity.Property(e => e.FinishWidth1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("finishWidth1");
            entity.Property(e => e.FinishtypeSubid).HasColumnName("finishtypeSubid");
            entity.Property(e => e.Finishtypeid).HasColumnName("finishtypeid");
            entity.Property(e => e.GreigeWidth)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GroundYarns)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("groundYarns");
            entity.Property(e => e.Hscode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HSCode");
            entity.Property(e => e.HssubCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HSSubCode");
            entity.Property(e => e.LockStatus)
                .HasDefaultValue(0L)
                .HasComment("1= Lock 0= Unlock")
                .HasColumnName("lockStatus");
            entity.Property(e => e.LoomNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LotNumber)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MainCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mainCode");
            entity.Property(e => e.MechPickDensity).HasColumnName("mechPickDensity");
            entity.Property(e => e.Modification)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("modification");
            entity.Property(e => e.ModifyBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modifyBy");
            entity.Property(e => e.ModifyComp)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifyOn)
                .HasColumnType("datetime")
                .HasColumnName("modifyOn");
            entity.Property(e => e.OtherRemarks).IsUnicode(false);
            entity.Property(e => e.Ppis).HasColumnName("PPIS");
            entity.Property(e => e.QualityCode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("qualityCode");
            entity.Property(e => e.ReedNumber).HasColumnName("reedNumber");
            entity.Property(e => e.ReedWidth).HasColumnName("reedWidth");
            entity.Property(e => e.Reference)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SelvedgeEnds).HasColumnName("selvedgeEnds");
            entity.Property(e => e.SelvedgeType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("selvedgeType");
            entity.Property(e => e.SelvedgeWeave)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("selvedgeWeave");
            entity.Property(e => e.ServiceCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SplrWpCount1)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SplrWpCount2)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SplrWpCount3)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SplrWpCount4)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SplrWtCount1)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SplrWtCount2)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SplrWtCount3)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SplrWtCount4)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Stno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STNo");
            entity.Property(e => e.Total1Warps).HasColumnName("total1Warps");
            entity.Property(e => e.Total1Weft).HasColumnName("total1Weft");
            entity.Property(e => e.Total2Warps).HasColumnName("total2Warps");
            entity.Property(e => e.Total2Weft).HasColumnName("total2Weft");
            entity.Property(e => e.Total3Warps).HasColumnName("total3Warps");
            entity.Property(e => e.Total3Weft).HasColumnName("total3Weft");
            entity.Property(e => e.Total4Warps).HasColumnName("total4Warps");
            entity.Property(e => e.Total4Weft).HasColumnName("total4Weft");
            entity.Property(e => e.TwillDirection)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("twillDirection");
            entity.Property(e => e.UnitId).HasColumnName("unitId");
            entity.Property(e => e.WarpCount1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarpCount2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarpCount3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarpCount4)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarpLength).HasColumnName("warpLength");
            entity.Property(e => e.WashEnds).HasColumnName("washEnds");
            entity.Property(e => e.WashPicks).HasColumnName("washPicks");
            entity.Property(e => e.WashSkewShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WashWarpShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("washWarpShrinkage");
            entity.Property(e => e.WashWeftShrinkage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("washWeftShrinkage");
            entity.Property(e => e.WashWeight).HasColumnName("washWeight");
            entity.Property(e => e.WashWidth)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("washWidth");
            entity.Property(e => e.Weave1).HasColumnName("weave1");
            entity.Property(e => e.Weave2).HasColumnName("weave2");
            entity.Property(e => e.WeftCount1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WeftCount2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WeftCount3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WeftCount4)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WpCushionPcnt).HasColumnName("wpCushionPcnt");
            entity.Property(e => e.WpQuality1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WpQuality2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WpQuality3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WpQuality4)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WpYarnId1)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.WpYarnId2)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.WpYarnId3)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.WpYarnId4)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.WtCushionPcnt).HasColumnName("wtCushionPcnt");
            entity.Property(e => e.WtQuality1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WtQuality2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WtQuality3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WtQuality4)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WtYarnId1)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.WtYarnId2)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.WtYarnId3)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.WtYarnId4)
                .HasMaxLength(14)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PmsChemicalHistory>(entity =>
        {
            entity.HasKey(e => e.AutoGenId);

            entity.ToTable("pmsChemicalHistory");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusChangedBy).HasMaxLength(100);
            entity.Property(e => e.StatusChangedComp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusChangedDate).HasColumnType("datetime");

            entity.HasOne(d => d.RequisitionDet).WithMany(p => p.PmsChemicalHistories)
                .HasForeignKey(d => d.RequisitionDetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Requisition).WithMany(p => p.PmsChemicalHistories)
                .HasForeignKey(d => d.RequisitionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PmsRequisition>(entity =>
        {
            entity.HasKey(e => e.RequisitionId).HasName("PK_pmsRequisitionGSP");

            entity.ToTable("pmsRequisition");

            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("approvedBy");
            entity.Property(e => e.ApprovedBy2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("approvedBy2");
            entity.Property(e => e.ApprovedComp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("approvedComp");
            entity.Property(e => e.ApprovedComp2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("approvedComp2");
            entity.Property(e => e.ApprovedOn)
                .HasColumnType("datetime")
                .HasColumnName("approvedOn");
            entity.Property(e => e.ApprovedOn2)
                .HasColumnType("datetime")
                .HasColumnName("approvedOn2");
            entity.Property(e => e.Comments)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Createby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createby");
            entity.Property(e => e.Createcomp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("createcomp");
            entity.Property(e => e.Createon)
                .HasColumnType("datetime")
                .HasColumnName("createon");
            entity.Property(e => e.DeptId).HasColumnName("deptId");
            entity.Property(e => e.DocDt).HasColumnType("datetime");
            entity.Property(e => e.DocId)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DocPrefix)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DocSuffix)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Mainid).HasColumnName("mainid");
            entity.Property(e => e.ModifyComp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("modifyComp");
            entity.Property(e => e.Modifyby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modifyby");
            entity.Property(e => e.Modifyon)
                .HasColumnType("datetime")
                .HasColumnName("modifyon");
            entity.Property(e => e.ReqTypeId).HasColumnName("reqTypeId");
            entity.Property(e => e.SinRetId).HasColumnName("sinRetId");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Dept).WithMany(p => p.PmsRequisitions)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_pmsRequisition_setDepartment");

            entity.HasOne(d => d.Store).WithMany(p => p.PmsRequisitions)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_pmsRequisition_setSetup");
        });

        modelBuilder.Entity<PmsRequisitionCd>(entity =>
        {
            entity.HasKey(e => e.RequisitionId);

            entity.ToTable("pmsRequisitionCD");

            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("approvedBy");
            entity.Property(e => e.ApprovedBy2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("approvedBy2");
            entity.Property(e => e.ApprovedComp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("approvedComp");
            entity.Property(e => e.ApprovedComp2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("approvedComp2");
            entity.Property(e => e.ApprovedOn)
                .HasColumnType("datetime")
                .HasColumnName("approvedOn");
            entity.Property(e => e.ApprovedOn2)
                .HasColumnType("datetime")
                .HasColumnName("approvedOn2");
            entity.Property(e => e.Comments)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Createby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createby");
            entity.Property(e => e.Createcomp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("createcomp");
            entity.Property(e => e.Createon)
                .HasColumnType("datetime")
                .HasColumnName("createon");
            entity.Property(e => e.DeptId).HasColumnName("deptId");
            entity.Property(e => e.DocDt).HasColumnType("datetime");
            entity.Property(e => e.DocId)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DocPrefix)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DocSuffix)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Mainid).HasColumnName("mainid");
            entity.Property(e => e.ModifyComp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("modifyComp");
            entity.Property(e => e.Modifyby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modifyby");
            entity.Property(e => e.Modifyon)
                .HasColumnType("datetime")
                .HasColumnName("modifyon");
            entity.Property(e => e.ReqTypeId).HasColumnName("reqTypeId");
            entity.Property(e => e.SinRetId).HasColumnName("sinRetId");
            entity.Property(e => e.StatusId).HasColumnName("statusId");

            entity.HasOne(d => d.Dept).WithMany(p => p.PmsRequisitionCds)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_pmsRequisitionCD_setDepartment");

            entity.HasOne(d => d.Store).WithMany(p => p.PmsRequisitionCds)
                .HasForeignKey(d => d.StoreId)
                .HasConstraintName("FK_pmsRequisitionCD_setSetup");
        });

        modelBuilder.Entity<PmsRequisitionDetCd>(entity =>
        {
            entity.HasKey(e => e.RequisitionDetId);

            entity.ToTable("pmsRequisitionDetCD");

            entity.Property(e => e.AlternateItemId).HasColumnName("alternateItemId");
            entity.Property(e => e.AvailableQty).HasColumnName("availableQty");
            entity.Property(e => e.Comments)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MachineId).HasColumnName("machineId");
            entity.Property(e => e.MirDetId).HasColumnName("mirDetId");
            entity.Property(e => e.QtyToProcure).HasColumnName("qtyToProcure");
            entity.Property(e => e.RqdforId).HasColumnName("rqdforId");
            entity.Property(e => e.SinRetDetId).HasColumnName("sinRetDetId");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Uomid).HasColumnName("UOMId");

            entity.HasOne(d => d.Item).WithMany(p => p.PmsRequisitionDetCds)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_pmsRequisitionDetCD_setItemCD");

            entity.HasOne(d => d.Requisition).WithMany(p => p.PmsRequisitionDetCds)
                .HasForeignKey(d => d.RequisitionId)
                .HasConstraintName("FK_pmsRequisitionDetCD_pmsRequisitionCD");
        });

        modelBuilder.Entity<PmsRequisitionDetGsp>(entity =>
        {
            entity.HasKey(e => e.RequisitionDetId);

            entity.ToTable("pmsRequisitionDetGSP");

            entity.Property(e => e.RequisitionDetId).HasColumnName("requisitionDetId");
            entity.Property(e => e.AlternateItemId).HasColumnName("alternateItemId");
            entity.Property(e => e.AvailableQty).HasColumnName("availableQty");
            entity.Property(e => e.Comments)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("comments");
            entity.Property(e => e.ItemId).HasColumnName("itemId");
            entity.Property(e => e.MachineId).HasColumnName("machineId");
            entity.Property(e => e.MirDetId).HasColumnName("mirDetId");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.QtyToProcure).HasColumnName("qtyToProcure");
            entity.Property(e => e.RequisitionId).HasColumnName("requisitionId");
            entity.Property(e => e.RqdforId).HasColumnName("rqdforId");
            entity.Property(e => e.SinRetDetId).HasColumnName("sinRetDetId");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UomId).HasColumnName("uomId");

            entity.HasOne(d => d.Item).WithMany(p => p.PmsRequisitionDetGsps)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_pmsRequisitionDetGSP_setItemCD");

            entity.HasOne(d => d.Requisition).WithMany(p => p.PmsRequisitionDetGsps)
                .HasForeignKey(d => d.RequisitionId)
                .HasConstraintName("FK_pmsRequisition_pmsRequisitionDetGSP");
        });

        modelBuilder.Entity<PmsRequisitionHistory>(entity =>
        {
            entity.HasKey(e => e.AutoGenId);

            entity.ToTable("pmsRequisitionHistory");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusChangedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusChangedComp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StatusChangedDate).HasColumnType("datetime");

            entity.HasOne(d => d.RequisitionDet).WithMany(p => p.PmsRequisitionHistories)
                .HasForeignKey(d => d.RequisitionDetId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Requisition).WithMany(p => p.PmsRequisitionHistories)
                .HasForeignKey(d => d.RequisitionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SetBuyer>(entity =>
        {
            entity.HasKey(e => e.BuyerId);

            entity.ToTable("setBuyer");

            entity.HasIndex(e => e.BuyerId, "IX_setBuyer");

            entity.Property(e => e.BuyerId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BuyerDetId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BuyerGrpId)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BuyerMainId)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BuyerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BuyerPriorId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BuyerSubId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Comments)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("comments");
            entity.Property(e => e.Createby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createby");
            entity.Property(e => e.Createon)
                .HasColumnType("datetime")
                .HasColumnName("createon");
            entity.Property(e => e.CreditDays).HasColumnName("creditDays");
            entity.Property(e => e.EfsendDt)
                .HasColumnType("datetime")
                .HasColumnName("EFSEndDt");
            entity.Property(e => e.Efsno)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EFSNo");
            entity.Property(e => e.EfsstartDt)
                .HasColumnType("datetime")
                .HasColumnName("EFSStartDt");
            entity.Property(e => e.Glid).HasColumnName("GLId");
            entity.Property(e => e.GlidcreateBy)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GLIDCreateBy");
            entity.Property(e => e.GlidcreateComp)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GLIDCreateComp");
            entity.Property(e => e.GlidcreateOn)
                .HasColumnType("datetime")
                .HasColumnName("GLIDCreateOn");
            entity.Property(e => e.IsEfs).HasColumnName("IsEFS");
            entity.Property(e => e.Modifyby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modifyby");
            entity.Property(e => e.Modifyon)
                .HasColumnType("datetime")
                .HasColumnName("modifyon");
            entity.Property(e => e.OpFcamountCr).HasColumnName("OpFCAmountCR");
            entity.Property(e => e.OpFcamountDr).HasColumnName("OpFCAmountDR");
            entity.Property(e => e.OpFcrate).HasColumnName("OpFCRate");
        });

        modelBuilder.Entity<SetDepartment>(entity =>
        {
            entity.HasKey(e => e.DeptId);

            entity.ToTable("setDepartment");

            entity.Property(e => e.CreateComp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("createComp");
            entity.Property(e => e.Createby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createby");
            entity.Property(e => e.Createon)
                .HasColumnType("datetime")
                .HasColumnName("createon");
            entity.Property(e => e.DeleteTag).HasColumnName("deleteTag");
            entity.Property(e => e.DeptAbbr)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ModifyComp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("modifyComp");
            entity.Property(e => e.Modifyby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modifyby");
            entity.Property(e => e.Modifyon)
                .HasColumnType("datetime")
                .HasColumnName("modifyon");

            entity.HasOne(d => d.Det).WithMany(p => p.SetDepartmentDets)
                .HasForeignKey(d => d.DetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_setDepartment_setSetup2");

            entity.HasOne(d => d.Sub).WithMany(p => p.SetDepartmentSubs)
                .HasForeignKey(d => d.SubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_setDepartment_setSetup1");
        });

        modelBuilder.Entity<SetItemCd>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK_setChemical");

            entity.ToTable("setItemCD");

            entity.HasIndex(e => e.MainId, "<IND_setItemCD, sysname,>");

            entity.HasIndex(e => e.MainId, "<INV_setItemCD, sysname,>");

            entity.HasIndex(e => e.ItemCode, "IX_setItemCD_ItemCode").IsUnique();

            entity.Property(e => e.AddPcntEfs).HasColumnName("AddPcntEFS");
            entity.Property(e => e.CreateComp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("createComp");
            entity.Property(e => e.Createby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("createby");
            entity.Property(e => e.Createon)
                .HasColumnType("datetime")
                .HasColumnName("createon");
            entity.Property(e => e.DeleteTag).HasColumnName("deleteTag");
            entity.Property(e => e.Glid).HasColumnName("GLId");
            entity.Property(e => e.GlidcreateBy)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GLIDCreateBy");
            entity.Property(e => e.GlidcreateComp)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GLIDCreateComp");
            entity.Property(e => e.GlidcreateOn)
                .HasColumnType("datetime")
                .HasColumnName("GLIDCreateOn");
            entity.Property(e => e.GrpCode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("grpCode");
            entity.Property(e => e.Hscode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HSCode");
            entity.Property(e => e.HssubCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HSSubCode");
            entity.Property(e => e.IsInclude).HasComment("1 for Include in Consumption and 2 for ");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(19)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ItemName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Lvl3Code)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Lvl4Code)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ModifyComp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("modifyComp");
            entity.Property(e => e.Modifyby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modifyby");
            entity.Property(e => e.Modifyon)
                .HasColumnType("datetime")
                .HasColumnName("modifyon");
            entity.Property(e => e.OldCode).HasMaxLength(100);
            entity.Property(e => e.SupplierId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Uomid).HasColumnName("UOMId");

            entity.HasOne(d => d.Det).WithMany(p => p.SetItemCdDets)
                .HasForeignKey(d => d.DetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_setItemCD_setSetup2");

            entity.HasOne(d => d.Main).WithMany(p => p.SetItemCdMains)
                .HasForeignKey(d => d.MainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_setItemCD_setSetup");

            entity.HasOne(d => d.Sub).WithMany(p => p.SetItemCdSubs)
                .HasForeignKey(d => d.SubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_setItemCD_setSetup1");

            entity.HasOne(d => d.Uom).WithMany(p => p.SetItemCdUoms)
                .HasForeignKey(d => d.Uomid)
                .HasConstraintName("FK_setItemCD_setSetup4");
        });

        modelBuilder.Entity<SetSetup>(entity =>
        {
            entity.ToTable("setSetup");

            entity.Property(e => e.SetsetupId).HasColumnName("setsetupId");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CreateComp)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateOn).HasColumnType("datetime");
            entity.Property(e => e.ForRajby).HasColumnName("forRajby");
            entity.Property(e => e.Glid).HasColumnName("GLID");
            entity.Property(e => e.ModifyBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifyComp)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Modifyon).HasColumnType("datetime");
            entity.Property(e => e.OtherLanguage)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.SetSetupComm2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("setSetupComm2");
            entity.Property(e => e.SetSetupComm3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("setSetupComm3");
            entity.Property(e => e.SetSetupComm4)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("setSetupComm4");
            entity.Property(e => e.SetSetupComm5)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("setSetupComm5");
            entity.Property(e => e.SetsetupComm)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("setsetupComm");
            entity.Property(e => e.SetsetupName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("setsetupName");
            entity.Property(e => e.SetsetupSegid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("setsetupSegid");
            entity.Property(e => e.Setsetupcode)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("setsetupcode");
        });

        modelBuilder.Entity<SmsUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("SMS_User");

            entity.Property(e => e.CreateComp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateOn).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifyBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifyComp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifyOn).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UserGroup)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasMany(d => d.Buyers).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "SetUserAccessBuyer",
                    r => r.HasOne<SetBuyer>().WithMany()
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_setUserAccessBuyer_setBuyer"),
                    l => l.HasOne<SmsUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_setUserAccessBuyer_SMS_User"),
                    j =>
                    {
                        j.HasKey("UserId", "BuyerId");
                        j.ToTable("setUserAccessBuyer");
                        j.HasIndex(new[] { "BuyerId" }, "IX_setUserAccessBuyer");
                        j.IndexerProperty<string>("BuyerId")
                            .HasMaxLength(14)
                            .IsUnicode(false)
                            .IsFixedLength();
                    });
        });

        modelBuilder.Entity<VsetDepartment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vsetDepartment");

            entity.Property(e => e.DeleteTag).HasColumnName("deleteTag");
            entity.Property(e => e.DeptAbbr)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DeptDet)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DeptGlid).HasColumnName("DeptGLID");
            entity.Property(e => e.DeptGrp)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DeptId).HasColumnName("deptId");
            entity.Property(e => e.DeptMain)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DeptSub)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Machine)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
