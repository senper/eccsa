
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model
{
    public class TrainingViewModel : AcademyViewModel<Training>
    {
        public Guid TopicId { get; set; }
        public List<TopicLookup> TopicLookup { get; set; }
        public List<ServiceRequest> Requests { get; set; }
        public Guid UserId { get; set; }
        public TrainingStatus Status { get; set; }
        public List<LookupValue> SurveySettings { get; set; }
    }
    public class TrainingSurvey
    {
        public Guid SettingId { get; set; }
        public Guid TrainingId { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class TrainingSponsorViewModel : AcademyViewModel<TrainingSponsor>
    {
        public List<Lookup2> SponsorsLookup { get; set; }
    }

    public class RegistrationViewModel : AcademyViewModel<Registration>
    {
        public List<Lookup2> SponsorsLookup { get; set; }
        public List<Lookup> DiscountCategory { get; set; }
    }
    public class AssessmentViewModel : AcademyViewModel<Assessment>
    {
    }
    public class AttendanceViewModel : AcademyViewModel<Attendance>
    {
    }
    public class DeliveryViewModel : AcademyViewModel<ResourceDelivery>
    {
        public List<Lookup2> ResourcesLookup { get; set; }
    }

    public class TopicLookup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
    public class TrainingCertificatePrintModel
    {
        public string FullName { get; set; }
        public string Topic { get; set; }
        public string Period { get; set; }
        public string PreparedBy { get; set; }
        public string PreparedByTitle { get; set; }
        public string Instrctor { get; set; }
    }
}
