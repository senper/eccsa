using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class FinancialPlanEntityTypeConfiguration :
        IEntityTypeConfiguration<FinancialPlan>
    {
        public void Configure(EntityTypeBuilder<FinancialPlan> config)
        {
            config.ToTable("financial_plans", "mande");


            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.PlanYear)
                  .HasColumnName("plan_year")
                  .IsRequired();

            config.Property(o => o.FinancialItemId)
                  .HasColumnName("financial_item")
                  .IsRequired();

            config.Property(o => o.Status)
                  .HasColumnName("status")
                  .IsRequired();

            config.Property(o => o.VersionNo)
                .HasColumnName("version_no")
                .IsRequired();

            config.Property(o => o.AnnualPlan)
                  .HasColumnName("annual_plan")
                  .IsRequired();

            config.Property(o => o.AnnualActual)
                  .HasColumnName("annual_actual")
                  .IsRequired();

            config.Property(o => o.Quarter1Plan)
                  .HasColumnName("quarter1_plan")
                  .IsRequired();

            config.Property(o => o.Quarter1Actual)
                  .HasColumnName("quarter1_actual")
                  .IsRequired();

            config.Property(o => o.Quarter1Remark)
                  .HasColumnName("quarter1_remark")
                  .HasMaxLength(255)
                  .IsUnicode()
                  .IsRequired();

            config.Property(o => o.Quarter2Plan)
                  .HasColumnName("quarter2_plan")
                  .IsRequired();

            config.Property(o => o.Quarter2Actual)
                  .HasColumnName("quarter2_actual")
                  .IsRequired();

            config.Property(o => o.Quarter2Remark)
                  .HasColumnName("quarter2_remark")
                  .HasMaxLength(255)
                  .IsUnicode()
                  .IsRequired();

            config.Property(o => o.Quarter3Plan)
                  .HasColumnName("quarter3_plan")
                  .IsRequired();

            config.Property(o => o.Quarter3Actual)
                  .HasColumnName("quarter3_actual")
                  .IsRequired();

            config.Property(o => o.Quarter3Remark)
                  .HasColumnName("quarter3_remark")
                  .HasMaxLength(255)
                  .IsUnicode()
                  .IsRequired();

            config.Property(o => o.Quarter4Plan)
                  .HasColumnName("quarter4_plan")
                  .IsRequired();

            config.Property(o => o.Quarter4Actual)
                  .HasColumnName("quarter4_actual")
                  .IsRequired();

            config.Property(o => o.Quarter4Remark)
                  .HasColumnName("quarter4_remark")
                  .HasMaxLength(255)
                  .IsUnicode()
                  .IsRequired();

        }


    }
   
    public class FinancialItemEntityTypeConfiguration :
       IEntityTypeConfiguration<FinancialItem>
    {
        public void Configure(EntityTypeBuilder<FinancialItem> config)
        {
            config.ToTable("financial_items", "mande");


            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                 .HasColumnName("id")
                 .IsRequired();


            config.Property(o => o.Code)
                  .HasColumnName("code")
                  .HasMaxLength(10)
                  .IsRequired();

            config.Property(o => o.Item)
                  .HasColumnName("item")
                  .HasMaxLength(255)
                  .IsRequired();

            config.Property(o => o.IsLabel)
                  .HasColumnName("is_label")
                  .IsRequired();

            config.Property(o => o.ParentId)
                  .HasColumnName("parent_id")
                  .IsRequired(false);

            config.Property(o => o.Status)
                  .HasColumnName("status")
                  .IsRequired();

            config.HasMany<FinancialItem>(s => s.SubItems)
                .WithOne(p => p.ParentItem)
                .HasForeignKey(f => f.ParentId);

            config.HasMany<FinancialPlan>(s => s.Plans)
               .WithOne(p => p.FinancialItem)
               .HasForeignKey(f => f.FinancialItemId);

        }


    }

    
}