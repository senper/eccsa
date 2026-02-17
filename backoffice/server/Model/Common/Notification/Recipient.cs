using ECCSA.BackOffice.Server.Model.Membership;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Notification
{
    public class RecipientSource : Entity
    {
        public String Name { get; set; }

        public String CompanyName { get; set; }

        public String MobileNo { get; set; }

        public String Email { get; set; }

        public ECCSAModuleFlag Owner { get; set; }

        public StakeholderTypeEnum StakeholderType { get; set; }

        public Guid? SectorId { get; set; }

        [NotMapped]
        public string Sector { get; set; }
        public string ImportSectorName { get; set; }
        public string ImportSubsectorName { get; set; }
        public string BusinessInterest { get; set; }
        public string Website { get; set; }
        public string OfficeTel { get; set; }
        public string Position { get; set; }

    }
    public class TempRecipientSource : IId
    {
        public Guid Id { get; set; }
        public String Name { get; set; }

        public String CompanyName { get; set; }

        public String MobileNo { get; set; }

        public String Email { get; set; }

        public ECCSAModuleFlag Owner { get; set; }

        public StakeholderTypeEnum StakeholderType { get; set; }

        public Guid? SectorId { get; set; }

        [NotMapped]
        public string Sector { get; set; }
        public string ImportSectorName { get; set; }
        public string ImportSubsectorName { get; set; }
        public string BusinessInterest { get; set; }
        public string Website { get; set; }
        public string OfficeTel { get; set; }
        public string Position { get; set; }

    }
    public class ImportRecipientSectorMap
    {
        public int Id { get; set; }
        public Guid SectorId { get; set; }
        public string ImportSectorName { get; set; }
    }
}
