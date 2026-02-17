using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class LegalProfile : Entity
    {
        public Guid UserId { get; set; }
        public LegalDocumentTypes LegalDocumentType { get; set; }
        [NotMapped]
        public int LegalDocumentTypeValue => (int)LegalDocumentType;
        public string DocumentNumber { get; set; }
        public string Title { get; set; }
        public DateTime? ExpiryDate { get; set; }
        [NotMapped]
        public string ExpiryDateStr => ExpiryDate?.ToString("yyyy-MM-dd") ?? "";
        public StatusEnum Status { get; set; }
        [NotMapped]
        public int StatusValue => (int)Status;
        [NotMapped]
        public bool IsExpired => ExpiryDate < DateTime.Today;
        public bool IsLocked { get; set; }
        public string FileExtension { get; set; }
    }
}
