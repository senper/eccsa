using System;
using System.ComponentModel.DataAnnotations;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public enum ProfileAttributeTypeEnum
    {
        Text = 0,
        Numeric = 1,
        Date = 2,
        Lookup = 3
    }
    public enum RequirementLevelEnum
    {
        NotRequired = 0,
        Mandatory = 1,
        Optional = 2
    }
    public enum StakeholderTypeEnum
    {
        [Display(Name = " ")]
        NotSet = 0,
        Association = 1,
        Individual = 2,
        Board = 3,
        [Display(Name = "Staff")]
        Management = 4,
        Authenticated = 5,
        Public = 6

    }

   

    public enum MembershipWorkflowEnum
    {
        [Display(Name = "Not Set")]
        NotSet = 0,
        Started = 1,
        Submitted = 2,
        [Display(Name = "Invoice Generated")]
        InvoiceGenerated = 3,
        [Display(Name = "Payment Made")]
        PaymentMade = 4,
        Accepted = 5,
        Rejected = 9
    }
    public enum AssociationTypeEnum
    {
        [Display(Name=" ")]
        NotSet = 0,
        Administrative = 1,
        Sectoral = 2
    }

    public enum LevelEnum
    {
        National = 0,
        Regional = 1,
        Zonal = 2,
        Woreda = 3
    }

    public enum ContactTypeEnum
    {
        [Display(Name = " ")]
        NotSet = 0,
        Mobile = 1,
        Email = 2,
        Website = 3,
        Landline = 4,
        Fax = 5,
        Facebook = 6,
        Twitter = 7,
        Physical = 9
    }

    public enum LegalStatusEnum
    {
        [Display(Name = " ")]
        NotSet = 0,
        Proprietary = 1,
        PLC = 2,
        ShareCompany = 3
    }

    



    public enum DocumentReviewStatusEnum
    {
        [Display(Name = " ")]
        NotSet = 0,
        Submitted = 1,
        Accepted = 2,
        Rejected = 3,
        [Display(Name ="Not Applicable")]
        NotApplicable = 4
        
    }


    [Flags]
    public enum TradeSectorFlag
    {
        Manufacturing = 1,
        Trade = 2,
        Service = 4,
    }

    [Flags]
    public enum BusinessTypeFlag
    {
        Export = 1,
        Import = 2,
        Agency = 4,
    }

    
    public enum ServiceTypeFlag
    {
        [Display(Name = " ")]
        None = 0,
        Industrial = 1,
        Craft = 2,
        Service = 4,
    }
    
    public enum AgencySpecializationFlag
    {
        [Display(Name =" ")]
        None = 0,
        [Display(Name = "Commission Agent")]
        CommissionAgent = 1,
        [Display(Name = "Commercial Broker")]
        CommercialBroker = 2,
        [Display(Name = "Commercial Reprsentative")]
        CommercialReprsentative = 4,
        [Display(Name = "Commercial Agent")]
        CommercialAgent = 8,
    }
}
