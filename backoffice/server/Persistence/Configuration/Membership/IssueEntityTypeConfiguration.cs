using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class IssueEntityTypeConfiguration :
	   IEntityTypeConfiguration<Issue>
	{
		public void Configure(EntityTypeBuilder<Issue> config)
		{
			config.ToTable("issues", "membership");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
			.HasColumnName("id")
			.IsRequired();

			config.Property(o => o.TypeId)
			.HasColumnName("type_id")
			.IsRequired();

			config.Property(o => o.CompanyName)
			.HasColumnName("company_name")
			.IsRequired()
			.HasMaxLength(255)
			.HasDefaultValue("")
			.IsUnicode();

			config.Property(o => o.DetailsOfOccurrence)
			.HasColumnName("details_of_occurrence")
			.IsRequired()
			.HasMaxLength(1000)
			.HasDefaultValue("")
			.IsUnicode();

			config.Property(o => o.IssueDate)
			.HasColumnName("issue_date")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.MeasureCode)
			.HasColumnName("measure_code")
			.IsRequired()
			.HasMaxLength(255)
			.HasDefaultValue("")
			.IsUnicode();

			config.Property(o => o.PlaceOfOccurrence)
			.HasColumnName("place_of_occurrence")
			.IsRequired()
			.HasMaxLength(255)
			.HasDefaultValue("")
			.IsUnicode();

			config.Property(o => o.DateAssigned)
			.HasColumnName("date_assigned")
			.HasColumnType("date");

			config.Property(o => o.Remark)
			.HasColumnName("remark")
			.IsRequired()
			.HasMaxLength(1000)
			.HasDefaultValue("")
			.IsUnicode();

			config.Property(o => o.ActionTaken)
			.HasColumnName("action_taken")
			.HasMaxLength(255)
			.IsUnicode();

			config.Property(o => o.Status)
				  .HasColumnName("status")
				  .IsRequired();

			config.Property(o => o.ActionOwner)
			.HasColumnName("action_owner");

			config.Property(o => o.DateActionTaken)
			.HasColumnName("action_taken_date")
			.HasColumnType("date");

			config.Property(o => o.DateOfOccurrence)
			.HasColumnName("date_of_occurrence")
			.HasColumnType("date")
			.IsRequired();

			config.Property(o => o.CountryRegion)
			.HasColumnName("country_region")
			.IsRequired();

			config.Property(o => o.TypeOfLocationId)
			.HasColumnName("type_of_location_id");

			config.Property(o => o.OtherLocation)
			.HasColumnName("other_location")
			.HasMaxLength(255)
			.IsUnicode(); ;

			config.Property(o => o.LocationId)
			.HasColumnName("location_id");

			config.Property(o => o.Description)
			.HasColumnName("description")
			.HasMaxLength(600)
			.HasDefaultValue("")
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.AffectedProducts)
			.HasColumnName("affected_products")
			.HasMaxLength(600)
			.HasDefaultValue("")
			.IsRequired()
			.IsUnicode();

			config.Property(o => o.ProductTariffCode)
			.HasColumnName("product_tariff_code")
			.IsRequired()
			.HasMaxLength(255)
			.HasDefaultValue("")
			.IsUnicode();

			config.Property(o => o.DescriptionOfProducts)
			.HasColumnName("description_of_products")
			.IsRequired()
			.HasDefaultValue("")
			.IsUnicode();

			config.Property(o => o.ProductValueCurrency)
			.HasColumnName("product_value_currency")
			.IsRequired()
			.HasMaxLength(255)
			.HasDefaultValue("")
			.IsUnicode();

			config.Property(o => o.ProductValueAmount)
			.HasColumnName("product_value_amount")
			.IsRequired().HasPrecision(4);

			config.Property(o => o.PreferredHandlerId)
			.HasColumnName("preferred_handler_id");

			config.Property(o => o.UserId)
			.HasColumnName("user_id")
			.IsRequired();

			config.HasMany<IssueAttachment>(o => o.Attachments)
				.WithOne(s => s.Issue)
				.HasForeignKey(s => s.IssueId);

		}
	}
}