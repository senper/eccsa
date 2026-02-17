using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class Sponsor
    {
        public Guid Id { get; set; }
		public String NameEng { get; set; }
		public String NameLocal { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
		public int Status { get; set; }


        public virtual ICollection<TrainingSponsor> TrainingSponsors { get; set; }


        
    }
}