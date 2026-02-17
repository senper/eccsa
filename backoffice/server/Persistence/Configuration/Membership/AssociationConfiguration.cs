using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Configuration.Membership
{
    public class AssociationEntityTypeConfiguration :
        IEntityTypeConfiguration<Association>
    {
        public void Configure(EntityTypeBuilder<Association> config)
        {
            config.ToTable("associations", "membership");


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


            config.Property(o => o.ShortName)
             .HasMaxLength(35)
            .HasColumnName("short_name")
             .HasDefaultValue(string.Empty)
            .IsRequired();

            config.Property(o => o.ShortName_Local)
             .HasMaxLength(35)
            .HasColumnName("short_name_local")
             .HasDefaultValue(string.Empty)
            .IsRequired();

            config.Property(o => o.MemberSince)
            .HasColumnName("member_since")
            .HasColumnType("date")
            .IsRequired();


            config.Property(o => o.AssociationType)
            .HasColumnName("association_type")
            .IsRequired();


            config.Property(o => o.AssociationLevel)
            .HasColumnName("association_level")
            .IsRequired();

            config.Property(o => o.ParentId)
                .HasColumnName("parent_id")
                .IsRequired()
                .HasDefaultValue(Guid.Empty);

            config.Property(o => o.LocationId)
                .HasColumnName("location_id")
                .IsRequired()
                .HasDefaultValue(Guid.Empty);

            config.Property(o => o.Profile)
                .HasColumnName("profile")
                .IsRequired()
                .HasDefaultValue("");

            config.Property(o => o.Profile_Local)
                .HasColumnName("profile_local")
                .IsRequired()
                .HasDefaultValue("");

            config.Property(o => o.Logo)
                .HasColumnName("logo");



            config.Property(o => o.Extension)
                .HasColumnName("extension")
                .HasMaxLength(20)
                .IsRequired()
                .HasDefaultValue("");


            config.Property(o => o.RegistrationFee)
                .HasColumnName("registration_fee")
                .IsRequired()
                .HasDefaultValue(0);

            config.Property(o => o.RenewalFee)
                .HasDefaultValue(0)
                .HasColumnName("renewal_fee");

            config.Property(o => o.EnableWebAccess)
                .IsRequired()
                .HasColumnName("enable_web_access");

            config.Property(o => o.ProposedRegistrationFee)
                .HasColumnName("proposed_registration_fee");

            config.Property(o => o.ProposedRenewalFee)
                .HasColumnName("proposed_renewal_fee");

            config.Property(o => o.ProposedBy)
                .HasColumnName("proposed_by");

            config.Property(o => o.DateProposed)
                .HasColumnName("date_proposed");


            config.HasMany<Staff>(o => o.Employees)
                .WithOne(s => s.Association)
                .HasForeignKey(s => s.AssociationId);

            config.HasMany<BoardMember>(o => o.BoardMembers)
                .WithOne(s => s.Association)
                .HasForeignKey(s => s.AssociationId);


            config.HasMany<MembershipEnrollment>(o => o.MembershipEnrollments)
                .WithOne(s => s.Association)
                .HasForeignKey(s => s.ParentAssociationId);

            config.HasMany<MembershipRequirement>(o => o.MembershipRequirements)
                .WithOne(s => s.Association)
                .HasForeignKey(s => s.AssociationId);


            config.HasMany<AssociationSetting>(o => o.Settings)
                .WithOne(s => s.Association)
                .HasForeignKey(s => s.AssociationId);

            config.HasMany<Applicant>(o => o.Applicants)
               .WithOne(s => s.Association)
               .HasForeignKey(s => s.AssociationId);


        }
    }
}