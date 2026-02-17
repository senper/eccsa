using ECCSA.BackOffice.Server.Model.PublicSecurity;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Trade;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECCSA.Public.WebApi
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

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var connectionPublicString = Configuration.GetConnectionString("DefaultPublicConnection");
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
            services.AddDbContext<EccsaPublicUserContext>(opt =>
            opt.UseNpgsql(connectionPublicString));

            services.AddControllers().AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECCSA.Public.WebApi", Version = "v1" });
            });

            RegisterDI(services);
            Authentication(services);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECCSA.Public.WebApi v1"));
            }

            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
            services.AddScoped<SurveyAudienceRepository>();
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
            services.AddScoped<TradeRepository>();
            services.AddScoped<PriceUnitRateRepository>();


            services.AddScoped<CurrencyRepository>();
            services.AddScoped<DiscountCategoryRepository>();
            services.AddScoped<InvoiceRepository>();
            services.AddScoped<SettlmentRepository>();

        }
        private static void Authentication(IServiceCollection services)
        {
            services.AddIdentity<PublicUser, PublicRole>()
                .AddDefaultTokenProviders()
                //.AddDefaultUI()
                .AddEntityFrameworkStores<EccsaPublicUserContext>();
            // configure jwt authentication
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
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
