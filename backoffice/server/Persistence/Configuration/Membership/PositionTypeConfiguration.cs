using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class PositionTypeEntityTypeConfiguration:
		IEntityTypeConfiguration<PositionType>
	{
	  public void Configure(EntityTypeBuilder<PositionType> config)
	  {
			config.ToTable("position_types", "membership") ;
			

			config.HasKey(o => o.Id);


			config.Property(o => o.Id)
			.HasColumnName("id")
			.HasColumnType("uuid")
			.IsRequired();

			config.Property(o => o.OrderNo)
			.HasColumnName("order_no")
			.IsRequired()
			.HasMaxLength(20);

			config.Property(o => o.NameEng)
			.HasColumnName("name_eng")
			
			.IsRequired()
			.HasMaxLength(200)
			.IsUnicode();


			config.Property(o => o.NameLocal)
			.HasColumnName("name_local")
			
			.IsRequired()
			
			.IsUnicode();

			config.Property(o => o.ForBoard)
				.HasColumnName("for_board")
				.IsRequired();

			config.Property(o => o.ForStaff)
				.HasColumnName("for_staff")
				.IsRequired();

			config.Property(o => o.Status)
			.HasColumnName("status")
			.IsRequired();


			config.HasMany<BoardMember>(o => o.BoardMembers)
				.WithOne(s => s.PositionType)
				.HasForeignKey(s => s.PositionTypeId);


			config.HasMany<Staff>(o => o.Employees)
				.WithOne(s => s.PositionType)
				.HasForeignKey(s => s.PositionTypeId);



		}
	}
}