using DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace iDFEmailService.Model.Notification
{
    public enum DeliveryStatus
    {
        NotSet = 0,
        Planned = 1,
        Sent = 2,
        Delivered = 3,
        NotDelivered = 4
    }
    public enum MessageStatus
    {
        Draft = 0,
        Submitted = 1,
        Processed = 2,
        Canceled = 3,
        Closed = 4
    }
    public class EmailDeliveryInfo
    {
        public Guid Id { get; set; }
        public String SenderEmail { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
        public int Count { get; set; } = 0;
        public List<DeliveryDetail> Details { get; set; } = new List<DeliveryDetail>();
    }

    public class DeliveryDetail
    {
        public Guid EmailId { get; set; }
        public String Subject { get; set; }
        public DateTime CreateStamp { get; set; } = DateTime.UtcNow;
        public List<MessageDelivery> SentList { get; set; }
    }
    public class EmailMessage 
    {
        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreateStamp { get; set; }

        public Guid UpdatedBy { get; set; }

        public DateTime UpdateStamp { get; set; }
        public int Owner { get; set; }
        public String Subject { get; set; }

        public Guid TemplateId { get; set; }       

        public String Body { get; set; }

        public List<MessageDelivery> Recipients { get; set; }

        public string Attachment { get; set; }

        public MessageStatus Status { get; set; }
        public Guid? ReferenceId { get; set; }
        public long IdentityId { get; set; }

    }

    public class SMSMessage
    {
        public Guid Id { get; set; }
        public String Message { get; set; }

        public int Owner { get; set; }

        public MessageStatus Status { get; set; }
        public int Priority { get; set; }
        public List<MessageDelivery> Recipients { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreateStamp { get; set; }

        public Guid UpdatedBy { get; set; }

        public DateTime UpdateStamp { get; set; }
    }


}
