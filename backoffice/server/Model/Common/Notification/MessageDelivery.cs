using System;

namespace ECCSA.BackOffice.Server.Model.Common.Notification
{
    public interface INotificationAddress
    {
        string FullName { get; set; }
        string Email { get; set; }
        string MobileNo { get; set; }
    }
    public class MessageDelivery
    {

        public CcGroup CCGroup { get; set; } = CcGroup.To;
        public DeliveryStatus Status { get; set; }
        public String Address { get; set; }  // Phone no , email

        public String Label { get; set; }

        public MessageDelivery()
        {



        }

        public MessageDelivery(string address, string label="", CcGroup ccGroup = CcGroup.To, DeliveryStatus status = DeliveryStatus.NotSet)
        {
            CCGroup = ccGroup;
            Status = status;
            Address = address;
            Label = string.IsNullOrEmpty(label)? address: label;
        }
    }
}
