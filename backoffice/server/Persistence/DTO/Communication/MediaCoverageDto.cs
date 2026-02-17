using ECCSA.BackOffice.Server.Model.Communication;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Communication
{
    public record CreateMediaCoverageInput (
		Guid MediaAgencyId
	, DateTime StartDate
	, DateTime EndDate
	, int DurationMinutes
	, int Frequency
	, int CoveredInFalg
	, String About
	, String Remark
				);


public record UpdateMediaCoverageInput (
	Guid Id
	, Guid MediaAgencyId
	, DateTime StartDate
	, DateTime EndDate
	, int DurationMinutes
	, int Frequency
	, int CoveredInFalg
	, String About
	, String Remark
				);




    
    
    public record MediaCoveragePayload(MediaCoverage PayLoad);
    
}
