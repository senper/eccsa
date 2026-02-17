using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateLearningResourceInput (
		Guid TopicId
	, String Name
	, String Description
	, ResourceType ResourceType
	, String ResourceContent
	, int Status
);


public record UpdateLearningResourceInput (
	Guid Id
	, Guid TopicId
	, String Name
	, String Description
	, ResourceType ResourceType
	, String ResourceContent
	, int Status
);




    
    
    public record LearningResourcePayload(LearningResource PayLoad);
    
}
