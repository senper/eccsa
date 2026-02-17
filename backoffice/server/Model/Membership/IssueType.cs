using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class IssueType {
        public Guid Id { get; set; }
        public String Code { get; set; }
        public String NameEng { get; set; } = String.Empty;
        public String NameLocal { get; set; } = string.Empty;

        [NotMapped]
        public String Name
        {
            get
            {
                return $"{(string.IsNullOrEmpty(NameEng) ? string.Empty : NameEng)} {(string.IsNullOrEmpty(NameLocal) ? string.Empty : NameLocal)}";
            }
        }

        public StatusEnum Status { get; set; } = StatusEnum.Active;

        public virtual ICollection<Issue> Issues { get; set; }

        
    }

}
