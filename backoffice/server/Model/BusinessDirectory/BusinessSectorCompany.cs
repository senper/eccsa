using System;

namespace ECCSA.BackOffice.Server.Model.BusinessDirectory
{
    public class BusinessSectorCompany
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid SectorId { get; set; }

        public virtual Company Company { get; set; }

        public virtual BusinessSector BusinessSector { get; set; }
    }
}
