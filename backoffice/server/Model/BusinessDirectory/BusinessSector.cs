using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.BusinessDirectory
{
    public class BusinessSector
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Guid? ParentId { get; set; }
        public virtual BusinessSector ParentSector {get;set;}
        public virtual ICollection<BusinessSector> SubSectors { get; set; }

        public int CompaniesCount => BusinessSectors == null? 0: BusinessSectors.Count;

        public virtual ICollection<BusinessSectorCompany> BusinessSectors { get; set; }
        public virtual ICollection<BusinessInterestSourceSector> BusinessInterestSourceSectors { get; set; }

        public virtual ICollection<TempCompany> TempCompanies { get; set; }
        [NotMapped]
        public string ParentName { get; set; }
       

        [NotMapped]
        public string GroupName
        {
            get
            {
                if (ParentSector == null) return string.Empty;
                return ParentSector.Name;
            }
        }
    }
    public class CompanyCountBySector
    {
        public Guid SectorId { get; set; }
        public int Count { get; set; }  
    }
}
