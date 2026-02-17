using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class Individual : Entity 
    {
        [NotMapped] 
        public String No { get; set; }
        public String NameEng { get; set; } = string.Empty;
        public String NameLocal { get; set; } = string.Empty;
        public DateTime MemberSince { get; set; } = DateTime.Today;
        public String TaxIdNumber { get; set; } = string.Empty;

        public String Address { get; set; } = string.Empty;

        public Guid LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<BusinessProfile> BusinessProfiles { get; set; }
        public virtual ICollection<LegalDocument> LegalDocuments { get; set; }

        public virtual ICollection<IndividualBusiness> BusinessLicenses { get; set; }
        public virtual ICollection<IndividualManufacturing> Manufacturings { get; set; }
        public virtual ICollection<IndividualService> Services { get; set; }
        public virtual ICollection<IndividualTrade> Trades { get; set; }

        public virtual ICollection<IndividualStaff> Employees { get; set; }
        [NotMapped]
        public List<IndividualTrade> Exports => Trades == null ? new List<IndividualTrade>() : Trades.Where(t => t.TradeDirection.ToUpperInvariant() == "E").ToList();
        [NotMapped]
        public List<IndividualTrade> Imports => Trades == null ? new List<IndividualTrade>() : Trades.Where(t => t.TradeDirection.ToUpperInvariant() == "I").ToList();

        [NotMapped]
        public List<MembershipEnrollment> Memberships { get; set; } = new List<MembershipEnrollment>();
        
        public String TradeRegistrationNumber { get; set; } = string.Empty;



        public Guid AssociatedUserId { get; set; } = Guid.Empty;

        public Guid? SectorId { get; set; } = Guid.Empty;

        [NotMapped]
        public string Subsector { get; set; } = string.Empty;
        [NotMapped]
        public string Sector { get; set; } = string.Empty;


        public virtual Stakeholder Stakeholder { get; set; }

        public TradeSectorFlag TradeSector { get; set; }

        public BusinessTypeFlag BusinessType { get; set; }

        public ServiceTypeFlag ServiceType { get; set; }

        public AgencySpecializationFlag AgencySpecialization { get; set; }
        public List<CustomField> CustomFields { get; set; }
        public string Capital { get; set; }
        public string NumberofEmployees { get; set; }
    }

    public class CustomField
    {
        public String Text { get; set; }
        public String Value { get; set; }
    }
}