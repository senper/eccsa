using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateSurveyResponseInput (
		Guid SurveyRespondentId
	, Guid QuestionId
	, Decimal NumericResponse
	, String TextualResponse
);


public record UpdateSurveyResponseInput (
	Guid Id
	, Guid SurveyRespondentId
	, Guid QuestionId
	, Decimal NumericResponse
	, String TextualResponse
);




    
    
    public record SurveyResponsePayload(SurveyResponse PayLoad);
    
}
