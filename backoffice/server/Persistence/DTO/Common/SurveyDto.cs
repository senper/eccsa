using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateSurveyInput (
		Guid SettingId
	, String Title
	, String Description
	, DateTime StartDate
	, DateTime EndDate
	, String AnalysisDescription
	, String Attachment
				);


public record UpdateSurveyInput (
	Guid Id
	, Guid SettingId
	, String Title
	, String Description
	, DateTime StartDate
	, DateTime EndDate
	, String AnalysisDescription
	, String Attachment
				);




    
    
    public record SurveyPayload(Survey PayLoad);
    
}
