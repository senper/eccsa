using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class Location
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public String NameEng { get; set; }
        public String NameLocal { get; set; }

        [NotMapped]
        public String Name { get
            {
                return $"{(string.IsNullOrEmpty(NameEng) ? string.Empty: NameEng)} {(string.IsNullOrEmpty(NameLocal) ? string.Empty : NameLocal)}";
            } }
        public Guid ParentId { get; set; }
        public int Level { get; set; }

        public Guid? Level1Id { get; set; }
        public String Level1NameEng { get; set; }
        public String Level1NameLocal { get; set; }
        public Guid? Level2Id { get; set; }
        public String Level2NameEng { get; set; }
        public String Level2NameLocal { get; set; }

        public virtual ICollection<Association> Associations { get; set; }

        public virtual ICollection<Individual> Individuals { get; set; }
    }
}