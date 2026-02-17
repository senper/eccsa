using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Website
{
    public class FooterMenuEntityTypeConfiguration :
            IEntityTypeConfiguration<FooterMenu>
    {
        public void Configure(EntityTypeBuilder<FooterMenu> config)
        {
            config.ToTable("footer_menus", "website");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.UsefulLinks)
                    .HasColumnName("useful_links")
                    .HasColumnType("jsonb")
                    .IsRequired();

            config.Property(o => o.ImportantLinks)
                    .HasColumnName("important_links")
                    .HasColumnType("jsonb")
                    .IsRequired(); 

            config.Property(o => o.ContactUs)
                    .HasColumnName("contact_us")
                    .HasColumnType("jsonb")
                    .IsRequired();
        }
    }
}
