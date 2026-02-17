using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class ProfileAttribure
    {
        public int Id { get; set; }
        public String NameEng { get; set; }
        public String NameLocal { get; set; }

        public ProfileAttributeTypeEnum AttributeType { get; set; }

        public int Max { get; set; }

        public int Min { get; set; }

        public Guid? LookupTypeId { get; set; }

        public List<SimpleLookup> Source { get; set; }

        public bool ForAssociation { get; set; }

        public bool ForIndividual { get; set; }
        [NotMapped]
        public String Name
        {
            get
            {
                return $"{(string.IsNullOrEmpty(NameEng) ? string.Empty : NameEng)} {(string.IsNullOrEmpty(NameLocal) ? string.Empty : NameLocal)}";
            }
        }
        public virtual ICollection<BusinessProfile> BusinessProfiles { get; set; }

        public virtual ICollection<AssociationProfile> AssociationProfiles { get; set; }
    }
}