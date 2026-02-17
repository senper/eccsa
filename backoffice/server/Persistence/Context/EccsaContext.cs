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
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Persistence.Configuration.Payment;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using ECCSA.BackOffice.Server.Persistence.Configuration.Common.Events;
using EventEntityTypeConfiguration = ECCSA.BackOffice.Server.Persistence.Configuration.Common.Events.EventEntityTypeConfiguration;
using EventDetailEntityTypeConfiguration = ECCSA.BackOffice.Server.Persistence.Configuration.Common.Events.EventDetailEntityTypeConfiguration;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Model.Advocacy;
using ECCSA.BackOffice.Server.Persistence.Configuration.Advocacy;
using ECCSA.BackOffice.Server.Model.Investment;
using ECCSA.BackOffice.Server.Persistence.Configuration.Investment;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Configuration.Common.Notification;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.ExternalData;
using ECCSA.BackOffice.Server.Persistence.Configuration.Common.Report;
using ECCSA.BackOffice.Server.Persistence.Configuration.BusinessDirectory;
using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using ECCSA.BackOffice.Server.Model.Common.Reports;
using ECCSA.BackOffice.Server.Persistence.Configuration.Trade;
using ECCSA.BackOffice.Server.Persistence.Configuration.Website;
using System;

namespace ECCSA.BackOffice.Server.Persistence.Context
{
    public class EccsaContext : DbContext// IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClam, UserToken>
    {
        public EccsaContext() : base() { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //	//optionsBuilder.UseNpgsql(@"localhost;Port=5432;Username=postgres;Password=sa;Database=eccsadb;");
        //}


        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Settlment> Settlments { get; set; }
        public DbSet<Model.Payment.Currency> Currencies { get; set; }
        public DbSet<DiscountCategory> DiscountCategories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignPartnerAgency> CampaignPartnerAgencies { get; set; }
        public DbSet<MediaAgency> MediaAgencies { get; set; }
        public DbSet<MediaCoverage> MediaCoverages { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<FlagType> FlagTypes { get; set; }
        public DbSet<Flag> Flags { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<ServiceTypeReport> ServiceTypeReports { get; set; }
        public DbSet<TabularDataSource> TabularDataSources { get; set; }
        public DbSet<TabularDataSourceDetail> TabularDataSourceDetails { get; set; }
        public DbSet<ServiceOwner> ServiceOwners { get; set; }

        public DbSet<ServiceRequirement> ServiceRequirements { get; set; }


        public DbSet<ServiceRequirementResponse> ServiceRequirementResponses { get; set; }

        public DbSet<ServiceFollowup> ServiceFollowups { get; set; }
        public DbSet<RequestAttachment> RequestAttachments { get; set; }

        public DbSet<YesNo> YesNo { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Association> Associations { get; set; }

        public DbSet<AssociationSetting> AssociationSettings { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }
        public DbSet<AssociationProfile> AssociationProfiles { get; set; }
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
        public DbSet<IndividualBusiness> IndividualBusinesses { get; set; }
        public DbSet<IndividualManufacturing> IndividualManufacturings { get; set; }
        public DbSet<IndividualService> IndividualServices { get; set; }
        public DbSet<IndividualTrade> IndividualTrades { get; set; }
       
        public DbSet<Staff> Employees { get; set; }
        public DbSet<IndividualStaff> IndividualEmployees { get; set; }

        public DbSet<CommodityPrice> CommodityPrices { get; set; }
        public DbSet<CommodityType> CommodityTypes { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<PriceUnitRate> PriceUnitRates { get; set; }
        public DbSet<Model.Trade.Currency> ExchangeCurrencies { get; set; }

        public DbSet<MarketPlace> MarketPlaces { get; set; }

        public DbSet<ImportExport> ImportExports { get; set; }

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

        public DbSet<ArbitrationDocument> ArbitrationDocuments { get; set; }
        public DbSet<ArbitrationFollowup> ArbitrationFollowups { get; set; }
        public DbSet<ArbitrationFollowupResult> ArbitrationFollowupResults { get; set; }
        public DbSet<ArbitrationRelatedParty> ArbitrationRelatedParties { get; set; }
        public DbSet<ArbitrationService> ArbitrationServices { get; set; }
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
        public DbSet<SurveyVote> SurveyVotes { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Indicator> Indicators { get; set; }

        public DbSet<QuarterMonthMap> QuarterMonths { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<UnplannedActivity> UnplannedActivities { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Model.Common.Planning.Configuration> PlanConfigurations { get; set; }

        public DbSet<FinancialPlan> FinancialPlans { get; set; }
        public DbSet<FinancialItem> FinancialItems { get; set; }

        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDetail> EventDetails { get; set; }

        public DbSet<Advertizement> Advertizements { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<CommonKeyword> CommonKeywords { get; set; }
        public DbSet<SearchResult> Contents { get; set; }

        public DbSet<News> News { get; set; }
        public DbSet<Law> Laws { get; set; }
        public DbSet<OtherMedia> OtherMedia { get; set; }
        public DbSet<PhotoAlbum> PhotoAlbums { get; set; }
        public DbSet<VideoGallery> VideoGalleries { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<QuickLink> QuickLinks { get; set; }

        public DbSet<QuickLinkHeader> QuickLinkHeaders { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<FooterMenu> FooterMenus { get; set; }

        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<PPDMeeting> PPDMeetings { get; set; }
        public DbSet<Recommendation> PPDRecommendations { get; set; }
        public DbSet<PPDAttachment> PPDAttachments { get; set; }
        public DbSet<MeetingPoint> PPDMeetingPoints { get; set; }

        public DbSet<Research> Researches { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectFollowup> ProjectFollowups { get; set; }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Participant> MeetingParticipants { get; set; }
        public DbSet<MeetingAttendance> MeetingAttendances { get; set; }
        public DbSet<Nominee> MeetingNominees { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<IssueAttachment> IssueAttachments { get; set; }
        public DbSet<IssueLocation> IssueLocations { get; set; }

        public DbSet<Business> Businesses { get; set; }

        public DbSet<RecipientSource> Recipients { get; set; }
        public DbSet<ImportRecipientSectorMap> ImportRecipientSectorMap { get; set; }
        public DbSet<TempRecipientSource> TempRecipients { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<SMSMessage> SMSMessages { get; set; }

        public DbSet<EmailMessage> EmailMessages { get; set; }

        public DbSet<LegalProfile> LegalProfiles { get; set; }
        public DbSet<ExportAgreement> ExportAgreements { get; set; }
        public DbSet<AgreementCommodity> AgreementCommodities { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<BusinessInformation> BusinessInformations { get; set; }
        public DbSet<ExternalDataType> ExternalDataTypes { get; set; }
        public DbSet<LookupType> LookupTypes { get; set; }
        public DbSet<LookupValue> LookupValues { get; set; }
        public DbSet<OperatorHistory> OperatorHistory { get; set; }
        public DbSet<Model.Common.Reports.Report> Reports { get; set; }
        public DbSet<Model.Common.Reports.ReportField> ReportFields { get; set; }
        public DbSet<NewsAttachment> NewsAttachment { get; internal set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<TempCompany> TempCompanies { get; set; }

        public DbSet<BusinessSectorCompany> BusinessSectorCompanies { get; set; }

        public DbSet<BusinessSector> BusinessSectors { get; set; }
        public DbSet<BusinessInterestSource> BusinessInterestSources { get; set; }
        public DbSet<BusinessInterestSourceDetail> BusinessInterestSourceDetails { get; set; }
        public DbSet<BusinessInterestProduct> BusinessInterestProducts { get; set; }
        public DbSet<BusinessInterestSourceSector> BusinessInterestSourceSectors { get; set; }
        public DbSet<MatchMaking> MatchMaking { get; set; }
        public DbSet<RegionSector> RegionSectors { get; set; }

        public DbSet<CertificateOfOrigin> CertificateOfOrigins { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<ESWRequest> EswRequests { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionDissimination> SubscriptionDissiminations { get; set; }
        public DbSet<NewsletterIssue> Newsletters { get; set; }
        public DbSet<ReportSource> ReportSources { get; set; }

        public DbSet<ReportPrimaryKey> ReportPrimaryKeys { get; set; }
        public DbSet<SourceField> ReportSourceFields { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }


     
        public EccsaContext(DbContextOptions<EccsaContext> options) : base(options) {

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new InvoiceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SettlementEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Configuration.Payment.CurrencyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountCategoryEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new LocationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CampaignEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CampaignPartnerAgencyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MediaAgencyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MediaCoverageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FlagTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FlagEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new ServiceTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceRequirementEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceOwnerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceRequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceFollowupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceResponseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceRequestAttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceTypeReportEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TabularDataSourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TabularDataSourceDetailEntityTypeConfiguration());


            modelBuilder.ApplyConfiguration(new GeneralSettingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new YesNoEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicantEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicantDocumentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MembershipRequirementEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new AssociationSettingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AddressEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AssociationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BoardMemberEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessProfileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AssociationProfileEntityTypeConfiguration());
            
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
            modelBuilder.ApplyConfiguration(new IndividualStaffEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new IndividualBusinessEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IndividualManufacturingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IndividualServicesEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IndividualTradeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RegionSectorEntityTypeConfiguration());


            modelBuilder.ApplyConfiguration(new BusinessSectorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyBusinessSectorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TempCompanyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessInterestSourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessInterestSourceDetailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessInterestSourceSectorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessInterestProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MatchMakingEntityTypeConfiguration());


            modelBuilder.ApplyConfiguration(new CommodityPriceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExchangeRateEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PriceUnitRateEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Configuration.CurrencyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MarketPlaceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ImportExportEntityTypeConfiguration());

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
            modelBuilder.ApplyConfiguration(new AttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SurveyVoteEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new IndicatorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlanEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProgressEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new QuarterMonthEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigurationEntityTypeConfiguration());


            modelBuilder.ApplyConfiguration(new UnplannedActivityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FinancialPlanEntityTypeConfiguration());
          
            
            modelBuilder.ApplyConfiguration(new FinancialItemEntityTypeConfiguration());

           
            



            modelBuilder.ApplyConfiguration(new EventTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EventEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EventDetailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AdvertizementEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new KeywordEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommonKeywordEntityTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new ContentEntityTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new NewsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LawEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OtherMediaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PhotoAlbumEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VideoGalleryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MenuEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new QuickLinkEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new QuickLinkHeaderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MediaTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TranslationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FooterMenuEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PartnerEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new DialogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DialogRecommendationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DialogMeetingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DialogAttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DialogPointEntityTypeConfiguration());


            modelBuilder.ApplyConfiguration(new ResearchEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PublicationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectFollowupEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new MeetingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingArtifactEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingParticipantEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingNomineeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MeetingAttendanceEntityTypeConfiguration());
            
            modelBuilder.ApplyConfiguration(new IssueTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IssueEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IssueAttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new IssueLocationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContactUsEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new BusinessEntityConfiguration());

            modelBuilder.ApplyConfiguration(new EmailMessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmailTemplateEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecipientSourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TempRecipientSourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ImportRecipientSectorMapEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SMSMessageEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new LegalProfileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExportAgreementEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AgreementCommodityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserContactEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new BusinessInformationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExternalDataTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LookupTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LookupValueEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CountryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OperatorHistoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportFieldEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportSourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportSourceFieldEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportPrimaryKeyEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new CertificateOfOriginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommodityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ESWRequestEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new SubscriptionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionDissiminationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NewsletterIssueEntityTypeConfiguration());
            
        }
    }
}