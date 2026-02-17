using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class LegalDocumentTypeEntityTypeConfiguration:
		IEntityTypeConfiguration<LegalDocumentType>
	{
	  public void Configure(EntityTypeBuilder<LegalDocumentType> config)
	  {
			config.ToTable("legal_document_types", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("int")
			.IsRequired();


			config.Property(o => o.NameEng)
			.HasColumnName("name_eng")
			
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.NameLocal)
			.HasColumnName("name_local")
			
			.IsRequired()
			
			.IsUnicode();


			config.Property(o => o.HasExpiry)
			.HasColumnName("has_expiry")
			
			.IsRequired();


			config.Property(o => o.AllowMultiple)
			.HasColumnName("allow_multiple")
			
			.IsRequired();




		}
	}
}