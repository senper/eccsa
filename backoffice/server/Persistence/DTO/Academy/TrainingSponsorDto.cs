using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateTrainingSponsorInput (
		Guid TrainingId
	, Guid SponsorId
		,string ReferenceNo
		,string Attachment
	, int Status
);


public record UpdateTrainingSponsorInput (
	Guid Id
	, Guid TrainingId
	, Guid SponsorId
		, string ReferenceNo
		, string Attachment
	, int Status
);




    
    
    public record TrainingSponsorPayload(TrainingSponsor PayLoad);
    
}
