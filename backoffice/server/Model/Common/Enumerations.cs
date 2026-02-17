using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ECCSA.BackOffice.Server.Model.Common
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            string displayName;
            displayName = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName();
            if (displayName == null)
            {
                displayName = enumValue.ToString();
            }
            return displayName;
        }
    }
    
    [Flags]
    public enum ECCSAModuleFlag
    {
        Membership = 1,
        Communication = 2,
        [Display(Name = "Trade Promotion")]
        Investment = 4,
        Academy = 8,
        Arbitration = 16,
        Research = 32,
        [Display(Name = "Trade Facilitation")]
        TradePromotion = 64,
        Administration = 128,
        Reporting = 256,
        MandE = 512,
        Support = 1024,
        [Display(Name = "BMO")]
        Bmo = 2048,
    }
    [Flags]
    public enum SendThroughFlag
    {
        Email = 1,
        SMS = 2
    }
    // Flag converted to Enum for M&E
    public enum ECCSAModuleEnum
    {
        [Display(Name = "")]
        None = 0,
        Membership = 1,
        Communication = 2,
        [Display(Name = "Trade Promotion")]
        Investment = 4,
        Academy = 8,
        Arbitration = 16,
        Research = 32,
        [Display(Name = "Trade Facilitation")]
        TradePromotion = 64,
        [Display(Name = "IT")]
        Administration = 128,
        [Display(Name = "M&E")]
        MandE = 512,
        [Display(Name = "Supporting Unit")]
        Support = 1024,
        [Display(Name = "BMO")]
        Bmo = 2048,
    }
    public enum ApplicationTypeEnum
    {
        New = 0,
        Renewal = 2
    }
    public enum RoleLevelEnum
    {
        None = 0,
        View = 1,
        LimitedUser = 2,
        User = 3,
        Admin = 4,
        Event = 5,
        Survey = 6,
        Finance = 9,
       
    }


    public enum EventStatus
    {
        [Display(Name = " ")]
        NotSet = 0,
        Draft = 1,
        Submitted = 2,
        Published = 3,
        Closed = 4
    }

    public enum IssueStatus
    {
        New = 0,
        Assigned = 1,
        Resolved = 2,

        //Rejected = 3,
        //RaisedToExternalOwner = 4,
        //Closed = 9
    }

    public enum IssueReportingMeans
    {
        NotSet = 0,
        Paper = 1,
        Website = 2,
        Telegram = 3,
        SMS = 4,
        Facebook = 5,
        Other = 99
    }
    public enum VATOptionEnum
    {
        Included = 0,
        Excluded = 1,
        NotApplicable = 2
    }

    public enum FrequencyEnum
    {
        None = 0,
        Monthly = 1,
        Quarterly = 2,
        Yearly = 3
    }
    public enum UnitOfMeasureEnum
    {
        None = 0,
        [Display(Name="Birr")]
        Money = 1,
        Count = 2,
        [Display(Name = "%")]
        Percent = 3
    }

    public enum PlanningPhaseEnum
    {
        Unknown = 0,
        PrePlanning = 1,
        Planning = 2,
        Closed = 3
    }

    public enum PlanningStatusEnum
    {
        [Display(Name = "")]
        None = -97,
        Planning = 0,
        Rejected = 1,
        Submitted = 2,
        Approved = 3,
        Completed = 98,
        Closed = 99,
    }

    public enum ProgressStatusEnum
    {
        [Display(Name = "")]
        None = 0,
        Draft = 0,
        Rejected = 1,
        Submitted = 2,
        Approved = 3,
        Completed = 98,
        Closed = 99,
    }
    public enum ServiceInputTypeEnum
    {
        [Display(Name = " ")]
        None = 0,

        [Display(Name = "Upload File")]
        Upload = 1,

        [Display(Name = "Numeric Counting")]
        Integer = 2,

        [Display(Name = "Numeric Decimal")]
        Decimal = 3,

        [Display(Name = "Date")]
        Date = 4,

        [Display(Name = "Text Short (less than 50 characters)")]
        ShortText = 5,

        [Display(Name = "Text Long (more than 200 characters)")]
        LongText = 6,
        //[Display(Name = "Single Select")]
        //SingleSelect = 7,
        [Display(Name = "Legal Document")]
        LegalDocument = 7,

        [Display(Name = "Trade Agreement")]
        TradeAgreement = 8,

        [Display(Name = "Invoice and Payment")]
        Payment = 9,

        [Display(Name = "Lookup")]
        SimpleLookup = 10,

        [Display(Name = "Tabular")]
        Tabular = 11,

        [Display(Name = "Email")]
        Email = 12,

        [Display(Name = "Mobile No.")]
        Mobile = 13,

        [Display(Name = "Phone No.")]
        Phone = 14,

        [Display(Name = "Text Medium (between 50 and 200 characters)")]
        MediumText = 15
    }

    public enum QuestionType
    {
        SingleChoice = 1,
        MultipleChoice = 2,
        Numeric = 3,
        Text = 4
    }

    public enum CertificateTypeEnum
    {
        [Display(Name = " ")]
        None = 0,
        [Display(Name ="Commercial Invoice")]
        CommercialInvoice = 1,
        [Display(Name = "Packing List")]
        PackingList = 2,
        [Display(Name = "Health Certificate")]
        HealthCertificate = 3,
        [Display(Name = "Certificate of Fee Sale")]
        FreeSale = 4,
        [Display(Name = "Support Letter")]
        SupportLetter = 5,
    }

    public enum RateBasisEnum
    {
        [Display(Name = " ")]
        NotSet = 0,

        Adhoc = 3,
        Membership = 4,
        Flat = 5
    }
    public enum WorkflowStateEnum
    {
        [Display(Name = "Not Submitted")]
        NotSet = 0,
        Submitted = 1,

        [Display(Name = "Under Review")]
        UnderReview = 2,

        [Display(Name = "Draft Approve")]
        DraftPrepared = 3,

        [Display(Name = "Draft Rejection")]
        RecommendRejection = 4,

        Accepted = 5,

        Rejected = 6,

        Closed = 100,
    }

    public enum PaymentMethodEnum
    {
        [Display(Name = " ")]
        NotSet = 0,
        
        [Display(Name = "Bank Transfer")]
        Manual = 1,

        [Display(Name = "Tele Birr")]
        MobileTransfer = 2,
    }

    public enum PaymentStatusEnum
    {
        [Display(Name = " ")]
        NotSet = 0,

        Accepted = 1,

        Partial = 2,

        Rejected = 3,

    }

    public enum StatusEnum
    {
        [Display(Name = "In Active")]
        Inactive = 0,
        Active = 1
    }

    public enum DissiminationStatusEnum
    {
        [Display(Name = "In Active")]
        Draft = 0,
        Sending = 1,
        Closed = 2
    }
    public enum PPDRecommendationEnum
    {
        
        Draft = 0,
        Accepted = 1,
        Rejected = 2,
        [Display(Name = "Forwarded for Joint Review")]
        Forwarded = 3

    }
    public enum MinuteStatusEnum
    {
        [Display(Name = " ")]
        NotSet = 0,
        Approve = 1,
        Reject = 2
    }

    public enum MeetingStatusEnum
    {
        [Display(Name = " ")]
        Draft = 0,
        [Display(Name = "Registration")]
        ReadyForRegistration = 1,
        [Display(Name = "Participation")]
        ReadyForParticipation = 2,
        [Display(Name = "Minute Voting")]
        ReadyForMinuteVoting = 3,
        Closed = 4
    }
    public enum LookupSourceTypeEnum
    {
        NotSet = 0,
        Dynamic = 1,
        System = 2,
        Enum = 3
    }
    public enum LookupValueTypeEnum
    {
        [Display(Name = "Id Based")]
        IdBased = 0,
        [Display(Name = "Name Based")]
        NameBased = 1,
        [Display(Name = "User Assigned")]
        UserAssigned = 2,
    
    }
    public enum ProjectStatusEnum
    {
        [Display(Name = "Not Started")]
        NotStarted = 0,

        Started = 1,
        [Display(Name = "Started In Active")]
        StartedInactive = 2,
        [Display(Name = "In Active")]
        Completed = 3
    }
    public enum ResearchStageEnum
    {
        [Display(Name = " ")]
        NotSet = 0,
        [Display(Name = "Stage I")]
        Stage1 = 1,
        [Display(Name = "Stage II")]
        Stage2 = 2,
        [Display(Name = "Stage III")]
        Stage3 = 3
    }

    public enum TaskStatusEnum
    {
        [Display(Name = "In Active")]
        NotSet = 0,
        Planned = 1,
        Started = 2,
        Completed = 3,
        Waiting = 4,
    }



    public enum YesNoEnum
    {
        [Display(Name = "Not Set")]
        NotSet = 0,
        Yes = 1,
        No = 2
    }

    [Flags]
    public enum MediaOutletFlag
    {
        NotSet = 0,
        SMS = 1,
        Telegram = 2,
        Email = 4,
        Website = 8
    }

    public enum SexEnum
    {
        NotSet = 0,
        Female = 1,
        Male = 2
    }

    public enum IndicatorTypeEnum
    {
        [Display(Name = "Root")]
        ROOT = 1,
        [Display(Name = "Header")]
        HEADER = 2,
        [Display(Name = "Aggregate")]
        AGGREGATE = 3,
        [Display(Name = "Leaf")]
        LEAF = 4
    }

    public enum DeliveryStatus
    {
        NotSet = 0,
        Planned = 1,
        Sent = 2,
        Delivered = 3,
        NotDelivered = 4
    }

    public enum CcGroup
    {
        To = 0,
        CC = 1,
        BCC = 2
    }

    public enum MessageStatus
    {
        Draft = 0,
        [Display(Name = "Send")]
        Submitted = 1,
        Processed = 2,
        Canceled = 3,
        Closed = 4,
    }

    //Is it possible to change this to lookup
    public enum ResourceType
    {
        [Display(Name = " ")]
        NotSet = 0,
        [Display(Name = "Book")]
        Book = 1,
        [Display(Name = "Publications")]
        Publications = 2,
        [Display(Name = "Newspapers")]
        Newspaper = 3,
        [Display(Name = "External Links")]
        Links = 4,
        [Display(Name = "Downloads")]
        Downloads = 5,
        [Display(Name = "Laws and Directives")]
        LawsandDirectives = 6,
        [Display(Name = "e-Newsletter")]
        eNewsletter = 7,
        [Display(Name = "Media Digest")]
        MediaDigest = 8,
        [Display(Name = "Other Materials")]
        Other = 99,
    }
    public enum DistributionFormat
    {
        NotSet = 0,
        Print = 1,
        Digital = 2,

    }
    public enum LegalDocumentTypes
    {
        [Display(Name = " ")]
        NotSet = 0,
        [Display(Name = "Trade Registration")]
        TradeRegistration = 1,
        [Display(Name = "Trade License")]
        TradeLicense = 2,
        [Display(Name = "TIN Certificate")]
        TINCertificate = 3,
        [Display(Name = "VAT Certificate")]
        VATCertificate = 4,
        [Display(Name = "Export License")]
        ExportLicense = 5,
        [Display(Name = "Article Of Incorporate")]
        ArticleOfIncorporate = 6,
        [Display(Name = "Article Of Constitution")]
        ArticleOfConstitution = 7,
        AuditReport = 8,
        [Display(Name = "Audit Report")]
        Agreement = 9,
        Other = 100,
    }


    public enum ReportTypeEnum
    {
        Unknown = 0,
        List = 1,
        Pie = 2,
        Spatial = 3,
        TreeMap = 4
    }

    public enum DrillDownComponent
    {
        None = 0,
        Jobseeker = 1,
        Worker = 2
    }

    public enum FilterFieldType
    {
        None = 0,
        DropDownListNumeric = 1,
        DropDownListText = 2,
        TextNumeric = 3,
        TextString = 4,
        TextStringLike = 5,
        DropDownListNumericFromTo = 6,
        TextNumericFromTo = 7
    }

    public enum TabularDataSourceDetailDataType
    {
        Text = 1,
        Number = 2,
        Lookup = 3
    }
    public enum CommercialInvoiceReportFields
    {
        ConsigneeName = 1,
        ConsignorName = 2,
        CountryOfOrigin = 3,
        DateOfShipment = 4,
        PermitNo = 5,
        CertificateOfOriginNo = 6,
        BankDetail = 7,
        Importer = 8,
        Details = 20,
    }
    public enum SupportLetterReportFields
    {
        EmbassyName = 1,
        EmbassyAddress = 2,
        FullName = 3,
        DestinationCountry = 4,
        Signature = 5,
        SignatoryName = 6,
        SignatoryPosition = 7,
        Watermark = 8
    }
    public enum HealthCertificateReportFields
    {
        RefNo = 1,
        Consignor = 2,
        ConsignorAddress = 3,
        Consignee = 4,
        ConsigneeAddress = 5,
        ExportDeclarationNo = 6,
        InvoiceNo = 7,
        PlaceofInspection = 8,
        DateofInspection = 9,
        ProductGoodName = 10,
        NoofPackage = 11,
        GrossWeight = 12,
        NetWeight = 13,
    }
    public enum PackingListReportFields
    {
        ShipperSeller = 1,
        FullAddress = 2,
        Tel = 3,
        Fax = 4,
        Email = 5,
        VATRegNo = 6,
        InvoiceNo = 7,
        ShippedBy = 8,
        ConsigneeBuyer = 9,
        ConsigneeFullAddress = 10,
        ConsigneeTel = 11,
        ConsigneeFax = 12,
        ConsigneeEmail = 13,
        ConsigneeVATRegNo = 14,
        ConsigneePlaceofdeliveryFullAddress = 15,
        InvoiceDate = 16,
        Details = 17,
    }

    public enum FreeSaleReportFields
    {
        Laws = 1,
        CompanyProduct = 2,
        DestinationCountries = 3,
        RequestorTitle = 4,
        RequestorName = 5,
        Day = 6,
        Details = 7,
    }

    public enum LawTypeEnum
    {
        None = 0,
        Proclamation = 1,
        Regulation = 2,
        Directive = 3
    }
    public enum LawCategoryEnum
    {
        None = 0,
        Trade = 1,
        Investment = 2,
        Customs = 3,
        Domestic = 4,
        Land = 5,
        Other = 99
    }
    public enum TextMediaTypeEnum {
        None = 0,
        Announcement = 1,
        PressRelease = 2,
        BusinessMemberOrganizations = 3,
        Blog = 4
    }

    public enum AdvertisementSectionEnum
    {
        None = 0,
        Header = 1,
        //Footer = 2,
        //HomeLeft = 3,
        //HomeRight = 4,
        Platinum = 5,
        Gold = 6,
        Silver = 7,
        Bronze = 8,
        Other = 9
    }

    public static class BaseConverter
    {
        const string Digits =    "0123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";
        const string Digits1 =   "GHIJKLMNPQRSTUVW";
        const string Digits2 =   "XYZ";
        const string HexDigits = "0123456789ABCDEF";
        public static string GetCodeFromGuid(string guidPart)
        {
            //Guid g = new Guid("57F67098-00A9-4F78-A729-4234F5AC512C");
            //int pos = g.ToString().LastIndexOf('-');
            //string part = g.ToString().Substring(pos + 1);
            long result = Convert.ToInt64(guidPart, 16);
            return GetCode(result);
            //Console.WriteLine(result.ToString());
        }

        public static string GetGuidFromCode(string code)
        {
            var charArray = code.ToUpperInvariant().ToArray();
            var oldBase = Digits.Length;
            var newBase = HexDigits.Length;
            var n = code.Length;
            var b = oldBase;
            

            return code;
        }

        public static string GetCode(long decimalNumber)
        {
            return DecimalToArbitrarySystem(decimalNumber, Digits.Length);
        }
        public static string DecimalToArbitrarySystem(long decimalNumber, int radix)
        {
            const int BitsInLong = 64;


            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber /= radix;
            }

            string result = new(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }
    }
}
