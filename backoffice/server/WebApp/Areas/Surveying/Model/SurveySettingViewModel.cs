using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.AspNetCore.Http;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Surveying.Model
{
    public class SurveySettingViewModel:SurveyingViewModel<SurveySetting>
    {
        public SurveySettingViewModel()
        {
            ChangeActive(Models.MenuItemEnum.SurveySetting);
        }
        public String ChoiceJson { get; set; }

        public bool HasSurvey => TheModel != null && TheModel.Surveys.Count > 0;
    }

    public class SurveyViewModel : SurveyingViewModel<Survey>
    {
        public SurveyViewModel()
        {
            ChangeActive(Models.MenuItemEnum.Survey);
        }
        public String ChoiceJson { get; set; }

        public bool HasResponse => TheModel != null && TheModel.Respondents.Count > 0;
    }

    public class AttachmentViewModel 
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid SurveyId { get; set; }
        
        public IFormFile File { get; set; }
    }
}
