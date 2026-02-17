using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateSurveyChoiceInput (
		Guid SurveySettingId
	, String OrderNo
	, Guid QuestionId
	, String ChoiceText
);


public record UpdateSurveyChoiceInput (
	Guid Id
	, Guid SurveySettingId
	, String OrderNo
	, Guid QuestionId
	, String ChoiceText
);




    
    
    public record SurveyChoicePayload(SurveyChoice PayLoad);
    
}
