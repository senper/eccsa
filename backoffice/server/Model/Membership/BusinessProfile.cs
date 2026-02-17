using System;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class BusinessProfile
    {
        public Guid Id { get; set; }
        public Guid IndividualId { get; set; }
        public int ProfileAttribureId { get; set; }
        public String AttributeValue { get; set; }

        public virtual Individual Individual { get; set; }
        public virtual ProfileAttribure ProfileAttribure { get; set; }

    }
    public class AssociationProfile
    {
        public Guid Id { get; set; }
        public Guid AssociationId { get; set; }
        public int ProfileAttribureId { get; set; }
        public String AttributeValue { get; set; }

        public virtual Association Association { get; set; }
        public virtual ProfileAttribure ProfileAttribure { get; set; }

    }
    public interface IIndividualDetail
    {
        Guid Id { get; set; }
        Guid IndividualId { get; set; }
    }
    public class IndividualService : IIndividualDetail
    {
        public Guid Id { get; set; }
        public Guid IndividualId { get; set; }
        public String SerNo { get; set; }
        public String ServiceName { get; set; }
        public virtual Individual Individual { get; set; }
    }

    public class IndividualManufacturing : IIndividualDetail
    {
        public Guid Id { get; set; }
        public Guid IndividualId { get; set; }
        public String SerNo { get; set; }
        public String Product { get; set; }
        public String Unit { get; set; }
        public String ProductionCapacity { get; set; }
        public String UtilizedCapacity { get; set; }
        public virtual Individual Individual { get; set; }
    }
    public class IndividualBusiness : IIndividualDetail
    {
        public Guid Id { get; set; }
        public Guid IndividualId { get; set; }
        public String SerNo { get; set; }
        public String BusinessType { get; set; }
        public String LicenseNo { get; set; }
        public int Capital { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime RenewalDate { get; set; }
        public virtual Individual Individual { get; set; }
    }

    public class IndividualTrade : IIndividualDetail
    {
        public Guid Id { get; set; }
        public Guid IndividualId { get; set; }
        public String SerNo { get; set; }
        public String TradeDirection { get; set; }
        public String ProductName { get; set; }
        public String Country { get; set; }
      
        public virtual Individual Individual { get; set; }
    }
}