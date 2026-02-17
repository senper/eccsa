using ECCSA.BackOffice.Server.API.GraphQL.Exceptions;
using ECCSA.BackOffice.Server.API.Seeding;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECCSA.BackOffice.Server.API
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            // ConfigureAuthenticationServices(services);


            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));


            services.AddDbContext<EccsaContext>(opt =>
            opt.UseNpgsql(connectionString));
            services.AddDbContext<EccsaUserContext>(opt =>
            opt.UseNpgsql(connectionString));

            services.AddErrorFilter<AppException>();

            RegisterGraphQL(services);
            RegisterDI(services);
            ConfigureAuthenticationServices(services);

            //services.AddQueryRequestInterceptor((context, builder, ct) =>
            //{

            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase.Initialize(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();
            app.UseWebSockets();
            app.UseRouting();

            app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
            app.UseGraphQLVoyager(
                path:"/graphql-voyager"
                );
        }
    }
}
