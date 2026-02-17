using System;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class NewsAttachment
    {
        public Guid Id { get; set; }
        public Guid AttachmentId { get; set; }
        public int NewsAttachmentTypeId { get; set; }
        public String Document { get; set; }
        public string Extension { get; set; }      

    }
}

