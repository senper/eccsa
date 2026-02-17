using ECCSA.BackOffice.Server.Model.Investment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Investment
{
    public class BusinessEntityConfiguration : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> config)
        {
            config.ToTable("businesses", "investment");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
                    .HasColumnName("id")
                    .IsRequired()
                    .ValueGeneratedOnAdd();


            config.Property(o => o.Name)
                .HasColumnName("name")
                .HasColumnType("varchar")
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(200);





            config.Property(o => o.Sector)
                .HasColumnName("sector")
                .HasColumnType("varchar")
                .IsRequired(false)
                .IsUnicode()
                .HasMaxLength(100);

            config.Property(o => o.Profile)
                   .HasColumnName("profile")
                   .HasMaxLength(800)
                   .IsUnicode();

            config.Property(o => o.Address)
                        .HasColumnName("address")
                        .HasColumnType("varchar")
                        .IsUnicode()
                        .HasMaxLength(200);

            config.Property(o => o.Country)
                   .HasColumnName("country")
                   .HasMaxLength(50)
                   .IsUnicode();



            config.Property(o => o.Email)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(50);




            config.Property(o => o.Telephone)
                        .HasColumnName("telephone")
                        .HasColumnType("varchar")
                        .IsRequired()
                        .HasMaxLength(50);

            config.Property(o => o.TINo)
                   .HasColumnName("tino")
                   .HasColumnType("varchar")
                   .IsRequired()
                   .HasMaxLength(20);

            config.Property(o => o.ContactPerson)
                   .HasColumnName("contact_person")
                   .HasColumnType("varchar")
                   .IsRequired()
                   .HasMaxLength(50);


            config.Property(o => o.Products)
                    .HasColumnName("products")
                    .HasColumnType("varchar")
                    .IsRequired()
                    .HasMaxLength(100);


            config.Property(o => o.YearOfEstablishment)
                       .HasColumnName("establishment_year")
                       .IsRequired();


            config.Property(o => o.WorkingCapital)
                    .HasColumnName("working_capital")
                    .IsRequired();


            config.Property(o => o.ConfirmationLink)
                   .HasColumnName("confirmation_link")
                   .HasColumnType("varchar")
                   .IsRequired()
                   .HasMaxLength(200);


            config.Property(o => o.EmailConfirmed)
                  .HasColumnName("email_confirmed")
                  .HasDefaultValue(false)
                  .IsRequired();

            config.Property(o => o.LinkExpiryDate)
                 .HasColumnName("link_expiry_date")
                 .HasColumnType("date")
                 .IsRequired();


            config.Property(o => o.PhoneConfirmed)
                    .HasColumnName("phone_confirmed")
                    .HasDefaultValue(false)
                    .IsRequired();

            config.Property(o => o.SMSCode)
                   .HasColumnName("sms_code")
                   .HasMaxLength(10)
                   .HasDefaultValue(string.Empty)
                   .IsRequired();




            config.Property(o => o.CreatedBy)
            .HasColumnName("created_by")
            .IsRequired();

            config.Property(o => o.CreateStamp)
                .HasColumnName("create_stamp")
                .HasDefaultValue(System.DateTime.UtcNow)
                .IsRequired();

            config.Property(o => o.UpdatedBy)
                .HasColumnName("updated_by")
                .IsRequired();

            config.Property(o => o.UpdateStamp)
                .HasColumnName("update_stamp")
                .HasDefaultValue(System.DateTime.UtcNow)
                .IsRequired();





        }
    }
}
