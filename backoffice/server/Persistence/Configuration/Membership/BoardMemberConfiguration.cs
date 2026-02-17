using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
	public class BoardMemberEntityTypeConfiguration:
		IEntityTypeConfiguration<BoardMember>
	{
	  public void Configure(EntityTypeBuilder<BoardMember> config)
	  {
			config.ToTable("board_members", "membership") ;
			

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
			.HasColumnType("uuid")
			.IsRequired();


			config.Property(o => o.AssociationId)
			.HasColumnName("association_id")
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




		   

	  }
	}
}