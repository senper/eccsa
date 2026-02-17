using Microsoft.EntityFrameworkCore;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Context
{
    public class ExternalDataContext : DbContext
    {
        public ExternalDataContext() : base() { }

        public ExternalDataContext(DbContextOptions<ExternalDataContext> options) : base(options) {

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
        }
    }
}
