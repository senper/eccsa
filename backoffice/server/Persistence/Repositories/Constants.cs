using System;

namespace ECCSA.BackOffice.Server.Persistence.Repositories
{
    public static class Constants
    {
        public static readonly int Finance_SupportId = 2;

        public static readonly Guid SupportingUnitLookupTypeId = Guid.Parse("424063b8-7cef-4ff0-8a13-c88334a100a5");
        public static readonly Guid ECCSA = Guid.Parse("00000000-0000-0000-0000-0000001e8480");
        public static readonly Guid MembershipBoardChairmanId = Guid.Parse("00000000-0000-0000-0000-0000000003e9");
        public static readonly Guid MembershipCEOId = Guid.Parse("00000000-0000-0000-0000-0000000007d1");
        public static readonly Guid EthiopiaId = Guid.Parse("00000000-0000-0000-0000-0000000f4240");
     //   public static readonly Guid ResourceTypeId = Guid.Parse("00000000-0000-0000-0000-000000000030");
        
        public static readonly Guid LegalStatusLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid SectorLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public static readonly Guid RegionLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        public static readonly Guid SponsorLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000004");
        public static readonly Guid TrainingTopicLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000005");

        public static readonly Guid StakeholderTypeEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000006");
        public static readonly Guid MembershipWorkflowEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000007");
        public static readonly Guid AssociationTypeEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000008");
        public static readonly Guid LevelEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000009");
        public static readonly Guid ContactTypeEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid LegalStatusEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000000b");
        public static readonly Guid MeetingTypeEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000000c");
        public static readonly Guid DocumentReviewStatusEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000000d");
        public static readonly Guid ECCSAModuleFlagLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000000e");
        public static readonly Guid RoleLevelEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000000f");
        public static readonly Guid EventStatusLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000010");
        public static readonly Guid IssueStatusLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000011");
        public static readonly Guid IssueReportingMeansLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000012");
        public static readonly Guid FrequencyEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000013");
        public static readonly Guid MandEPhaseEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000014");
        public static readonly Guid ServiceInputTypeEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000015");
        public static readonly Guid QuestionTypeLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000016");
        public static readonly Guid RateBasisEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000017");
        public static readonly Guid WorkflowStateEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000018");
        public static readonly Guid PaymentMethodEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000019");
        public static readonly Guid StatusEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000001a");
        public static readonly Guid ResearchStageEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000001b");
        public static readonly Guid TaskStatusEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000001c");
        public static readonly Guid YesNoEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000001d");
        public static readonly Guid MediaOutletFlagLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000001e");
        public static readonly Guid SexEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000001f");
        public static readonly Guid IndicatorTypeEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000020");
        public static readonly Guid DeliveryStatusLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000021");
        public static readonly Guid CcGroupLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000022");
        public static readonly Guid MessageStatusLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000023");
        public static readonly Guid ResourceTypeLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000024");
        public static readonly Guid DistributionFormatLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000025");
        public static readonly Guid LegalDocumentTypesLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000026");
        public static readonly Guid ReportTypeEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000027");
        public static readonly Guid FilterFieldTypeLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000028");
        public static readonly Guid ProfileAttributeTypeEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-000000000029");
        public static readonly Guid RequirementLevelEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000002a");
        public static readonly Guid PlanningStatusEnumLookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000002b");
        




    }
}