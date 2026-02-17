using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common
{

    public class GeneralSetting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class YesNo
    {
        public int Id { get; set; }
		public String Name { get; set; }
		public String NameLocal { get; set; }
       
    }

    
    public class LookupType
    {
        public static Guid Association = new("00000000-0000-0000-1000-000000000008");
        public static Guid OptionalRequirements = new("00000000-0000-0000-a000-000000000001");
        public Guid Id { get; set; }

        public String Name { get; set; }
        public StatusEnum Status { get; set; }
        public ECCSAModuleFlag Owner { get; set; }
        public bool IsHierarchical { get; set; } = false;

        public LookupValueTypeEnum ValueSourceType { get; set; }
        public LookupSourceTypeEnum SourceType { get; set; } = LookupSourceTypeEnum.NotSet;
        public virtual ICollection<LookupValue> LookupValues { get; set; }
    }
    public class LookupValue
    {

        public static LookupValue Default;

        static LookupValue()
        {
            Default = new LookupValue
            {
                Id = Guid.Empty,
                Name = string.Empty,
                Status = StatusEnum.Active,
                ParentId = null
            };
        }
        public LookupValue()
        {

        }
        public Guid Id { get; set; }

        [NotMapped]
        public String Text
        {
            get => Name;
            set => Name = value;
        }
        public String Name { get; set; }

        public string Code { get; set; } = string.Empty;

        public StatusEnum Status { get; set; }

        public Guid LookupTypeId { get; set; }
        public Guid? ParentId { get; set; }
        [NotMapped]
        public bool UseId { get; set; }
        public string Value { get; set; }

        public virtual LookupType LookupType { get; set; }

        [NotMapped]
        public string ParentName { get; set; } = string.Empty;
    }
}