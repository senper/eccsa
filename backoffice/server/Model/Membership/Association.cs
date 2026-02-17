using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class Association : Entity
    {
        public static Association NULL
        {
            get
            {
                return new Association()
                {
                    Id = Guid.Empty,
                    NameEng = "",
                    NameLocal = "",
                    ShortName = "",
                    ShortName_Local = "",
                };
            }
        }
        public String NameEng { get; set; }
        public String NameLocal { get; set; }

        [NotMapped]
        public String Name
        {
            get
            {
                return $"{(string.IsNullOrEmpty(NameEng) ? string.Empty : NameEng)} {(string.IsNullOrEmpty(NameLocal) ? string.Empty : NameLocal)}";
            }
        }
        [NotMapped]
        public String NameShort
        {
            get
            {
                if (ShortName != null && ShortName.Trim().Length > 0) return ShortName;
                if (string.IsNullOrEmpty(NameEng)) return string.Empty;
                return NameEng.Trim().Replace("Chamber", "").Replace("chamber", "").Replace("of Commerce", "").Replace("Association", "");
            }
        }

        public String ShortName { get; set; }

        [NotMapped]
        public String NameShort_Local
        {
            get
            {
                if (ShortName_Local != null && ShortName_Local.Trim().Length > 0) return ShortName_Local;
                if (string.IsNullOrEmpty(NameLocal)) return string.Empty;
                return NameLocal.Trim().Replace("ቻምበር", "").Replace("ቻምበር", "").Replace("ቻምበር", "").Replace("አሶሴሽን", "");
            }
        }
        public String ShortName_Local { get; set; }
        public DateTime MemberSince { get; set; }

        [NotMapped]
        public string MembershipDate => MemberSince.ToString("yyyy-MM-dd");
        public AssociationTypeEnum AssociationType { get; set; }
        public LevelEnum AssociationLevel { get; set; }

        public Guid ParentId { get; set; }

        public Guid LocationId { get; set; }

        public decimal RegistrationFee { get; set; }

        public decimal RenewalFee { get; set; }
        
        //[NotMapped]
        public decimal? ProposedRegistrationFee { get; set; }
        
       // [NotMapped]
        public decimal? ProposedRenewalFee { get; set; }
        
       // [NotMapped]
        public Guid? ProposedBy { get; set; }

        //[NotMapped]
        public DateTime? DateProposed { get; set; }

        [NotMapped]
        public Stakeholder Stakeholder { get; set; }

        [NotMapped]
        public Association Parent { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<BoardMember> BoardMembers { get; set; }
        public virtual ICollection<MembershipEnrollment> MembershipEnrollments { get; set; }

        public virtual ICollection<MembershipRequirement> MembershipRequirements { get; set; }

        public virtual ICollection<AssociationSetting> Settings { get; set; }

        public virtual ICollection<AssociationProfile> AssociationProfiles { get; set; }

        public virtual ICollection<Staff> Employees { get; set; }
        [NotMapped]
        public bool AllowOnlineApplication
        {
            get
            {
                if (MembershipRequirements == null || MembershipRequirements.Count == 0) return false;
                if (RegistrationFee <= 1) return false;
                if (RenewalFee <= 1) return false;
                if (Settings == null || Settings.Count == 0) return false;
                return true;
            }
        }

        public String Profile { get; set; } = string.Empty;
        public String Profile_Local { get; set; } = string.Empty;
        public byte[] Logo { get; set; }
        public String Extension { get; set; }

        public Boolean EnableWebAccess { get; set; }
        [NotMapped]
        public List<LookupType> RequirementLookups { get; set; }
        public virtual ICollection<Applicant> Applicants { get; set; }
    }

    public class AssociationSetting
    {
        public Guid Id { get; set; }

        public Guid AssociationId { get; set; }

        public string BankName { get; set; }

        public String AccountNumber { get; set; }

        public ECCSAModuleFlag SubSystem { get; set; }

        //TODO: Implement feature of two person process request / approve 

        //[NotMapped]
        public string ProposedBankName { get; set; } = string.Empty;

        //[NotMapped]
        public String ProposedAccountNumber { get; set; } = string.Empty;

        //[NotMapped]
        public Guid? ProposedBy { get; set; } = Guid.Empty;

        //[NotMapped]
        public DateTime? DateProposed { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; } = Guid.Empty;

        public virtual Association Association { get; set; }

    }
    public class MembershipRequirement
    {
        public Guid Id { get; set; }

        public Guid AssociationId { get; set; }


        public string OrderNo { get; set; }


        public RequirementLevelEnum ApplicationRequirementLevel { get; set; }
        [NotMapped]
        public int ApplicationRequirementLevelValue => (int)ApplicationRequirementLevel;

        public RequirementLevelEnum RenewalRequirementLevel { get; set; }
        [NotMapped]
        public int RenewalRequirementLevelValue => (int)RenewalRequirementLevel;
        public virtual Association Association { get; set; }



        public RequirementTypeEnum RequirementType { get; set; }
        [NotMapped]
        public int RequirementTypeValue => (int)RequirementType;



        public String Name { get; set; } = string.Empty;


        public string Label { get; set; } = string.Empty;
        public string LabelLocal { get; set; } = string.Empty;



        public String Description { get; set; } = string.Empty;


        public ServiceInputTypeEnum InputMethod { get; set; } = ServiceInputTypeEnum.None;

        [NotMapped]
        public int InputMethodValue => (int)InputMethod;


        [NotMapped]
        public int CharacterLength => UpperLimit;

        [NotMapped]
        public string InputType { get; set; }

        public int UpperLimit { get; set; }
        public int LowerLimit { get; set; }

        public StatusEnum Status { get; set; } = StatusEnum.Active;

        public Guid? LookupTypeId { get; set; }

        public bool ApproveOnSubmit { get; set; }

        [NotMapped]
        public List<SimpleLookup> SelectSources { get; set; }

        [NotMapped]
        public bool HasApplicants { get; set; }

        public virtual ICollection<ApplicantDocument> ApplicantDocuments { get; set; }
        public virtual ICollection<LegalDocument> IndividualDocuments { get; set; }
    }
}