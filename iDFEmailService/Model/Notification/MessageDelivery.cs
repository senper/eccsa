using System;

namespace iDFEmailService.Model.Notification
{
    public enum CcGroup
    {
        To = 0,
        CC = 1,
        BCC = 2
    }
    public class MessageDelivery
    {

        public CcGroup CCGroup { get; set; } = CcGroup.To;
        public DeliveryStatus Status { get; set; }
        public String Address { get; set; }  // Phone no , email

        public String Label { get; set; }

       
    }
}
