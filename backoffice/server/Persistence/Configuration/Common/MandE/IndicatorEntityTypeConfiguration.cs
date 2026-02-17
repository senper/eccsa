using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class IndicatorEntityTypeConfiguration :
        IEntityTypeConfiguration<Indicator>
    {
        public void Configure(EntityTypeBuilder<Indicator> config)
        {
            config.ToTable("indicators", "mande");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired();

            config.Property(o => o.Code)
                  .HasColumnName("code")
                  .HasColumnType("varchar")
                  .IsRequired()
                  .HasMaxLength(20);

            config.Property(o => o.Name)
                .HasColumnName("name")
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            config.Property(o => o.Description)
                    .HasColumnName("description")
                    .HasMaxLength(800)
                    .IsUnicode();


            config.Property(o => o.ParentId)
                  .HasColumnName("parent_id")
                  .IsRequired(false);

            config.Property(o => o.ResponsibleUnit)
                  .HasColumnName("responsible_unit")
                  .IsRequired();

            config.Property(o => o.Frequency)
                 .HasColumnName("frequency")
                 .HasDefaultValue(FrequencyEnum.None)
                 .IsRequired();

            config.Property(o => o.IndicatorType)
                  .HasColumnName("indicator_type")
                  .IsRequired();

           
            config.Property(o => o.Status)
                  .HasColumnName("status")
                  .IsRequired();

            config.HasMany<Indicator>(s => s.SubIndicators)
                  .WithOne(p => p.ParentIndicator)
                  .HasForeignKey(f => f.ParentId);

           
        }
    }
}