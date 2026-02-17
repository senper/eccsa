using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class News : Entity

    {
        public String Title { get; set; }

        public String TitleLocal { get; set; }
        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";

        public String Brief { get; set; }

        public String BriefLocal { get; set; }
        [NotMapped]
        public String BriefFull => $"{Brief} {BriefLocal}";

        public String Full { get; set; }

        public String FullLocal { get; set; }

        [NotMapped]
        public String FullFull => $"{Full} {FullLocal}";

        public string OrderNo { get; set; }

        public String Author { get; set; }

        public String AuthorLocal { get; set; }

        [NotMapped]
        public String AuthorFull => $"{Author} {AuthorLocal}";
        public DateTime NewsDate { get; set; }

        public string Extension { get; set; }
        public string ExtensionHome { get; set; }
        [NotMapped]
        public bool BannerSetted => File.Exists(BannerUrl);
        [NotMapped]
        public bool HomeBannerSetted => File.Exists(HomeBannerUrl);
        [NotMapped]
        public string BannerUrl =>
            Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot", "pr", "documents",
                   Id.ToString()[^15..].Replace("-", "") + Extension);

        public string BannerUrlRelative =>
            Path.Combine("pr", "documents",
                   Id.ToString()[^15..].Replace("-", "") + Extension);

        [NotMapped]
        public string HomeBannerUrl =>
            Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot", "pr", "documents", "home",
                   Id.ToString()[^15..].Replace("-", "") + ExtensionHome);

        public string HomeBannerUrlRelative =>
            Path.Combine("pr", "documents", "home",
                   Id.ToString()[^15..].Replace("-", "") + ExtensionHome);
    }
}
