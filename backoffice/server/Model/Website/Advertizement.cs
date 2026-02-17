using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class Advertizement : Entity
    {
        public String FullName { get; set; }
        public String Email { get; set; }
        public String CompanyName { get; set; }

        [NotMapped]
        public String Url { get; set; }

        public String PhoneNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime  EndDate { get; set; }

        public AdvertisementSectionEnum Section { get; set; }
        public string Extension { get; set; }
    }
}
