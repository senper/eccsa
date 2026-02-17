using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace ECCSA.BackOffice.Server.Persistence.Configuration
{
    public class GeneralSettingEntityTypeConfiguration :
        IEntityTypeConfiguration<GeneralSetting>
    {
        public void Configure(EntityTypeBuilder<GeneralSetting> config)
        {
            config.ToTable("general_settings");


            config.HasKey(o => o.Key);


            config.Property(o => o.Key)
            .HasColumnName("key")
            .IsRequired()
            .HasMaxLength(50);


            config.Property(o => o.Value)
            .HasColumnName("value")
            .IsRequired()
            .HasMaxLength(255);
        }
    }
    public class YesNoEntityTypeConfiguration :
        IEntityTypeConfiguration<YesNo>
    {
        public void Configure(EntityTypeBuilder<YesNo> config)
        {
            config.ToTable("yes_no");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .IsRequired();


            config.Property(o => o.Name)
            .HasColumnName("name")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(50);


            config.Property(o => o.NameLocal)
            .HasColumnName("name_local")

                        .HasMaxLength(100)
            .IsUnicode();

        }
    }

    public class CountryEntityTypeConfiguration :
        IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> config)
        {
            config.ToTable("countries");


            config.HasKey(o => o.Name);


            


            config.Property(o => o.Name)
                .HasColumnName("name")
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);


            config.Property(o => o.Iso3)
                .HasColumnName("iso3")
                .IsRequired(false)
                .HasMaxLength(100)
                .IsUnicode();

            config.Property(o => o.Iso2)
                .HasColumnName("iso2")
                .IsRequired(false)
                .HasMaxLength(100)
                .IsUnicode();




        }
    }
    public class LookupTypeEntityTypeConfiguration :
        IEntityTypeConfiguration<LookupType>
    {
        public void Configure(EntityTypeBuilder<LookupType> config)
        {
            config.ToTable("lookup_types");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired();


            config.Property(o => o.Name)
            .HasColumnName("name")
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(50);


            config.Property(o => o.Owner)
            .HasColumnName("owner")
            .IsRequired();

            config.Property(o => o.IsHierarchical)
               .HasColumnName("is_hierarchical")
               .IsRequired();

            config.Property(o => o.ValueSourceType)
                .HasColumnName("value_source_type")
                .IsRequired();

            config.Property(o => o.SourceType)
               .HasColumnName("source_type")
               .IsRequired();

            config.Property(o => o.Status)
                .HasColumnName("status")
                .IsRequired();

            config.HasMany<LookupValue>(o => o.LookupValues)
                  .WithOne(s => s.LookupType)
                 .HasForeignKey(s => s.LookupTypeId);
        }
    }

    public class LookupValueEntityTypeConfiguration :
        IEntityTypeConfiguration<LookupValue>
    {
        public void Configure(EntityTypeBuilder<LookupValue> config)
        {
            config.ToTable("lookup_values");


            config.HasKey(o => o.Id);


            config.Property(o => o.Id)
            .HasColumnName("id")
            .IsRequired();


            config.Property(o => o.Name)
                .HasColumnName("name")
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);
            config.Property(o => o.LookupTypeId)
               .HasColumnName("lookup_type_id")
               .IsRequired();

            config.Property(o => o.Status)
                .HasColumnName("status")
                .IsRequired();
            config.Property(o => o.ParentId)
                .HasColumnName("parent_id");

            config.Property(o => o.Value)
               .HasColumnName("value")
               .HasMaxLength(50);

            config.Property(o => o.Code)
               .HasColumnName("code")
               .HasDefaultValue(string.Empty)
               .HasMaxLength(50);

        }
    }
}