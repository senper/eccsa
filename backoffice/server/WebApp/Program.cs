using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
                //.WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                //.WriteTo.File(path: "/var/www/backoffice/Logs/log-debug.json", rollingInterval: RollingInterval.Day)
                //.WriteTo.File(path: "/var/www/backoffice/Logs/log-info.json", 
                //restrictedToMinimumLevel: LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(path: "/var/www/backoffice/Logs/log-error.json",
                //restrictedToMinimumLevel: LogEventLevel.Error, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(path: "/var/www/backoffice/Logs/log-warning.json",
                //restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
                .CreateLogger();
         
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
