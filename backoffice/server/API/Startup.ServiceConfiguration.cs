using ECCSA.BackOffice.Server.API.GraphQL.Queries;
using ECCSA.BackOffice.Server.API.GraphQL.Mutations;
using Microsoft.Extensions.DependencyInjection;
using ECCSA.BackOffice.Server.Persistence.Repositories.Trade;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;

namespace ECCSA.BackOffice.Server.API
{
    public partial class Startup
    {
        private void RegisterGraphQL(IServiceCollection services)
        {
            services
                .AddGraphQLServer()
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>()
                .AddAuthorization()
                ;
        }

        private void RegisterDI(IServiceCollection services)
        {
            services.AddScoped<Query>();

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
            services.AddScoped<ApplicantDocumentRepository>();
            services.AddScoped<LegalDocumentTypeRepository>();
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

        }
    }
}
