using ECCSA.BackOffice.Server.Model.Website;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class MenuEntityTypeConfiguration :
            IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> config)
        {
            config.ToTable("menus", "website");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
                .UseIdentityColumn()
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.TitleLocal)
                    .HasColumnName("title_local")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.Path)
              .HasColumnName("path")
              .HasMaxLength(255)
              .IsRequired()
              .IsUnicode();

           

            config.Property(o => o.ParentId)
              .HasColumnName("parent_id")
              .IsRequired(false);

            config.Property(o => o.HeaderId)
                .HasColumnName("header_id")
                .IsRequired(false);

            config.Property(o => o.Order)
              .HasColumnName("order")
              .HasDefaultValue(0)
              .IsRequired();

            config.Property(o => o.ShowOnMainMenu)
              .HasColumnName("show_on_main_menu")
              .HasDefaultValue(false)
              .IsRequired();

            config.HasMany<Menu>(o => o.Children)
                .WithOne(s => s.ParentMenu)
                .IsRequired(false)
                .HasForeignKey(s => s.ParentId);

            config.HasMany<MediaType>(o => o.MediaTypes)
               .WithOne(s => s.Menu)
               .IsRequired(false)
               .HasForeignKey(s => s.MenuId);
        }
    }


    public class QuickLinkEntityTypeConfiguration :
            IEntityTypeConfiguration<QuickLink>
    {
        public void Configure(EntityTypeBuilder<QuickLink> config)
        {
            config.ToTable("quicklinks", "website");

            config.HasKey(o => o.Id);
            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.TitleLocal)
                    .HasColumnName("title_local")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.HeaderId)
              .HasColumnName("header_id")
              .IsRequired(true);

            config.Property(o => o.Order)
              .HasColumnName("order")
              .HasDefaultValue(0)
              .IsRequired();

            config.Property(o => o.MenuId)
              .HasColumnName("menu_id")
              .IsRequired(false);

            config.HasMany<MediaType>(o => o.MediaTypes)
                .WithOne(s => s.QuickLink)
                .IsRequired(false)
                .HasForeignKey(s => s.QuickLinkId);

        }
    }


    public class QuickLinkHeaderEntityTypeConfiguration :
           IEntityTypeConfiguration<QuickLinkHeader>
    {
        public void Configure(EntityTypeBuilder<QuickLinkHeader> config)
        {
            config.ToTable("quicklink_headers", "website");

            config.HasKey(o => o.Id);
            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();
            config.Property(o => o.Label)
                    .HasColumnName("label")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.LabelLocal)
                    .HasColumnName("label_local")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.HasMany<QuickLink>(o => o.QuickLinks)
                 .WithOne(s => s.Header)
                 .IsRequired()
                 .HasForeignKey(s => s.HeaderId);


        }
    }
    public class MediaTypeEntityTypeConfiguration :
            IEntityTypeConfiguration<MediaType>
    {
        public void Configure(EntityTypeBuilder<MediaType> config)
        {
            config.ToTable("media_types", "website");

            config.HasKey(o => o.Id);
            config.Property(o => o.Id)
                .HasColumnName("id")
                .IsRequired();

            config.Property(o => o.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.NameLocal)
                    .HasColumnName("name_local")
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

            config.Property(o => o.MenuId)
                    .HasColumnName("menu_id")
                    .IsRequired(false);

            config.Property(o => o.QuickLinkId)
                    .HasColumnName("quicklink_id")
                    .IsRequired(false);

            config.Property(o => o.MediaAuthor)
                .HasColumnName("media_author")
                .IsRequired();
            config.HasMany<OtherMedia>(o => o.Contents)
                .WithOne(s => s.MediaType)
                .IsRequired()
                .HasForeignKey(s => s.MediaTypeId);
        }
    }
}