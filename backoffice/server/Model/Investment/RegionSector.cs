using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Investment
{
    public class RegionSector
    {
        public static readonly Guid EthiopiaId = new Guid("00000000-0000-0000-0000-0000000f4240");
        public Guid Id { get; set; }

        [NotMapped]
        public String NameFull => $"{Name} {NameLocal}";
        public string Name { get; set; }
        public String NameLocal { get; set; }
        public bool IsRegion { get; set; }

        public String Profile { get; set; }
        public String ProfileLocal { get; set; }

        [NotMapped]
        public bool IsCountry => Id == EthiopiaId;
        [NotMapped]
        public string Type
        {
            get
            {
                if (Id == EthiopiaId) return "Country";
               return  IsRegion ? "Region" : "Sector";
            }
        }
    }
}
