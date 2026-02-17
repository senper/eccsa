using ECCSA.BackOffice.Server.Model.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Communication
{
	public class CampaignEntityTypeConfiguration:
		IEntityTypeConfiguration<Campaign>
	{
	  public void Configure(EntityTypeBuilder<Campaign> config)
	  {
			config.ToTable("campaigns", "communication") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Title)
			.HasColumnName("title")
			.HasColumnType("varchar")
						.HasMaxLength(100);





			config.Property(o => o.Description)
			.HasColumnName("description")
			
						
			.IsUnicode();


			config.Property(o => o.StartDate)
			.HasColumnName("start_date")
			.HasColumnType("date");
			

			config.Property(o => o.EndDate)
			.HasColumnName("end_date")
			.HasColumnType("date");
			

			config.Property(o => o.Purpose)
			.HasColumnName("purpose")
			
						
			.IsUnicode();

            config.Property(o => o.Extension)
                .HasColumnName("extension")
                .HasDefaultValue("")
                .HasMaxLength(10)
                .IsRequired();
                


            config.Property(o => o.Status)
			.HasColumnName("status")
			.HasColumnType("int");
			

			config.Property(o => o.CreatedBy)
				.HasColumnName("created_by")
				.IsRequired();

			config.Property(o=> o.CreateStamp)
				.HasColumnName("create_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();

			config.Property(o => o.UpdatedBy)
				.HasColumnName("updated_by")
				.IsRequired();

			config.Property(o => o.UpdateStamp)
				.HasColumnName("update_stamp")
				.HasDefaultValue(System.DateTime.UtcNow)
				.IsRequired();
			

			config.HasMany<CampaignPartnerAgency>(o => o.CampaignPartnerAgencies)
				.WithOne(s => s.Campaign)
				.HasForeignKey(s => s.CampaignId);




		   

	  }
	}
}