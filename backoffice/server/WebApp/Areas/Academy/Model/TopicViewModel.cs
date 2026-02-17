
using ECCSA.BackOffice.Server.Model.Academy;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model
{
    public class TopicViewModel : AcademyViewModel<Topic>
    {
        public List<CategoryLookup> CategoryLookup { get; set; }
    }
    public class ResourceViewModel : AcademyViewModel<LearningResource>
    {
    }
    public class SettingViewModel : AcademyViewModel<TopicSetting>
    {
        public List<SettingVariable> SettingsLookup { get; set; }
    }

    public class CategoryLookup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
    }
}
