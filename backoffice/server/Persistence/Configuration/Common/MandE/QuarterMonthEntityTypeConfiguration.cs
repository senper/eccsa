using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class QuarterMonthEntityTypeConfiguration :
        IEntityTypeConfiguration<QuarterMonthMap>
    {
        public void Configure(EntityTypeBuilder<QuarterMonthMap> config)
        {
            config.ToTable("quarter_month_map", "mande");


            config.HasKey(o => new { o.Quarter, o.Month });


            config.Property(o => o.Quarter)
                  .HasColumnName("quarter")
                  .IsRequired();
            config.Property(o => o.Month)
                  .HasColumnName("month")
                  .IsRequired();

     
         
        }
    }
}