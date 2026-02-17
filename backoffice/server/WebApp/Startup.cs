using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Trade;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using React.AspNet;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Advocacy;
using FluentValidation.AspNetCore;
using DinkToPdf.Contracts;
using DinkToPdf;
using ECCSA.BackOffice.Server.WebApp.ReportServices;
using ECCSA.BackOffice.Server.Persistence.Repositories.Investment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.mande;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.Persistence.Repositories.BusinessDirectory;
using ECCSA.BackOffice.Server.Persistence.Repositories;
using Microsoft.AspNetCore.DataProtection;

namespace WebApp
{
    public class Startup
    {
        internal static byte[] SharedSecret = Encoding.ASCII.GetBytes(
            "abcdefghijklmnopqrstuvwxyz1234567890");
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddDbContext<EccsaSqlContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultSqlConnection")));
            //services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<EccsaContext>(opt =>
            opt.UseNpgsql(connectionString));

            services.AddDbContext<EccsaUserContext>(opt =>
            opt.UseNpgsql(connectionString));

            services.AddDbContext<ExternalDataContext>(opt =>
           opt.UseNpgsql(Configuration.GetConnectionString("ExternalDataConnection")));
            services.AddDbContext<EccsaAdminContext>(opt =>
           opt.UseNpgsql(connectionString));

            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders()
                //.AddDefaultUI()
                .AddEntityFrameworkStores<EccsaUserContext>();

            int loginSessionTimeOut = 15;
            try
            {
                loginSessionTimeOut = Configuration.GetValue<int>("LoginSessionTimeOut");
                loginSessionTimeOut = Math.Max(Math.Min(loginSessionTimeOut, 30), 2);
            }
            catch { }
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(loginSessionTimeOut);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });



            services.AddSingleton(typeof(IConverter),
                                    new SynchronizedConverter(new PdfTools()));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();
            //services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            //services.AddTransient<MailService.WebApi.Services.IMailService, MailService.WebApi.Services.MailService>();
            services.AddTransient<IReportService, ReportService>();
            // Make sure a JS engine is registered, or you will get an error!
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName)
              .AddV8();
            //
            services.AddControllersWithViews().AddJsonOptions(x =>
                   {
                       x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                       x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                       x.JsonSerializerOptions.IgnoreNullValues = true;
                   }
                   );

            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECCSA.Public.WebApi", Version = "v1" });
            });

            RegisterDI(services);
            Authentication(services, loginSessionTimeOut);
            services.AddFluentValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECCSA.Public.WebApi v1"));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.Use(async (context, next) =>
                {
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    await next();
                });
            }

            //app.UseCookiePolicy(
            //new CookiePolicyOptions
            //{
            //    //Secure = env.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always,
            //});

            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();
            //
            app.UseReact(config =>
            {
                // If you want to use server-side rendering of React components,
                // add all the necessary JavaScript files here. This includes
                // your components as well as all of their dependencies.
                // See http://reactjs.net/ for more information. Example:
                //config
                //  .AddScript("~/js/First.jsx")
                //  .AddScript("~/js/Second.jsx");

                // If you use an external build too (for example, Babel, Webpack,
                // Browserify or Gulp), you can improve performance by disabling
                // ReactJS.NET's version of Babel and loading the pre-transpiled
                // scripts. Example:
                //config
                //  .SetLoadBabel(false)
                //  .AddScriptWithoutTransform("~/js/bundle.server.js");
            });
            //

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area}/{controller=Dashboard}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private void RegisterDI(IServiceCollection services)
        {
            services.AddScoped<AssessmentRepository>();
            services.AddScoped<AttendanceRepository>();
            services.AddScoped<CategoryRepository>();
            services.AddScoped<MemberRepository>();
            services.AddScoped<NeedAssessmentRepository>();
            services.AddScoped<NeedAssessmentDetailRepository>();
            services.AddScoped<RegistrationRepository>();
            services.AddScoped<LearningResourceRepository>();
            services.AddScoped<ResourceDeliveryRepository>();
            services.AddScoped<SettingVariableRepository>();
            services.AddScoped<SponsorRepository>();
            services.AddScoped<TopicRepository>();
            services.AddScoped<TopicSettingRepository>();
            services.AddScoped<TrainingRepository>();
            services.AddScoped<TrainingSponsorRepository>();

            services.AddScoped<ArbitrationDocumentRepository>();
            services.AddScoped<ArbitrationFollowupRepository>();
            services.AddScoped<ArbitrationFollowupResultRepository>();
            services.AddScoped<ArbitrationRelatedPartyRepository>();
            services.AddScoped<ArbitrationServiceRepository>();
            services.AddScoped<ArbitrationServiceTypeRepository>();
            services.AddScoped<ArbitrationTaskTypeRepository>();

            services.AddScoped<SurveyChoiceRepository>();
            services.AddScoped<ServiceRequestRepository>();
            services.AddScoped<ServiceTypeRepository>();
            services.AddScoped<SurveyRepository>();
            services.AddScoped<SurveyQuestionRepository>();
            services.AddScoped<SurveyRespondentRepository>();
            services.AddScoped<SurveyResponseRepository>();
            services.AddScoped<SurveySettingRepository>();

            services.AddScoped<CampaignRepository>();
            services.AddScoped<CampaignPartnerAgencyRepository>();
            services.AddScoped<MediaAgencyRepository>();
            services.AddScoped<MediaCoverageRepository>();
            services.AddScoped<ResourceRepository>();

            services.AddScoped<AddressRepository>();
            services.AddScoped<ApplicantRepository>();
            services.AddScoped<AssociationRepository>();
            services.AddScoped<BusinessRepository>();
            services.AddScoped<RegionalProfileRepository>();
            services.AddScoped<AssociationSettingRepository>();
            services.AddScoped<MembershipRequirementRepository>();
            services.AddScoped<BoardMemberRepository>();
            services.AddScoped<BusinessProfileRepository>();
            services.AddScoped<ContactRepository>();
            services.AddScoped<IndividualRepository>();
            services.AddScoped<LegalDocumentRepository>();
            services.AddScoped<LegalDocumentTypeRepository>();
            services.AddScoped<ApplicantDocumentRepository>();

            services.AddScoped<LocationRepository>();
            services.AddScoped<MembershipEnrollmentRepository>();
            services.AddScoped<PositionTypeRepository>();
            services.AddScoped<ProfileAttribureRepository>();
            services.AddScoped<RenewalHistoryRepository>();
            services.AddScoped<RepresentativeRepository>();
            services.AddScoped<RepresentativeContactRepository>();
            services.AddScoped<StaffRepository>();
            services.AddScoped<StakeholderRepository>();

            services.AddScoped<CommodityPriceRepository>();
            services.AddScoped<CommodityTypeRepository>();
            services.AddScoped<ExchangeRateRepository>();
            services.AddScoped<PriceUnitRateRepository>();
            services.AddScoped<TradeRepository>();


            services.AddScoped<CurrencyRepository>();
            services.AddScoped<DiscountCategoryRepository>();
            services.AddScoped<InvoiceRepository>();
            services.AddScoped<SettlmentRepository>();


            services.AddScoped<EventRepository>();
            services.AddScoped<EventTypeRepository>();

            services.AddScoped<AdvocacyRepository>();


            services.AddScoped<IssueRepository>();
            services.AddScoped<MeetingRepository>();

            services.AddScoped<InvestmentRepository>();

            services.AddScoped<NotificationRepository>();

            services.AddScoped<LegalProfileRepository>();
            services.AddScoped<ExportAgreementRepository>();
            services.AddScoped<UserContactRepository>();
            services.AddScoped<ExternalDataRepository>();
            services.AddScoped<UserAdministrationRepository>();
            services.AddScoped<BusinessInformationRepository>();
            services.AddScoped<PublicUserRepository>();
            services.AddScoped<YesNoRepository>();
            services.AddScoped<MandERepository>();
            services.AddScoped<MandEFinancialRepository>();

            services.AddScoped<ReportsRepository>();
            services.AddScoped<NewsRepository>();
            services.AddScoped<LawsRepository>();
            services.AddScoped<WebsiteRepository>();
            services.AddScoped<BmoRepository>();
            services.AddScoped<MediaRepository>();
            services.AddScoped<AdvertizementRepository>();
            services.AddScoped<NewsAttachmentRepository>();
            services.AddScoped<BusinessInterestRepository>();
            services.AddScoped<GalleryRepository>();
            services.AddScoped<BaseRepository>();

            services.AddScoped<GetLookupType<ECCSA.BackOffice.Server.Model.Common.Reports.Report>>();
            services.AddScoped<GetLookupType<SelectListItem>>();
            services.AddSingleton<telegramtest.ITelegramService, telegramtest.TelegramService>();
        }

        private static void Authentication(IServiceCollection services, int loginSessionTimeOut)
        {
            //services.AddIdentity<User, Role>()
            //    .AddDefaultTokenProviders()
            //    //.AddDefaultUI()
            //    .AddEntityFrameworkStores<EccsaUserContext>();
            // configure jwt authentication
            services
                .AddAuthentication(
                //x =>
                //{
                //    x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;// JwtBearerDefaults.AuthenticationScheme;
                //    x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;// JwtBearerDefaults.AuthenticationScheme;
                //    x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; //JwtBearerDefaults.AuthenticationScheme;
                //}
                )
        .AddCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(loginSessionTimeOut);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(SharedSecret),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.HttpContext.Request.Query.ContainsKey("token"))
                            {
                                context.Token = context.HttpContext.Request.Query["token"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });


        }
    }
}
