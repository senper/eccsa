using ECCSA.BackOffice.Server.Model.ExternalData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
	public class ExternalDataTypeEntityTypeConfiguration :
		IEntityTypeConfiguration<ExternalDataType>
	{
		public void Configure(EntityTypeBuilder<ExternalDataType> config)
		{
			config.ToTable("external_data_types");


			config.HasKey(o =>  o.Id );


			config.Property(o => o.Id)
				.HasColumnName("id")
				.IsRequired();

			config.Property(o => o.SchemaName)
				.HasColumnName("schema_name")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();

			config.Property(o => o.SourceName)
				.HasColumnName("table_name")
				.HasMaxLength(100)
				.IsUnicode()
				.IsRequired();


			config.Property(o => o.Caption)
				.HasColumnName("caption")
				.HasMaxLength(100)
				.IsRequired()
				.IsUnicode();

			config.Property(o => o.MetaData)
				.HasColumnName("meta_data")
				.HasColumnType("jsonb")
				.IsRequired();

			config.Property(o => o.DataPeriods)
				.HasColumnName("periods")
				.HasColumnType("jsonb")
				.IsRequired();

			config.Property(o => o.CreatedBy)
				  .HasColumnName("created_by")
				  .IsRequired();

			config.Property(o => o.CreateStamp)
				  .HasColumnName("create_stamp")
				  .IsRequired();

			config.Property(o => o.UpdatedBy)
				  .HasColumnName("updated_by")
				  .IsRequired();

			config.Property(o => o.UpdateStamp)
				  .HasColumnName("update_stamp")
				  .IsRequired();
		}
	}

}