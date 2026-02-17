using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class RepresentativeEntityTypeConfiguration:
		IEntityTypeConfiguration<Representative>
	{
	  public void Configure(EntityTypeBuilder<Representative> config)
	  {
			config.ToTable("representatives", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.ValueGeneratedOnAdd()
			.IsRequired();


			config.Property(o => o.MemberId)
			.HasColumnName("member_id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.Title)
			.HasColumnName("title")
			
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.NameEng)
			.HasColumnName("name_eng")
			
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.NameLocal)
			.HasColumnName("name_local")
			
			.IsRequired()
			
			.IsUnicode();


			config.Property(o => o.OrderNo)
			.HasColumnName("order_no")
			.HasMaxLength(20)
			.HasDefaultValue(string.Empty)
			.IsRequired();


			config.Property(o => o.Status)
			.HasColumnName("status")
			.HasDefaultValue(Model.Common.StatusEnum.Active)
			.IsRequired();


			config.Property(o => o.Position)
			.HasColumnName("position")
			.HasMaxLength(50)
			.IsRequired();


			config.Property(o => o.StartDate)
			.HasColumnName("start_date")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.EndDate)
			.HasColumnName("end_date")
			.HasColumnType("date");
			

			
			

			
			

			config.HasMany<RepresentativeContact>(o => o.RepresentativeContacts)
				.WithOne(s => s.Representative)
				.HasForeignKey(s => s.RepresentativeId);




		   

	  }
	}
}