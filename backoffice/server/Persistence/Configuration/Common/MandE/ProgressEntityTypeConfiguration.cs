using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ProgressEntityTypeConfiguration :
       IEntityTypeConfiguration<Progress>
    {
        public void Configure(EntityTypeBuilder<Progress> config)
        {
            config.ToTable("progresses", "mande");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired();

            config.Property(o => o.PlanId)
                  .HasColumnName("plan_id")
                  .IsRequired();

        

            config.Property(o => o.ReportingDate)
                    .HasColumnName("reporting_date")
                    .HasColumnType("date")
                    .IsRequired();

            config.Property(o => o.ReportNo)
                   .HasColumnName("report_no")
                   .IsRequired();


            config.Property(o => o.Physical)
                   .HasColumnName("physical")
                   .HasPrecision(18,4)
                   .IsRequired();

            config.Property(o => o.Financial)
                   .HasColumnName("financial")
                   .HasPrecision(18,2)
                   .IsRequired();




        }
    }
}