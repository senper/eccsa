using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Trade;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class AssociationViewModel : MembershipViewModel<Association>
    {
        public string BoardDirectorPath { get; set; } = "/assets/img/membership/person.png";
        public string SecretaryGeneralPath { get; set; } = "/assets/img/membership/person.png";
        public string LogoPath { get; set; } = "/assets/img/membership/logo.png";

        public List<Location> Locations { get; set; } = new List<Location>();


        public Staff Staff { get; set; } = new Staff();
        public AssociationSetting Setting { get; set; } = new AssociationSetting();
        public MembershipRequirement MembershipRequirement { get; set; } = new MembershipRequirement();

        public List<Staff> BoardMembers = new List<Staff>();

        public List<Staff> KeyStaffs = new List<Staff>();


        public Staff BoardChairman { get; set; } = new Staff();
        public Staff CEO { get; set; } = new Staff();
        public List<PositionType> PositionTypes { get; set; } = new List<PositionType>();

        public List<LegalDocumentType> legalDocumentTypes { get; set; } = new List<LegalDocumentType>();
        public PositionType PositionType { get; set; } = new PositionType();
        public List<string> ProfileImages = new List<string>();

        public AssociationProfile BusinessProfile { get; set; } = new AssociationProfile();
        public List<ProfileAttribure> ProfileAttribures { get; set; } = new List<ProfileAttribure>();

        public List<LookupType> LookupTypes { get; set; }

        public List<MembershipRequirement> Groups { get; set; }

        public bool HasServiceRequest { get; set; }

        public bool ShowNew { get; set; } = true;

        public List<SimpleLookup> InputeTypes
        {
            get
            {
                List<SimpleLookup> lst = new List<SimpleLookup>();
                foreach (ServiceInputTypeEnum val in Enum.GetValues(typeof(ServiceInputTypeEnum)))
                {
                    if (val == ServiceInputTypeEnum.Payment) continue;
                    lst.Add(new SimpleLookup()
                    {
                        Id = (int)val,
                        Name = val.GetDisplayName(),
                    });
                }
                return lst;
            }
        }

    }

    public class DashboardViewModel : MembershipViewModel<Association>
    {

        public string TransactionDate { get; set; }

        public List<ExchangeRate> ExchangeRates { get; set; }
        public List<PeriodReport> BusinessInformation { get; set; }

        public List<CommodityPrice> CommodityPrices { get; set; }
        public int AssociationsCount { get; set; }
        public int Members { get; set; }
        public int Applicants { get; set; }
        public bool IsMemberUser { get; set; }
    }
    public class PrintModel
    {
        public string Assocation { get; set; }
        public List<PrintDetailModel> Members { get; set; }
    }
    public class PrintDetailModel
    {
        public string Position { get; set; }
        public string MemberName { get; set; }
    }
    public class PeriodReport
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Value { get; set; }
    }
}
