using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateAssessmentInput (
		Guid TrainingId
	, Guid RegistrationId
	, DateTime Date
	, int ResultScore
	, int ResultPass
				);


public record UpdateAssessmentInput (
	Guid Id
	, Guid TrainingId
	, Guid RegistrationId
	, DateTime Date
	, int ResultScore
	, int ResultPass
				);




    
    
    public record AssessmentPayload(Assessment PayLoad);
    
}
