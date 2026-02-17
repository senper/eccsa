using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Communication;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Communication
{
    public record CreateMediaAgencyInput (
		String NameEng
	, String NameLocal
	, MediaTypeFlag MediaType
	, int IsLocalAgency
	, MediaAgencyOwnershipType OwnershipType
	, String RepresentativeNameEng
	, String RepresentativeNameLocal
	, String ResponsiblePersonnelEng
	, String ResponsiblePersonnelLocal
	, String Email
	, String Mobile
	, String Telephone
	, String Url
	, String Description
	, String Remark
	, StatusEnum Status
				);


public record UpdateMediaAgencyInput (
	Guid Id
	, String NameEng
	, String NameLocal
	, MediaTypeFlag MediaType
	, int IsLocalAgency
	, MediaAgencyOwnershipType OwnershipType
	, String RepresentativeNameEng
	, String RepresentativeNameLocal
	, String ResponsiblePersonnelEng
	, String ResponsiblePersonnelLocal
	, String Email
	, String Mobile
	, String Telephone
	, String Url
	, String Description
	, String Remark
	, StatusEnum Status
				);




    
    
    public record MediaAgencyPayload(MediaAgency PayLoad);
    
}
