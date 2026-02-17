using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IndividualStaffEntityTypeConfiguration :
		IEntityTypeConfiguration<IndividualStaff>
	{
		public void Configure(EntityTypeBuilder<IndividualStaff> config)
		{
			config.ToTable("individual_employees", "membership");


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


			config.Property(o => o.PositionTypeId)
			.HasColumnName("position_type_id")
			.IsRequired();


			config.Property(o => o.IndividualId)
			.HasColumnName("individual_id")
			.HasColumnType("uuid")
			.IsRequired();




			config.Property(o => o.StartDate)
			.HasColumnName("start_date")
			.HasColumnType("date")
			.IsRequired();


			config.Property(o => o.EndDate)
			.HasColumnName("end_date")
			.HasColumnType("date");


			config.Property(o => o.Status)
			.HasColumnName("status")
			.IsRequired();


			config.Property(o => o.Email)
		.HasColumnName("email")
		.HasMaxLength(200)
		.HasDefaultValue("");


			config.Property(o => o.Mobile)
			.HasColumnName("mobile")
			.HasMaxLength(50)
			.HasDefaultValue("")
			.IsRequired();

			config.Property(o => o.Photo)
			.HasColumnName("photo");

			config.Property(o => o.Extension)
			.HasColumnName("extension")
			.HasMaxLength(10)
			.HasDefaultValue("")
			.IsRequired();

			config.Property(o => o.Title)
			.HasColumnName("title")
			.HasMaxLength(20)
			.HasDefaultValue("")
			.IsRequired();

		}
	}

}