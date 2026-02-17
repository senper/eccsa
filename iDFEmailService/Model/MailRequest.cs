using System.Collections.Generic;

namespace iDFEmailService.Model
{
    public class MailRequest
    {
        public List<string> ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        //public List<IFormFile> Attachments { get; set; }
        public MailSettings SenderEmail { get; set; }
    }

   
}
