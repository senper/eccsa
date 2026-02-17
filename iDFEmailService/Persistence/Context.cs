using iDFEmailService.Model;
using iDFEmailService.Model.Notification;
using Microsoft.EntityFrameworkCore;
using System;

namespace iDFEmailService.Persistence
{
    public class Context : DbContext
    {
        private string ConnectionString { get; set; }
        public Context(string connectionString) : base()
        {
            ConnectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }
        public DbSet<EmailMessage> EmailMessages { get; set; }
        public DbSet<SMSMessage> SMSMessages { get; set; }
        public DbSet<EmailDeliveryInfo> DeliveryInfos { get; set; }
        public DbSet<MailSettings> MailSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmailDelieveryInfoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmailMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MailSettingsInfoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SMSMessageEntityTypeConfiguration());
        }
    }
}
