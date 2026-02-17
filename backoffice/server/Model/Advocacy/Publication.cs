using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Advocacy
{
    public class Publication : Entity
    {
        public String Title { get; set; }
        public String TitleLocal { get; set; }

        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";

        public String Description { get; set; }

        public String DescriptionLocal { get; set; }

        public String Keywords { get; set; }

        public String Url { get; set; }

        public String Funding { get; set; }
        public String FundingLocal { get; set; }

        [NotMapped]
        public String FundingFull => $"{Funding} {FundingLocal}";

        public bool IsPublic { get; set; }

        public StatusEnum Status { get; set; }

        public String Remark { get; set; }

        public String RemarkLocal { get; set; }
        public String Author { get; set; }

        public String AuthorLocal { get; set; }


        public DateTime PublicationDate { get; set; }
        [NotMapped]
        public string PublicationDateStr => PublicationDate.ToString("yyyy-MM-dd");


        public decimal? DigitalCost { get; set; }

        public string PrintUri { get; set; }
        public decimal? PrintCost { get; set; }

    }
}
