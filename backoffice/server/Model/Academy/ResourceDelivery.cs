using System;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class ResourceDelivery
    {
        public DeliveryMethod DeliveryMethod { get; set; }
		public Guid Id { get; set; }
		public Guid ResourceId { get; set; }
		public Guid RegistrationId { get; set; }
		public DateTime DeliveryDate { get; set; }
		

        

        public virtual LearningResource Resource { get; set; }
public virtual Registration Registration { get; set; }

    }
}