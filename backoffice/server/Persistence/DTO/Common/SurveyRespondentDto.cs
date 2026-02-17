using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateSurveyRespondentInput (
		Guid SurveyId
	, Guid RespondentStakeholderId
	, DateTime StartDate
	, DateTime EndDate
	, String Remark
				);


public record UpdateSurveyRespondentInput (
	Guid Id
	, Guid SurveyId
	, Guid RespondentStakeholderId
	, DateTime StartDate
	, DateTime EndDate
	, String Remark
				);




    
    
    public record SurveyRespondentPayload(SurveyRespondent PayLoad);
    
}
