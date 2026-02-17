using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ExportAgreement : Entity
    {
        public Guid UserId { get; set; }
        public string AgreementNo { get; set; }
        public string LicenseNo { get; set; }
        public string Consignee {get;set;}
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string TypeOfCarriage { get; set; }
        public string CountyOfDestination { get; set; }
        public string PortOfDestination { get; set; }
        public string PortOfDeparture { get; set; }
        public string NameOfCarrier { get; set; }
        public string TermOfDelivery { get; set; }
        public bool IsLocked { get; set; }
        public StatusEnum Status { get; set; }
        public virtual ICollection<AgreementCommodity> Commodities { get; set; }
    }

    public class AgreementCommodity
    {
        public Guid Id { get; set; }
        public Guid AgreementId { get; set; }   
        public virtual ExportAgreement Agreement { get; set; }
        public String HSCode { get; set; }
        public String Name { get; set; }
    }
}
