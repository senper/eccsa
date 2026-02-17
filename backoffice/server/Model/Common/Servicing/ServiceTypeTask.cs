using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ServiceTypeTask
    {
        public Guid Id { get; set; }

        public Guid ServiceTypeId { get; set; }

        public string OrderNo { get; set; }
        public string NameEng { get; set; }

        public string NameLocal { get; set; }
        [NotMapped]
        public String Name
        {
            get
            {
                return $"{NameEng} {NameLocal}";
            }
        }
        public string DescriptionEng { get; set; }

        public string DescriptionLocal { get; set; }

        public virtual ServiceType ServiceType { get; set; }




        public StatusEnum Status { get; set; }

        
    }

}