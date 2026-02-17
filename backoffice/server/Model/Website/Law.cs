using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class Law : Entity
    {
        public String Title { get; set; }
        public String TitleLocal { get; set; }

        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";
        public String Brief { get; set; }
        public String BriefLocal { get; set; }
        [NotMapped]
        public String BriefFull => $"{Brief} {BriefLocal}";
        public String Extension { get; set; }
        public String ExtensionLocal { get; set; }
        public LawTypeEnum LawType { get; set; }
        public LawCategoryEnum Category { get; set; }

        public override bool IsValid()
        {


            return true;
        }
    }


   

}
