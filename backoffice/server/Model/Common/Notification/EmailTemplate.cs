using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Common.Notification
{
    public class EmailTemplate : Entity
    {
        public ECCSAModuleFlag Owner { get; set; }
        public String Name { get; set; }
        public List<EmailPlaceHolder> PlaceHolders { get; set; }
    }


}
