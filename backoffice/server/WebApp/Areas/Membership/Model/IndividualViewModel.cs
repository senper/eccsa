using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class IndividualViewModel : MembershipViewModel<MembershipEnrollment>
    {

        public List<Association> Associations { get; set; } = new List<Association>();

        public Guid AssociationId { get; set; }
        public IndividualStaff Staff { get; set; } = new IndividualStaff();
        public List<IndividualStaff> Staffs { get; set; } = new List<IndividualStaff>();

        public Guid LocationId { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();

        public List<BusinessProfile> BusinessProfiles = new List<BusinessProfile>();

        public List<LegalDocument> LegalDocuments = new List<LegalDocument>();

        public LegalDocument LegalDocument { get; set; } = new LegalDocument();

        public BusinessProfile BusinessProfile { get; set; } = new BusinessProfile();
        public List<PositionType> PositionTypes { get; set; } = new List<PositionType>();

        public List<LegalDocumentType> LegalDocumentTypes { get; set; } = new List<LegalDocumentType>();
        public List<ProfileAttribure> ProfileAttribures { get; set; } = new List<ProfileAttribure>();

        public RenewalHistory RenewalHistory { get; set; }
        public Individual TheMember { get; set; }
        public string AssociationJson { get; internal set; }

        public Invoice Invoice { get; set; }

        public Settlment Payment { get; set; }

        public List<LookupValue> Sectors { get; set; }

        public bool ShowAssociations { get; set; } = true;
        public IndividualBusiness License { get; set; } = new IndividualBusiness();
        public IndividualManufacturing Manufacturing { get; set; } = new IndividualManufacturing();
        public IndividualService Service { get; set; } = new IndividualService();
        public IndividualTrade Trade { get; set; } = new IndividualTrade();

    }

   
    public class InvoiceViewModel
    {
        public Guid Id { get; set; }
        public Guid AssociationId { get; set; }

        public decimal Fee { get; set; }
    }

    public class IndividualImportModel : IId
    {
        public IndividualImportModel()
        {

        }
        public Guid Id { get; set; }
        public String CompanyName { get; set; }
        public String TIN { get; set; }
        public String Mobile { get; set; }
        public String MemberCode { get; set; }
        public String Sector { get; set; }
    }
}
