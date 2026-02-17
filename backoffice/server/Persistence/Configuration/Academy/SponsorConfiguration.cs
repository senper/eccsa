using ECCSA.BackOffice.Server.Model.Academy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class SponsorEntityTypeConfiguration:
        IEntityTypeConfiguration<Sponsor>
    {
      public void Configure(EntityTypeBuilder<Sponsor> config)
      {
            config.ToTable("sponsors", "training") ;
            

            config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
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

			config.Property(o => o.Email)
			.HasColumnName("email");

			config.Property(o => o.Mobile)
			.HasColumnName("mobile");


			config.Property(o => o.Status)
			.HasColumnName("status")
			.HasColumnType("int")
			.IsRequired();


			config.HasMany<TrainingSponsor>(o => o.TrainingSponsors)
				.WithOne(s => s.Sponsor)
				.HasForeignKey(s => s.SponsorId);




           

      }
    }
}