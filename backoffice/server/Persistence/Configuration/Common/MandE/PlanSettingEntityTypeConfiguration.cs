using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class ConfigurationEntityTypeConfiguration :
        IEntityTypeConfiguration<Model.Common.Planning.Configuration>
    {
        public void Configure(EntityTypeBuilder<Model.Common.Planning.Configuration> config)
        {
            config.ToTable("configuration", "mande");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
                  .HasColumnName("id")
                  .IsRequired();

          

            config.Property(o => o.Green)
                   .HasColumnName("green")
                   .HasPrecision(18, 2)
                   .IsRequired();

            config.Property(o => o.Yello)
                   .HasColumnName("yello")
                   .HasPrecision(18, 2)
                   .IsRequired();

            config.Property(o => o.Blue)
                   .HasColumnName("blue")
                   .HasPrecision(18, 2)
                   .IsRequired();

            config.Property(o => o.BlueGreen)
                   .HasColumnName("blue_green")
                   .HasPrecision(18, 2)
                   .IsRequired();

            config.Property(o => o.Calendar)
                  .HasColumnName("calendar")
                  .HasMaxLength(1)
                  .IsRequired();

            config.Property(o => o.CurrentYear)
                 .HasColumnName("current_year")
                 .IsRequired();

            config.Property(o => o.CurrentMonth)
                .HasColumnName("current_month")
                .IsRequired();

            config.Property(o => o.PlanYear)
                  .HasColumnName("plan_year")
                  .IsRequired();
            config.Property(o => o.ClosedYears)
                 .HasColumnName("closed_years")
                 .HasColumnType("jsonb")
                 .IsRequired();

            config.Property(o => o.VersionNo)
                .HasColumnName("version_no")
                .IsRequired();
            config.Property(o => o.PlanningPhase)
                .HasColumnName("planning_phase")
                .IsRequired();
           
            config.Property(o => o.PlanVersionNo)
                .HasColumnName("plan_version")
                .IsRequired();
        }
    }

    
}