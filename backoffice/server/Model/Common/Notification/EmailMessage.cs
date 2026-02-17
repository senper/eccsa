using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Notification
{
    public class EmailMessage : Entity
    {
        public ECCSAModuleFlag Owner { get; set; }
        public String Subject { get; set; }

        public Guid TemplateId { get; set; }
        [NotMapped]
        public EmailTemplate Template { get; set; }

        public String Body { get; set; }

        public List<MessageDelivery> Recipients { get; set; }

        public string Attachment { get; set; }

        public MessageStatus Status { get; set; }
        public Guid? ReferenceId { get; set; }

    }


}
