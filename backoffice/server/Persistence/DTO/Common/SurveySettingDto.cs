using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Common
{
    public record CreateSurveySettingInput (
		String Title
	, String Description
	, ECCSAModuleFlag ResponsibleUnit
	, Boolean IsRecurrent
	, FrequencyEnum Recurrence
	, int OpenDays
	, MediaOutletFlag PublishThrough
				);


public record UpdateSurveySettingInput (
	Guid Id
	, String Title
	, String Description
	, ECCSAModuleFlag ResponsibleUnit
	, Boolean IsRecurrent
	, FrequencyEnum Recurrence
	, int OpenDays
	, MediaOutletFlag PublishThrough
				);




    
    
    public record SurveySettingPayload(SurveySetting PayLoad);
    
}
