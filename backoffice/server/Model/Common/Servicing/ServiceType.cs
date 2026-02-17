using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Common.Notification;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ServiceType : Entity
    {

        public ECCSAModuleFlag ServiceOwner { get; set; }
        public CertificateTypeEnum CertificateType { get; set; } = CertificateTypeEnum.None;
        public int CertificateTypeValue => (int)CertificateType;
        [NotMapped]
        public string ServiceOwnerStr => ServiceOwner.ToString();
        public string NameEng { get; set; }

        public string NameLocal { get; set; }

        [NotMapped]
        public List<ServiceTypeWorkerDetail> WorkerDetails { get; set; }
        public List<ServiceTypeWorker> WorkerList { get; set; } 

        [NotMapped]
        public String Name
        {
            get
            {
                return $"{NameEng} {NameLocal}";
            }
        }

        public bool ApprovalRequired { get; set; } = true;

        public bool IsResourceServiceType { get; set; } = false;

        public bool ApprovalAttachmentRequired { get; set; } = true;

        public bool AllowAdditionalAttachments { get; set; } = false;

        public string DescriptionEng { get; set; }

        public string DescriptionLocal { get; set; }

        public RateBasisEnum RateBasis { get; set; }

        public decimal MemberServiceCharge { get; set; }
        public decimal NonMemberServiceCharge { get; set; }

        public bool IsArbitration { get; set; }
        public bool AllowOptionalEntries { get; set; }

        [NotMapped]
        public string ServiceCharge
        {
            get
            {
                if (RateBasis == RateBasisEnum.Adhoc)
                    return NonMemberServiceCharge > 0 ? $" [{NonMemberServiceCharge}]Birr" : "";
                if (RateBasis == RateBasisEnum.Flat)
                    return $" [{NonMemberServiceCharge}]Birr";
                return $"Members [{MemberServiceCharge}]Birr and  Others [{NonMemberServiceCharge}]Birr";
            }
        }

        public StatusEnum Status { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string ReportTemplate { get; set; }

        public bool HasChildren
        {
            get
            {
                if (ServiceRequests == null && ServiceRequirements == null) return false;
                if (ServiceRequirements.Count == 0) return false;
                return true;
            }
        }

        public virtual ICollection<ServiceRequirement> ServiceRequirements { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
        public virtual ICollection<ServiceTypeReport> ServiceTypeReports { get; set; }
        [NotMapped]
        public List<LookupType> RequirementLookups { get; set; }
        [NotMapped]
        public List<TabularDataSource> TabularRequirements { get; set; }
        [NotMapped]
        public List<LookupValue> OptionalEntryLabels { get; set; }

        public static ServiceType FromEvent(EventDetail eventDetail, RateBasisEnum rateBasis, decimal membersRate, decimal nonMemberRate, Guid userId)
        {
            return new ServiceType()
            {
                Id = eventDetail.DetailId,
                NameEng = $"{eventDetail.Event.Title} {eventDetail.Title}",
                NameLocal = $"{eventDetail.Event.TitleLocal} {eventDetail.TitleLocal}",
                DescriptionEng = eventDetail.Event.Description,
                DescriptionLocal = eventDetail.Event.DescriptionLocal,
                RateBasis = rateBasis,
                MemberServiceCharge = membersRate,
                NonMemberServiceCharge = nonMemberRate,
                ServiceOwner = eventDetail.Event.EventOwner,
                StartDate = eventDetail.StartDate,
                EndDate = eventDetail.EndDate,
                ApprovalRequired = false,
                ApprovalAttachmentRequired = false,
                Status = eventDetail.Event.Status == EventStatus.Published ? StatusEnum.Active : StatusEnum.Inactive,
                CreatedBy = userId,
                CreateStamp = DateTime.UtcNow,
                UpdatedBy = userId,
                UpdateStamp = DateTime.UtcNow
            };
        }

    }

    public class ServiceTypeWorker 
    {
        public Guid UserId { get; set; }
       
    }
    public class ServiceTypeWorkerDetail :  INotificationAddress
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
    }
}