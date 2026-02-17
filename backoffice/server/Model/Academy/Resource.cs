using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class LearningResource
    {
        public Guid Id { get; set; }
		public Guid TopicId { get; set; }
		public String Name { get; set; }
		public String Description { get; set; } 
		public ResourceType ResourceType { get; set; }
		public String ResourceContent { get; set; }
		public int Status { get; set; }


        public virtual ICollection<ResourceDelivery> ResourceDeliveries { get; set; }


        public virtual Topic Topic { get; set; }

    }
}