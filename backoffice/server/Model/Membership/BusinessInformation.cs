using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class BusinessInformation : Entity
    {
        public string Title { get; set; }

        public String TitleLocal { get; set; }

        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";
        public string Description { get; set; }

        public String DescriptionLocal { get; set; }
        public string Link { get; set; }
        public DateTime EndDate { get; set; }
        public string Extension { get; set; }

        public bool ImageSetted => File.Exists(ImageUrl);
        [NotMapped]
        public string ImageUrl => Extension!= null && Extension.Length > 0 ? 
           Path.Combine(Directory.GetCurrentDirectory(),
                  "wwwroot", "membership", "documents",
                  Id.ToString()[^15..].Replace("-", "") + Extension):"";

        [NotMapped]
        public string ImageUrlRelative =>
            Path.Combine("membership", "documents",
                   Id.ToString()[^15..].Replace("-", "") + Extension);

    }

    public record BusinessInfoDescription(Guid Id, string Title, DateTime EndDate);
}
