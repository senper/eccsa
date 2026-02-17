using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class OtherMedia : Entity
    {
       
        public int MediaTypeId { get; set; }

        public MediaType MediaType { get; set; }
        public String Title { get; set; } = string.Empty;

        public String TitleLocal { get; set; }
        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";

        public String Brief { get; set; } = String.Empty;

        public String BriefLocal { get; set; } = String.Empty;
        [NotMapped]
        public String BriefFull => $"{Brief} {BriefLocal}";

        public String Content { get; set; } = string.Empty;

        public String ContentLocal { get; set; }

        public DateTime DateCreated { get; set; }
        public string DateCreatedStr => DateCreated.ToString("dd/MM/yyyy");

        public Guid Author { get; set; }
        public String AuthorName { get; set; } = string.Empty;

        public String AuthorNameLocal { get; set; }
        [NotMapped]
        public String AuthorNameFull => $"{AuthorName} {AuthorNameLocal}";

        public int Views { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Shares { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
