using ECCSA.BackOffice.Server.Model.PublicSecurity;
using ECCSA.BackOffice.Server.Persistence.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Context
{
    public class EccsaPublicUserContext : IdentityDbContext<PublicUser, PublicRole, Guid, PublicUserClaim,
        PublicUserRole, PublicUserLogin, PublicRoleClam, PublicUserToken>
    {
        public EccsaPublicUserContext() : base() { }

        public EccsaPublicUserContext(DbContextOptions<EccsaPublicUserContext> options) : base(options) {

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PublicRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PublicUserClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PublicUserLoginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PublicUserRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PublicUserEntityTypeConfiguration());

           
        }
    }
}
