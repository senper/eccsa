using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Common.Report
{
    public class ReportFieldEntityTypeConfiguration :
	IEntityTypeConfiguration<Model.Common.Reports.ReportField>
	{
		public void Configure(EntityTypeBuilder<Model.Common.Reports.ReportField> config)
		{
			config.ToTable("report_field", "public");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.ReportId)
				.HasColumnName("report_id")
				.IsRequired();

			config.Property(o => o.FieldName)
				.HasColumnName("field_name")
				.IsRequired();

			config.Property(o => o.Caption)
					.HasColumnName("caption")
					.IsRequired();

			config.Property(o => o.DisplayOrder)
					.HasColumnName("display_order");

			config.Property(o => o.IsViewable)
					.HasColumnName("is_viewable")
					;

			config.Property(o => o.IsGroupField)
					.HasColumnName("is_group_field")
					;

			config.Property(o => o.IsPercent)
					.HasColumnName("is_percent")
				.HasMaxLength(50)
					.IsRequired();

			config.Property(o => o.IsNumeric)
					.HasColumnName("is_numeric");

			config.Property(o => o.IsFilterField)
					.HasColumnName("is_filter_field");

			config.Property(o => o.DropDownSource)
					.HasColumnName("dropdown_source");
            config.Property(o => o.DropDownSourceId)
                .HasColumnName("dropdown_source_id")
                .IsRequired(false);

            config.Property(o => o.FieldTypeId)
					.HasColumnName("field_type_id");

			config.Property(o => o.IsDefault)
					.HasColumnName("is_default");

			config.Property(o => o.FilterFieldName)
					.HasColumnName("filter_field_name")
					.HasMaxLength(100)
					.IsRequired();
		}
	}
}