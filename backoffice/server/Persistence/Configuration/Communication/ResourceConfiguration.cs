using ECCSA.BackOffice.Server.Model.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.Communication
{
    public class ResourceEntityTypeConfiguration :
        IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> config)
        {
            config.ToTable("resources", "communication");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .IsRequired()
                .ValueGeneratedOnAdd();


            config.Property(o => o.Title)
                .HasColumnName("title")
                .HasColumnType("varchar")
                .IsRequired()
                .IsUnicode(true)
                .HasMaxLength(100);

            config.Property(o => o.TitleLocal)
                .HasColumnName("title_local")
                .HasColumnType("varchar")
                .IsRequired(false)
                .IsUnicode(true)
                .HasMaxLength(100);

            config.Property(o => o.Type)
               .HasColumnName("type")
               .IsRequired();

            config.Property(o => o.TypeId)
                .HasColumnName("type_id")
                .IsRequired();

            config.Property(o => o.Author)
                .HasColumnName("author")
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();

            config.Property(o => o.AuthorLocal)
                .HasColumnName("author_local")
                .IsRequired(false)
                .HasMaxLength(100)
                .IsUnicode();


            config.Property(o => o.Description)
                .HasColumnName("description")
                .HasColumnType("varchar")
                .HasMaxLength(800)
                .IsUnicode()
                .IsRequired();
            config.Property(o => o.DescriptionLocal)
                .HasColumnName("description_local")
                .HasColumnType("varchar")
                .HasMaxLength(800)
                .IsUnicode()
                .IsRequired(false);

            config.Property(o => o.PublicationDate)
                    .HasColumnName("publication_date")
                    .HasColumnType("date")
                    .IsRequired();

            config.Property(o => o.Url)
                    .HasColumnName("url")
                    .HasColumnType("varchar")
                    .HasMaxLength(100)
                    .IsRequired();


            config.Property(o => o.Extension)
                    .HasColumnName("extension")
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

            config.Property(o => o.DigitalCost)
                   .HasColumnName("digital_cost")
                   .IsRequired(false);

            config.Property(o => o.PrintUri)
                  .HasColumnName("print_uri")
                  .HasColumnType("varchar")
                  .HasMaxLength(100)
                  .IsRequired();

            config.Property(o => o.PrintCost)
                   .HasColumnName("print_cost")
                   .IsRequired(false);


            config.Property(o => o.OwnerModule)
                   .HasColumnName("owner_module")
                   .IsRequired();


            config.Property(o => o.Profile)
                .HasColumnName("profile")
                .IsUnicode()
                .HasDefaultValue("");

            config.Property(o => o.ProfileLocal)
                .HasColumnName("profile_local")
                .IsUnicode()
                .HasDefaultValue("");

            config.Property(o => o.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired();

            config.Property(o => o.CreateStamp)
                .HasColumnName("create_stamp")
                .IsRequired();

            config.Property(o => o.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired();

            config.Property(o => o.UpdateStamp)
                .HasColumnName("update_stamp")
                .IsRequired();
        }
    }
}