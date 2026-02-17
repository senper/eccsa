using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class SettingVariable
    {
        public int Id { get; set; }
		public String Code { get; set; }
		public String NameEng { get; set; }
		public String NameLocal { get; set; }
		public SettingVariableDataType DataType { get; set; }
		public int MinValue { get; set; }
		public int MaxValue { get; set; }


        public virtual ICollection<TopicSetting> TopicSettings { get; set; }


        
    }
}