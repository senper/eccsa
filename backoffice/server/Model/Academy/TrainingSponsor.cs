using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class TrainingSponsor
    {
        public Guid Id { get; set; }
        public string ReferenceNo { get; set; }
        public Guid TrainingId { get; set; }
        public Guid SponsorId { get; set; }
        public string Attachment { get; set; }
        public int Status { get; set; }


        public virtual ICollection<Registration> Registrations { get; set; }


        public virtual Training Training { get; set; }
        public virtual Sponsor Sponsor { get; set; }

    }
}