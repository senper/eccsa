using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Common.Notification
{
    public class SMSMessage : Entity
    {
        public String Message { get; set; }

        public ECCSAModuleFlag Owner { get; set; }

        public MessageStatus Status { get; set; }
        public int Priority { get; set; }
        public List<MessageDelivery> Recipients { get; set; } 
    }


}
