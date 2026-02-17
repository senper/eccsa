using ApiCallerService.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiCallerService.Persistance
{
    public class Context : DbContext
    {
        public string _connectionString { get; set; }
        public Context(string connectionString) : base()
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql(_connectionString);

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
        public DbSet<CertificateOfOrigin> CertificateOfOrigins { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<ESWRequest> ESWRequests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CertificateOfOriginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ESWRequestEntityTypeConfiguration());
        }
    }
}
