using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Report
{
    public class ReportEntityTypeConfiguration :
        IEntityTypeConfiguration<Model.Common.Reports.Report>
    {
        public void Configure(EntityTypeBuilder<Model.Common.Reports.Report> config)
        {
            config.ToTable("report", "public");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Title)
                .HasColumnName("title")

                .IsRequired();

            config.Property(o => o.ReportTypeId)
                    .HasColumnName("report_type_id");

            config.Property(o => o.SourceName)
                    .HasColumnName("source_name")
                    .IsRequired();

            config.Property(o => o.SubTitle)
                    .HasColumnName("subtitle")
                ;

            config.Property(o => o.FileName)
                    .HasColumnName("file_name")
                    ;

            config.Property(o => o.GroupName)
                    .HasColumnName("group_name")
                    ;

            config.Property(o => o.SortOrder)
                    .HasColumnName("sort_order")
                .HasMaxLength(50)
                    .IsRequired();

            config.Property(o => o.IsActive)
                .HasColumnName("is_active")
                .IsRequired(false);

            config.Property(o => o.DefaultValueKey)
                    .HasColumnName("default_value_key");

            config.Property(o => o.SpatialComponent)
                    .HasColumnName("spatial_component");

            config.Property(o => o.TrendComponent)
                    .HasColumnName("trend_component");

            config.Property(o => o.SpatialSource)
                    .HasColumnName("spatial_source");

            config.Property(o => o.TrendSource)
                    .HasColumnName("trend_source");

            config.Property(o => o.PKFieldId)
                    .HasColumnName("pk_field_id")
                    .IsRequired();

            config.Property(o => o.ReportOwner)
                    .HasColumnName("report_owner")
                    .IsRequired();

            config.Property(o => o.ServiceOwnerFieldName)
                    .HasColumnName("service_owner_field_name");

            config.HasMany(o => o.ReportFields)
                  .WithOne(s => s.Report)
                 .HasForeignKey(s => s.ReportId);
        }
    }

    public class ReportSourceEntityTypeConfiguration :
        IEntityTypeConfiguration<Model.Common.Reports.ReportSource>
    {
        public void Configure(EntityTypeBuilder<Model.Common.Reports.ReportSource> config)
        {
            config.ToTable("vreport_sources", "public");

            config.HasKey(o => o.SourceName);

            config.Property(o => o.SourceName)
                .HasColumnName("source_name")
                .IsRequired();

            config.Property(o => o.SchemaName)
                .HasColumnName("table_schema")

                .IsRequired();

            config.Property(o => o.TableName)
                    .HasColumnName("table_name");

            

            config.HasMany(o => o.SourceFields)
                  .WithOne(s => s.Source)
                 .HasForeignKey(s => s.SourceName);
        }
    }

    public class ReportPrimaryKeyEntityTypeConfiguration :
        IEntityTypeConfiguration<Model.Common.Reports.ReportPrimaryKey>
    {
        public void Configure(EntityTypeBuilder<Model.Common.Reports.ReportPrimaryKey> config)
        {
            config.ToTable("vreport_primarykeys", "public");

            config.HasKey(o =>
            new { o.SchemaName, o.TableName, o.ColumnName });

            config.Property(o => o.SchemaName)
                .HasColumnName("table_schema")
                .IsRequired();

            config.Property(o => o.ColumnName)
                .HasColumnName("column_name")
                .IsRequired();

            config.Property(o => o.DataType)
                    .HasColumnName("data_type");

            config.Property(o => o.TableName)
                    .HasColumnName("table_name")
                    .IsRequired();

           
        }
    }

    public class ReportSourceFieldEntityTypeConfiguration :
        IEntityTypeConfiguration<Model.Common.Reports.SourceField>
    {
        public void Configure(EntityTypeBuilder<Model.Common.Reports.SourceField> config)
        {
            config.ToTable("vreport_source_fields", "public");

            config.HasKey(o =>
                new { o.SourceName, o.ColumnName });

            config.Property(o => o.SourceName)
                .HasColumnName("source_name")
                .IsRequired();

            config.Property(o => o.ColumnName)
                .HasColumnName("column_name")
                .IsRequired();

            config.Property(o => o.DataType)
                .HasColumnName("data_type");

            config.Property(o => o.SourceName)
                .HasColumnName("source_name")
                .IsRequired();


        }
    }
}