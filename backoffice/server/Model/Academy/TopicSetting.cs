using System;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class TopicSetting
    {
        public Guid Id { get; set; }
		public Guid TopicId { get; set; }
		public int SettingVariableId { get; set; }
		public String SettingValue { get; set; }
		

        

        public virtual Topic Topic { get; set; }
public virtual SettingVariable SettingVariable { get; set; }

    }
}