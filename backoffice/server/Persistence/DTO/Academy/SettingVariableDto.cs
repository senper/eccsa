using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateSettingVariableInput (
		String Code
	, String NameEng
	, String NameLocal
	, SettingVariableDataType DataType
	, int MinValue
	, int MaxValue
);


public record UpdateSettingVariableInput (
	int Id
	, String Code
	, String NameEng
	, String NameLocal
	, SettingVariableDataType DataType
	, int MinValue
	, int MaxValue
);




    
    
    public record SettingVariablePayload(SettingVariable PayLoad);
    
}
