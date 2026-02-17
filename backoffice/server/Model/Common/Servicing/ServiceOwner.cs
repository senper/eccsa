using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ServiceOwner
    {
        public ECCSAModuleFlag Id { get; set; }
        [NotMapped]
        public int IdInt => (int)Id;
        public String Title { get; set; }
        public String TitleLocal { get; set; }
        public String Description { get; set; }
        public String DescriptionLocal { get; set; }
        public StatusEnum Status { get; set; }
    }

}