using ApiCallerService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCallerService.Persistance
{
    public class ESWRequestEntityTypeConfiguration :
        IEntityTypeConfiguration<ESWRequest>
    {
        public void Configure(EntityTypeBuilder<ESWRequest> config)
        {
            config.ToTable("esw_requests", "trade");

            config.HasKey(o => o.RequestDate);

            config.Property(o => o.RequestDate)
            .HasColumnName("request_date")
            .IsRequired();

            config.Property(o => o.Success)
            .HasColumnName("success")
            .IsRequired();

            config.Property(o => o.Message)
            .HasColumnName("message");
        }
    }
}
