using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory
{
    public class MatchMakingEntityTypeConfiguration :
		IEntityTypeConfiguration<MatchMaking>
	{
		public void Configure(EntityTypeBuilder<MatchMaking> config)
		{
			config.ToTable("match_making", "directory");

			config.HasKey(o => o.Id);

			config.Property(o => o.Id)
				.HasColumnName("id")
				.HasColumnType("uuid")
				.IsRequired();

			config.Property(o => o.SourceId)
				.HasColumnName("source_id")
				.IsRequired()
				.HasColumnType("uuid");

			config.Property(o => o.UserId)
				.HasColumnName("user_id")
				.IsRequired()
				.HasColumnType("uuid");

			config.Property(o => o.RequestDate)
				.HasColumnName("request_date")
				.IsRequired();

			config.Property(o => o.Remark)
				.HasColumnName("remark")
				.HasColumnType("varchar")
				.HasMaxLength(255);

			config.Property(o => o.Status)
				.HasColumnName("status")
				.IsRequired();
		}
	}
}