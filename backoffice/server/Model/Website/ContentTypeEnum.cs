using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public enum ContentTypeEnum
    {
        None = 0,
        [Display(Name = "About The Chamber")]
        AboutTheChamber = 100,
        [Display(Name = "General assembly")]
        GeneralAssembly = 101,
        [Display(Name = "Board of Directors")]
        BoardOfDirectors = 102,
        [Display(Name = "The Secretariat")]
        TheSecretariat = 103,
        [Display(Name = "Former Presidents")]
        FormerPresidents = 104,
        [Display(Name = "Our Location")]
        OurLocation = 105,
        [Display(Name = "Our Partners")]
        OurPartners = 106,
        StartegicPlan = 107,
        ChamberLaw = 108,
        ECCSAProjects = 109,
        FormerSecretaryGenerals = 110,

        Services = 120,
        Academy = 121,
        Arbitration = 122,
        [Display(Name = "Trade Facilitation")]
        TradeFacilitation = 123,
        [Display(Name = "Trade Promotion")]
        TradePromotion = 124,
        [Display(Name = "Match Making")]
        MatchMaking = 125,
        Research = 126,
        
        Membership = 130,
        Events = 135,
        
        Media = 140,
        News = 141,
        [Display(Name = "Ethiopia Profile")]
        PressRelease = 142,
        Speeches = 143,
        Annoucement = 144,
        Blog = 145,
        PhotoGallery = 146,
        VideoGallery = 147,
        Newsletter = 148,

        Resources = 150,
        Directory = 151,
        [Display(Name = "Public Private Dialog")]
        PPD = 152,
        
        Profile = 160,
        [Display(Name = "Ethiopia Profile")]
        EthiopiaProfile = 161,
        [Display(Name = "Region Profile")]
        RegionProfile = 162,
        [Display(Name = "Sector Profile")]
        SectorProfile = 163,

        [Display(Name = "Trade Law Proclamation")]
        TradeLawProclamation = 170,
        [Display(Name = "Trade Law Directive")]
        TradeLawDirective = 171,
        [Display(Name = "Trade Law Regulation")]
        TradeLawRegulation = 172,

        [Display(Name = "Investment Law Proclamation")]
        InvestmentLawProclamation = 173,
        [Display(Name = "Investment Law Directive")]
        InvestmentLawDirective = 174,
        [Display(Name = "Investment Law Regulation")]
        InvestmentLawRegulation = 175,

        [Display(Name = "Customs Law Proclamation")]
        CustomsLawProclamation = 176,
        [Display(Name = "Customs Law Directive")]
        CustomsLawDirective = 177,
        [Display(Name = "Customs Law Regulation")]
        CustomsLawRegulation = 178,

        [Display(Name = "Domestic Law Proclamation")]
        DomesticLawProclamation = 180,
        [Display(Name = "Domestic Law Directive")]
        DomesticLawDirective = 181,
        [Display(Name = "Domestic Law Regulation")]
        DomesticLawRegulation = 182,

        [Display(Name = "Land Law Proclamation")]
        LandLawProclamation = 183,
        [Display(Name = "Land Law Directive")]
        LandLawDirective = 184,
        [Display(Name = "Land Law Regulation")]
        LandLawRegulation = 185,

        [Display(Name = "Other Law Proclamation")]
        OtherLawProclamation = 186,
        [Display(Name = "Other Law Directive")]
        OtherLawDirective = 187,
        [Display(Name = "Other Law Regulation")]
        OtherLawRegulation = 188,

        ContactUs = 190,
        Issue = 191,
        BMO = 192,
    }

    public class SearchResult
    {
        public Guid Id { get; set; }

        public ContentTypeEnum ContentType { get; set; }

        public String Title { get; set; }

        public String Brief { get; set; }

        [NotMapped]
        public string ContentTypeText { get; set; }

        [NotMapped]
        public Guid ContentId => Id;

        [NotMapped]
        public int ContentTypeInt => (int)ContentType;
    }

   
}
