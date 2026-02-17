using ECCSA.BackOffice.Server.Model.Academy;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateRegistrationInput (
		Guid TrainingId
	, Guid LoginId
	, Guid? TrainingSponsorId
	, String FullName
	, String EmailAddress
	, String MobileNo
	, DateTime RegistrationDate
		, int DiscountCategory
				);


public record UpdateRegistrationInput (
	Guid Id
	, Guid TrainingId
	, Guid LoginId
	, Guid? TrainingSponsorId
	, String FullName
	, String EmailAddress
	, String MobileNo
	, DateTime RegistrationDate
		, int DiscountCategory
				);


	public record UpdateRegistrationFeedbackInput(
	Guid Id
	, int TrainingRating
	, String TrainingFeedback
	, String CertificateAttachment
				);



	public record RegistrationPayload(Registration PayLoad);
    
}
