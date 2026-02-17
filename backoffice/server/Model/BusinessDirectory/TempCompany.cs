using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.BusinessDirectory
{
    public class TempCompany: IAddress
    {
        public Guid Id { get; set; }
        public Guid SectorId { get; set; }
        public String Name { get; set; }
        public String Telephone { get; set; }
        public String Mobile { get; set; }
        public String Fax { get; set; }
        public String Email { get; set; }
        public String PoBox { get; set; }
        public String Web { get; set; }
        public String Extension { get; set; }
        [NotMapped]
        public List<Company> TempList { get; set; } = new List<Company>();
        [NotMapped]
        public Guid NewId { get; set; } 
        public virtual BusinessSector BusinessSector { get; set; }  
    }
}
