using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Configuration;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Configuration.Membership;
using ECCSA.BackOffice.Server.Persistence.Configuration.Communication;
using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Configuration.Arbitration;
using ECCSA.BackOffice.Server.Model.Common.Servicing;

namespace ECCSA.BackOffice.Server.Persistence.Context
{
	public class EccsaSqlContext : DbContext// IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClam, UserToken>
	{
		public EccsaSqlContext() : base() { }
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{

		//	//optionsBuilder.UseNpgsql(@"localhost;Port=5432;Username=postgres;Password=sa;Database=eccsadb;");
		//}
		public DbSet<Campaign> Campaigns { get; set; }
		public DbSet<CampaignPartnerAgency> CampaignPartnerAgencies { get; set; }
		public DbSet<MediaAgency> MediaAgencies { get; set; }
		public DbSet<MediaCoverage> MediaCoverages { get; set; }
		public DbSet<Resource> Resources { get; set; }
		public DbSet<FlagType> FlagTypes { get; set; }
		public DbSet<Flag> Flags { get; set; }
		public DbSet<ServiceRequest> ServiceRequests { get; set; }
		public DbSet<ServiceType> ServiceTypes { get; set; }
		public DbSet<YesNo> YesNo { get; set; }
		public DbSet<Address> Addresses { get; set; }
		public DbSet<Association> Associations { get; set; }

		public DbSet<AssociationSetting> AssociationSettings { get; set; }
		public DbSet<BoardMember> BoardMembers { get; set; }
		public DbSet<BusinessProfile> BusinessProfiles { get; set; }
		public DbSet<Contact> Contacts { get; set; }

		public DbSet<Applicant> Applicants { get; set; }

		public DbSet<ApplicantDocument> ApplicantDocuments { get; set; }

		public DbSet<MembershipRequirement> MembershipRequirements { get; set; }
		public DbSet<Individual> Individuals { get; set; }
		public DbSet<LegalDocumentType> LegalDocumentTypes { get; set; }
		public DbSet<LegalDocument> LegalDocuments { get; set; }
		public DbSet<MembershipEnrollment> MembershipEnrollments { get; set; }
		public DbSet<PositionType> PositionTypes { get; set; }
		public DbSet<ProfileAttribure> ProfileAttribures { get; set; }
		public DbSet<RenewalHistory> RenewalHistory { get; set; }
		public DbSet<RepresentativeContact> RepresentativeContacts { get; set; }
		public DbSet<Representative> Representatives { get; set; }
		public DbSet<Stakeholder> Stakeholders { get; set; }

		public DbSet<Staff> Employees { get; set; }

		public DbSet<CommodityPrice> CommodityPrices { get; set; }
		public DbSet<CommodityType> CommodityTypes { get; set; }
		public DbSet<ExchangeRate> ExchangeRates { get; set; }
		public DbSet<PriceUnitRate> PriceUnitRates { get; set; }


		public DbSet<Assessment> Assessments { get; set; }
		public DbSet<Attendance> Attendances { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<NeedAssessmentDetail> NeedAssessmentDetails { get; set; }
		public DbSet<NeedAssessment> NeedAssessments { get; set; }
		public DbSet<Registration> Registrations { get; set; }
		public DbSet<ResourceDelivery> ResourceDeliveries { get; set; }
		public DbSet<LearningResource> LearningResources { get; set; }
		public DbSet<SettingVariable> SettingVariables { get; set; }
		public DbSet<Sponsor> Sponsors { get; set; }
		public DbSet<TopicSetting> TopicSettings { get; set; }
		public DbSet<Topic> Topics { get; set; }
		public DbSet<TrainingSponsor> TrainingSponsors { get; set; }
		public DbSet<Training> Trainings { get; set; }

		public DbSet<Location> Locations { get; set; }




		public DbSet<ArbitrationDocument> ArbitrationDocuments {get;set;}
		public DbSet<ArbitrationFollowup> ArbitrationFollowups { get; set; }
		public DbSet<ArbitrationFollowupResult> ArbitrationFollowupResults  {get;set;}
		public DbSet<ArbitrationRelatedParty> ArbitrationRelatedParties  {get;set;}
		public DbSet<ArbitrationService> ArbitrationServices {get;set;}
		public DbSet<ArbitrationServiceType> ArbitrationServiceTypes { get; set; }

		public DbSet<ArbitrationTaskType> ArbitrationTasks { get; set; }



		public DbSet<SurveySetting> SurveySettings { get; set; }
		public DbSet<Survey> Surveys { get; set; }
		public DbSet<SurveyAudience> SurveyAudiences { get; set; }
		public DbSet<SurveyChoice> SurveyChoices { get; set; }
		public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
		public DbSet<SurveyRespondent> SurveyRespondents { get; set; }
		public DbSet<SurveyResponse> SurveyResponses { get; set; }

		public DbSet<SelectedChoice> SelectedChoices { get; set; }
		public EccsaSqlContext(DbContextOptions<EccsaSqlContext> options) : base(options) { }

		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.ApplyConfiguration(new LocationEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new CampaignEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new CampaignPartnerAgencyEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new MediaAgencyEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new MediaCoverageEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ResourceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new FlagTypeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new FlagEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ServiceRequestEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ServiceTypeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new YesNoEntityTypeConfiguration());


			modelBuilder.ApplyConfiguration(new ApplicantEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ApplicantDocumentEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new MembershipRequirementEntityTypeConfiguration());


			modelBuilder.ApplyConfiguration(new AssociationSettingEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new AddressEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new AssociationEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new BoardMemberEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new BusinessProfileEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ContactEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new IndividualEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new LegalDocumentTypeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new LegalDocumentEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new MembershipEnrollmentEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new PositionTypeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ProfileAttribureEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new RenewalHistoryEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new RepresentativeContactEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new RepresentativeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new StakeholderEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new StaffEntityTypeConfiguration());

			modelBuilder.ApplyConfiguration(new CommodityPriceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new CommodityTypeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ExchangeRateEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new PriceUnitRateEntityTypeConfiguration());


			modelBuilder.ApplyConfiguration(new AssessmentEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new AttendanceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new NeedAssessmentDetailEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new NeedAssessmentEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new RegistrationEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ResourceDeliveryEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new LearningResourceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SettingVariableEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SponsorEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new TopicSettingEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new TopicEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new TrainingSponsorEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new TrainingEntityTypeConfiguration());


			modelBuilder.ApplyConfiguration(new ArbitrationDocumentEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ArbitrationFollowupEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ArbitrationFollowupResultEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ArbitrationRelatedPartyEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ArbitrationServiceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ArbitrationServiceTypeEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new ArbitrationTaskTypeEntityTypeConfiguration());



			modelBuilder.ApplyConfiguration(new SelectedChoiceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SurveyAudienceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SurveyChoiceEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SurveyEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SurveyQuestionEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SurveyRespondentEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SurveyResponseEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SurveySettingEntityTypeConfiguration());


		}
	}
}