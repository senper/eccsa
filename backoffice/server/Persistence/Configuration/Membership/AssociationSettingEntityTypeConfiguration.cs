using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class AssociationSettingEntityTypeConfiguration :
		IEntityTypeConfiguration<AssociationSetting>
	{
		public void Configure(EntityTypeBuilder<AssociationSetting> config)
		{
			config.ToTable("association_settings", "membership");


			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.AssociationId)
			.HasColumnName("association_id")
			.IsRequired();


			config.Property(o => o.BankName)
			.HasColumnName("bank_name")
			.HasMaxLength(100)
			.IsUnicode();
			//.IsRequired();

			config.Property(o => o.AccountNumber)
			.HasColumnName("account_number")
			.HasMaxLength(100)
			.IsUnicode();
			//.IsRequired();

			config.Property(o => o.SubSystem)
			.HasColumnName("sub_system")
			.IsRequired();

			config.Property(o => o.ProposedBankName)
			.HasMaxLength(100)
			.HasColumnName("proposed_bank_name");

			config.Property(o => o.ProposedAccountNumber)
			.HasMaxLength(100)
			.HasColumnName("proposed_account_number");

			config.Property(o => o.ProposedBy)
			.HasColumnName("proposed_by");

			config.Property(o => o.DateProposed)
			.HasColumnName("date_proposed");

			config.Property(o => o.UpdatedDate)
			.HasColumnName("updated_date");

			config.Property(o => o.UpdatedBy)
			.HasColumnName("updated_by");
		}
	}
}