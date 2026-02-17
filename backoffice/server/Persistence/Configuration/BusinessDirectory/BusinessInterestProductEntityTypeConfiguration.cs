using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory
{
    public class BusinessInterestProductEntityTypeConfiguration :
    IEntityTypeConfiguration<BusinessInterestProduct>
    {
        public void Configure(EntityTypeBuilder<BusinessInterestProduct> config)
        {
            config.ToTable("products", "directory");

            config.HasKey(o => o.Id);

            config.Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

            config.Property(o => o.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .IsUnicode()
            .IsRequired()
            .HasMaxLength(255);

            config.Property(o => o.NameNormalized)
            .HasColumnName("name_normalized")
            .HasColumnType("varchar")
            .IsUnicode()
            .IsRequired()
            .HasMaxLength(255);

            config.HasMany<BusinessInterestSourceDetail>(o => o.BusinessInterestSourceDetails)
                .WithOne(s => s.ProductType)
                .HasForeignKey(s => s.ProductTypeId);

        }
    }
}