using System;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ECCSA.BackOffice.Server.API.Seeding
{
    public static partial class InitializeDatabase
    {

        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<EccsaContext>();
            //if (!context.Database.EnsureCreated()) return;
            //InitializeTrade(context);
            //InitializeMembership(context);
            //TODO: Add Other Initialization
            //context.SaveChangesAsync();
        }

      


       
        private static Guid Sequence(int i) =>
             Guid.Parse("00000000-0000-0000-0000-000000000000".Substring(0, 
                 36 - i.ToString("X").Length) + i.ToString("X"));
        
    }
}
