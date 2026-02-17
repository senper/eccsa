using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Common.EventManagement
{
    public class EventType
    {
        public Guid EventTypeId { get; set; }
        public String Title { get; set; }

        public String Description { get; set; }

        public StatusEnum Status { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
