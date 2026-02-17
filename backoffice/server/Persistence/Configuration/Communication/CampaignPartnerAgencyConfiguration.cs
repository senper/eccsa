using ECCSA.BackOffice.Server.Model.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Communication
{
	public class CampaignPartnerAgencyEntityTypeConfiguration:
		IEntityTypeConfiguration<CampaignPartnerAgency>
	{
	  public void Configure(EntityTypeBuilder<CampaignPartnerAgency> config)
	  {
			config.ToTable("campaign_partner_agencies", "communication") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.CampaignId)
			.HasColumnName("campaign_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.MediaAgencyId)
			.HasColumnName("media_agency_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Status)
			.HasColumnName("status")
			.HasColumnType("int");
			

			config.Property(o => o.CommunicatedThrough)
			.HasColumnName("communicated_through")
			.HasColumnType("int")
			.IsRequired();


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
			



		   

	  }
	}
}