using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class LegalDocument
    {
        public Guid Id { get; set; }
        public Guid IndividualId { get; set; }
        public Guid RequirementId { get; set; }
        public string Extension { get; set; }
        public String Remark { get; set; }
        public string TextResponse { get; set; }
        public DateTime? DateResponse { get; set; }
        public decimal? DecimalResponse { get; set; }
        public int? IntResponse { get; set; }

        public DateTime? DateSubmitted { get; set; }


        [NotMapped]
        public String RelativePath
        {
            get; set;
        }

        [NotMapped]
        public string Response
        {
            get
            {
                if (Requirement == null)
                {
                    return "?";
                }
                return Requirement.InputMethod switch
                {
                    ServiceInputTypeEnum.Upload => TextResponse,
                    ServiceInputTypeEnum.Integer => IntResponse.Value.ToString(),
                    ServiceInputTypeEnum.Decimal => DecimalResponse.Value.ToString(),
                    ServiceInputTypeEnum.Date => DateResponse.Value.ToString("yyyy-MM-dd"),
                    _ => TextResponse,
                };
            }
        }

        public Guid? LookupValueId { get; set; }


        public Guid? ReviewedBy { get; set; }

        public DateTime? DateReviewed { get; set; }
        public DocumentReviewStatusEnum ReviewStatus { get; set; }
        [NotMapped]
        public int ReviewStatusValue => (int)ReviewStatus;


        public virtual MembershipRequirement Requirement { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                return Requirement?.Name;
            }
        }



        public virtual Individual Individual { get; set; }

    }

    public class ApplicantDocument
    {
        public Guid Id { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid RequirementId { get; set; }
        public string Extension { get; set; }
        public String Remark { get; set; }


        public virtual Applicant Applicant  { get; set; }




        public string TextResponse { get; set; }
        public DateTime? DateResponse { get; set; }
        public decimal? DecimalResponse { get; set; }
        public int? IntResponse { get; set; }

        public DateTime? DateSubmitted { get; set; }


        [NotMapped]
        public String RelativePath
        {
            get; set;
        }

        [NotMapped]
        public string Response
        {
            get
            {
                if (Requirement == null)
                {
                    return "?";
                }
                return Requirement.InputMethod switch
                {
                    ServiceInputTypeEnum.Upload => TextResponse,
                    ServiceInputTypeEnum.Integer => IntResponse.Value.ToString(),
                    ServiceInputTypeEnum.Decimal => DecimalResponse.Value.ToString(),
                    ServiceInputTypeEnum.Date => DateResponse.Value.ToString("yyyy-MM-dd"),
                    _ => TextResponse,
                };
            }
        }

        public Guid? LookupValueId { get; set; }


        public Guid? ReviewedBy { get; set; }

        public DateTime? DateReviewed { get; set; }
        public DocumentReviewStatusEnum ReviewStatus { get; set; }
        [NotMapped]
        public int ReviewStatusValue => (int)ReviewStatus;


        public virtual MembershipRequirement Requirement { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                    return Requirement?.Name;
            }
        }


    }
}