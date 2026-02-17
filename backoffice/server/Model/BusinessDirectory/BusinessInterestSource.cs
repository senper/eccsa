using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.BusinessDirectory
{
    public class BusinessInterestSource
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Telephone { get; set; }
        public String Email { get; set; }
        public String Description { get; set; }
        public Guid? UserId { get; set; }
        public String Address { get; set; }
        [NotMapped]
        public string Remark { get; set; }

        public virtual ICollection<BusinessInterestSourceSector> Sectors { get; set; }
        public virtual ICollection<BusinessInterestSourceDetail> Details { get; set; }
        public virtual ICollection<MatchMaking> Matches { get; set; }
    }
    public class BusinessInterestSourceSector
    {
        public Guid Id { get; set; }
        public Guid BusinessInterestSourceId { get; set; }
        public Guid SectorId { get; set; }

        public virtual BusinessInterestSource BusinessInterestSource { get; set; }

        public virtual BusinessSector BusinessSector { get; set; }
    }
    public class BusinessInterestSourceDetail
    {
        public Guid Id { get; set; }
        public Guid BusinessInterestSourceId { get; set; }
        public Guid? ProductTypeId { get; set; }
        public virtual BusinessInterestProduct ProductType { get; set; }
        public Guid BusinessType { get; set; }
        public String Description { get; set; }
        public String DescriptionNormalized { get; set; }
        public virtual BusinessInterestSource BusinessInterestSource { get; set; }
    }
}
