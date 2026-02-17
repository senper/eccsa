using ECCSA.BackOffice.Server.Model.Website;
using System;


namespace ECCSA.BackOffice.Server.Persistence.DTO.Website
{
    public record CreateNewsAttachmentInput(
	  
	Guid AttachmentId
	, int NewsAttachmentTypeId
	, String Document
	, string Extension
);


    public record UpdateNewsAttachmentInput(
        
         Guid AttachmentId
        , int NewsAttachmentTypeId
        , String Document
        , string Extension


    );
    
   
    

    public record NewsAttachmentPayload(NewsAttachment PayLoad);

}

