using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.BusinessDirectory
{
    public class Company : IAddress
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Telephone { get; set; }
        public String Mobile { get; set; }
        public String Fax { get; set; }
        public String Email { get; set; }
        public String PoBox { get; set; }
        public String Web { get; set; }
        public String Extension { get; set; }

        public virtual ICollection<BusinessSectorCompany> BusinessSectors { get; set; }
    }
    public interface IAddress
    {
         String Telephone { get; set; }
         String Mobile { get; set; }
         String Fax { get; set; }
         String Email { get; set; }
         String PoBox { get; set; }
         String Web { get; set; }
         String Extension { get; set; }
    }
}
