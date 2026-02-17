using ECCSA.BackOffice.Server.Model.Membership;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ServiceRequirement
    {
        public Guid Id { get; set; }

        public Guid ServiceTypeId { get; set; }

        public Guid? ParentId { get; set; }

        [NotMapped]
        public bool IsParent { get; set; }

        [NotMapped]
        public bool HasChild { get; set; }

        public RequirementTypeEnum RequirementType { get; set; }
        [NotMapped]
        public int RequirementTypeValue => (int)RequirementType;

        public string OrderNo { get; set; }

        public string NameEng { get; set; } = string.Empty;

        public string NameLocal { get; set; } = string.Empty;

        public string Label { get; set; } = string.Empty;
        public string LabelLocal { get; set; } = string.Empty;
        [NotMapped]
        public String Name
        {
            get
            {
                return $"{NameEng} {NameLocal}";
            }
        }

       
        public string DescriptionEng { get; set; } = string.Empty;

        public string DescriptionLocal { get; set; } = string.Empty;



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
        public bool IsForOpponent { get; set; }
        [NotMapped]
        public List<SimpleLookup> SelectSources { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<ServiceRequirementResponse> Responses { get; set; }
        public virtual ICollection<ServiceTypeReport> ServiceTypeReports { get; set; }
    }

}