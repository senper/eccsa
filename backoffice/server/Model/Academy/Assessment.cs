using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class Assessment : Entity
    {
        public Guid TrainingId { get; set; }
        public Guid RegistrationId { get; set; }
        public DateTime Date { get; set; }
        public int ResultScore { get; set; }
        public int ResultPass { get; set; }




        public virtual Training Training { get; set; }
        public virtual Registration Registration { get; set; }

    }
}