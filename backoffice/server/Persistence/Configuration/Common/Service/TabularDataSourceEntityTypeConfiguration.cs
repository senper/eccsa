using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class TabularDataSourceEntityTypeConfiguration :
        IEntityTypeConfiguration<TabularDataSource>
    {
        public void Configure(EntityTypeBuilder<TabularDataSource> config)
        {
            config.ToTable("tabular_data_sources", "services");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired();

            config.Property(o => o.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.HasMany<TabularDataSourceDetail>(o => o.Details)
                .WithOne(s => s.TabularDataSource)
                .HasForeignKey(s => s.TabularDataSourceId);
        }
    }
    public class TabularDataSourceDetailEntityTypeConfiguration :
       IEntityTypeConfiguration<TabularDataSourceDetail>
    {
        public void Configure(EntityTypeBuilder<TabularDataSourceDetail> config)
        {
            config.ToTable("tabular_datasource_details", "services");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired();

            config.Property(o => o.TabularDataSourceId)
            .HasColumnName("tabular_data_source_id")
            .IsRequired();

            config.Property(o => o.No)
                    .HasColumnName("no")
                    .HasMaxLength(3)
                    .IsRequired();

            config.Property(o => o.Caption)
                    .HasColumnName("caption")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsRequired();

            config.Property(o => o.DataType)
                    .HasColumnName("data_type")
                    .IsRequired();

            config.Property(o => o.MaxLength)
                    .HasColumnName("max_length")
                    .IsRequired();

            config.Property(o => o.ShowOnReport)
                    .HasColumnName("show_on_report")
                    .IsRequired();

            config.Property(o => o.Status)
                    .HasColumnName("status")
                    .IsRequired();

            config.Property(o => o.LookupId)
                    .HasColumnName("lookup_id")
                    .IsRequired(false);
        }
    }
}