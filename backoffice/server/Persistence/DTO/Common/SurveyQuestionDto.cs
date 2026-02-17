using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateSurveyQuestionInput (
		Guid SurveySettingId
	, String OrderNo
	, String Question
	, QuestionType QuestionType
	, decimal? MinRange
	, decimal? MaxRange
	, Boolean Cascaded
	, Guid? ParentChoiceId
);


public record UpdateSurveyQuestionInput (
	Guid Id
	, Guid SurveySettingId
	, String OrderNo
	, String Question
	, QuestionType QuestionType
	, decimal? MinRange
	, decimal? MaxRange
	, Boolean Cascaded
	, Guid? ParentChoiceId
);




    
    
    public record SurveyQuestionPayload(SurveyQuestion PayLoad);
    
}
