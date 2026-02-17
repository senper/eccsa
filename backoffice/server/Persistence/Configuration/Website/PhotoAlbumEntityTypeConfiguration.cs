using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class PhotoAlbumEntityTypeConfiguration :
            IEntityTypeConfiguration<PhotoAlbum>
    {
        public void Configure(EntityTypeBuilder<PhotoAlbum> config)
        {
            config.ToTable("photo_album", "website");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode();

            config.Property(o => o.Description)
                .HasColumnName("description")
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode();
        }
    }
}