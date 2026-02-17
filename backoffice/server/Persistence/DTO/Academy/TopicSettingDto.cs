using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateTopicSettingInput (
		Guid TopicId
	, int SettingVariableId
	, String SettingValue
);


public record UpdateTopicSettingInput (
	Guid Id
	, Guid TopicId
	, int SettingVariableId
	, String SettingValue
);




    
    
    public record TopicSettingPayload(TopicSetting PayLoad);
    
}
