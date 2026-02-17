using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
	public record CreateTopicInput (
		String Code
	, String NameEng
	, String NameLocal
	, int CategoryId
	, String Decription
	, String DecriptionLocal
	, int Status
		,bool IsPublic
				);


public record UpdateTopicInput (
	Guid Id
	, String Code
	, String NameEng
	, String NameLocal
	, int CategoryId
	, String Decription
	, String DecriptionLocal
	, int Status
				);




	
	
	public record TopicPayload(Topic PayLoad);
	
}
