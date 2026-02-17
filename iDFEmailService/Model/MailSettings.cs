using DocumentFormat.OpenXml.Office2010.ExcelAc;
using iDFEmailService.Model.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace iDFEmailService.Model
{
    public class MailSettings
    {
        public Guid Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int SMTPPort { get; set; }
        public int IMAPPort { get; set; }
        public int MaxEmailPerDay { get; set; }
        public DateTime CreateStamp { get; set; }
        public DateTime? ActivatedStamp { get; set; }
        public DateTime? DeactivatedStamp { get; set; }
        [NotMapped]
        public EmailDeliveryInfo DeliveryInfo { get; set; }
    }
}
