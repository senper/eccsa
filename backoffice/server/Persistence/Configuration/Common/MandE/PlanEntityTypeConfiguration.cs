using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ReportingUnitConfiguration : IEntityTypeConfiguration<ReportingUnit>
    {
        public void Configure(EntityTypeBuilder<ReportingUnit> config)
        {
            config.ToTable("reporting_units", "mande");


            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Code)
                .HasColumnName("code")
                .HasMaxLength(20)
                .IsRequired();

            config.Property(o => o.Name)
               .HasColumnName("name")
               .HasMaxLength(50)
               .IsRequired();

        }
    }
    public class PlanEntityTypeConfiguration :
       IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> config)
        {
            config.ToTable("plans", "mande");


            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();


            config.Property(o => o.Code)
                .HasColumnName("code")
                .HasMaxLength(20)
                .IsRequired();

            config.Property(o => o.InternalCode)
                .HasColumnName("internal_code")
                .HasMaxLength(50)
                .IsRequired();

            config.Property(o => o.PlanYear)
               .HasColumnName("plan_year")
               .IsRequired();

            config.Property(o => o.Unit)
               .HasColumnName("unit")
               .IsRequired();

            config.Property(o => o.PlanAmount)
                .HasColumnName("plan_amount")
                .HasMaxLength(100)
                .IsRequired();

            config.Property(o => o.SupportingUnitId)
                .HasColumnName("supporting_unit_id")
                .HasDefaultValue(0)
                .IsRequired();

            config.Property(o => o.ExpectedResult)
               .HasColumnName("expected_result")
               .HasMaxLength(255)
               .IsRequired();

            config.Property(o => o.ResponsibleUnit)
               .HasColumnName("responsible_unit")
               .IsRequired();

            config.Property(o => o.EstimatedBudget)
               .HasColumnName("estimated_budget")
               .IsRequired();

            config.Property(o => o.FromChamber)
               .HasColumnName("from_chamber")
               .IsRequired();

            config.Property(o => o.FromDonor)
               .HasColumnName("from_donor")
               .IsRequired();

            config.Property(o => o.FinancialItemId)
                .HasColumnName("financial_item_id")
                .IsRequired(false);

            config.Property(o => o.Name)
               .HasColumnName("name")
               .HasMaxLength(50)
               .IsRequired();

            config.Property(o => o.IsLabel)
              .HasColumnName("is_label")
              .IsRequired();


            config.Property(o => o.Status)
             .HasColumnName("status")
             .IsRequired();

            config.Property(o => o.ParentId)
                .HasColumnName("parent_id")
                .IsRequired(false);

            config.Property(o => o.Planable)
                .HasColumnName("planable")
                .HasDefaultValue(false)
                .IsRequired(true);

            config.Property(o => o.Month01)
                 .HasColumnName("m01")
                 .HasColumnType("jsonb")
                 .IsRequired();
            config.Property(o => o.Month02)
                 .HasColumnName("m02")
                 .HasColumnType("jsonb")
                 .IsRequired();
            config.Property(o => o.Month03)
                 .HasColumnName("m03")
                 .HasColumnType("jsonb")
                 .IsRequired();
            config.Property(o => o.Month04)
                 .HasColumnName("m04")
                 .HasColumnType("jsonb")
                 .IsRequired();
            config.Property(o => o.Month05)
                 .HasColumnName("m05")
                 .HasColumnType("jsonb")
                 .IsRequired();

            config.Property(o => o.Month06)
                .HasColumnName("m06")
                .HasColumnType("jsonb")
                .IsRequired();
            config.Property(o => o.Month07)
                 .HasColumnName("m07")
                 .HasColumnType("jsonb")
                 .IsRequired();
            config.Property(o => o.Month08)
                 .HasColumnName("m08")
                 .HasColumnType("jsonb")
                 .IsRequired();
            config.Property(o => o.Month09)
                 .HasColumnName("m09")
                 .HasColumnType("jsonb")
                 .IsRequired();
            config.Property(o => o.Month10)
                  .HasColumnName("m10")
                  .HasColumnType("jsonb")
                  .IsRequired();
            config.Property(o => o.Month11)
                  .HasColumnName("m11")
                  .HasColumnType("jsonb")
                  .IsRequired();
            config.Property(o => o.Month12)
                 .HasColumnName("m12")
                 .HasColumnType("jsonb")
                 .IsRequired();

            config.Property(o => o.Quarter1Remark)
               .HasColumnName("quarter1")
               .HasMaxLength(50)
               .IsRequired();

            config.Property(o => o.Quarter2Remark)
               .HasColumnName("quarter2")
               .HasMaxLength(50)
               .IsRequired();

            config.Property(o => o.Quarter3Remark)
               .HasColumnName("quarter3")
               .HasMaxLength(50)
               .IsRequired();

            config.Property(o => o.Quarter4Remark)
               .HasColumnName("quarter4")
               .HasMaxLength(50)
               .IsRequired();

            config.Property(o => o.AnnualProgress)
               .HasColumnName("annual_progress")
               .HasMaxLength(50)
               .IsRequired();

            config.Property(o => o.Remark)
               .HasColumnName("remark")
               .HasMaxLength(2000)
               .IsRequired();

            config.Property(o => o.BasePlanId)
                .HasColumnName("base_plan_id")
                .IsRequired();

            config.Property(o => o.DataMaintenanceHistory)
                .HasColumnName("maintenance_history")
                .HasColumnType("jsonb")
                .IsRequired(false);
            config.Property(o => o.VersionNo)
                .HasColumnName("version_no")
                .IsRequired();

            config.HasMany<Plan>(s => s.SubPlans)
                 .WithOne(p => p.ParentPlan)
                 .HasForeignKey(f => f.ParentId);

            config.HasMany<DetailPlan>(s => s.DetailPlans)
                .WithOne(p => p.Plan)
                .HasForeignKey(f => f.PlanId);


        }



    }
    
   
   
    public class UnplannedActivityEntityTypeConfiguration :
      IEntityTypeConfiguration<UnplannedActivity>
    {
        public void Configure(EntityTypeBuilder<UnplannedActivity> config)
        {
            config.ToTable("unplanned_activities", "mande");


            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.ReportingUnit)
                .HasColumnName("reporting_unit")
                .IsRequired();

            config.Property(o => o.PlanYear)
               .HasColumnName("plan_year")
               .IsRequired();

            config.Property(o => o.MonthNo)
               .HasColumnName("month_no")
               .IsRequired();


            config.Property(o => o.ActivitySummary)
               .HasColumnName("activity_summary")
               .HasMaxLength(1000)
               .IsRequired();

            config.Property(o => o.NetIncome)
               .HasColumnName("net_income")
               .IsRequired();

            config.Property(o => o.SupportingUnitId)
               .HasColumnName("supporting_unit_id")
               .IsRequired();


            config.Property(o => o.Status)
             .HasColumnName("status")
             .IsRequired();






        }



    }

    public class DetailPlanEntityTypeConfiguration :
        IEntityTypeConfiguration<DetailPlan>
    {
        public void Configure(EntityTypeBuilder<DetailPlan> config)
        {
            config.ToTable("detail_plan", "mande");


            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.PlanId)
                .HasColumnName("plan_id")
                .IsRequired();

            config.Property(o => o.MonthNo)
                .HasColumnName("month_no")
                .IsRequired();

            config.Property(o => o.Amount)
                .HasColumnName("amount")
                .IsRequired(false);

            config.Property(o => o.Actual)
                .HasColumnName("actual")
                .IsRequired(false);


            config.Property(o => o.Progress)
                .HasColumnName("progress")
                .IsRequired(false);


            config.Property(o => o.BasePlanId)
                .HasColumnName("base_plan_id")
                .IsRequired();

            config.HasMany<DetailProgress>(s => s.DetailProgresses)
                .WithOne(p => p.DetailPlan)
                .HasForeignKey(f => f.DetailPlanId);

        }



    }

    public class DetailProgressEntityTypeConfiguration :
        IEntityTypeConfiguration<DetailProgress>
    {
        public void Configure(EntityTypeBuilder<DetailProgress> config)
        {
            config.ToTable("detail_progress", "mande");


            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.DetailPlanId)
                .HasColumnName("detail_plan_id")
                .IsRequired();

            config.Property(o => o.MonthNo)
                .HasColumnName("month_no")
                .IsRequired();



            config.Property(o => o.Actual)
                .HasColumnName("actual")
                .IsRequired(false);


            config.Property(o => o.Progress)
                .HasColumnName("progress")
                .IsRequired(false);


            config.Property(o => o.Output)
                .HasColumnName("output")
                .IsRequired();

            config.Property(o => o.Remark)
                .HasColumnName("remark")
                .IsRequired();
        }



    }
}