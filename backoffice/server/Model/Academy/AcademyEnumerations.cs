using System.ComponentModel.DataAnnotations;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public enum ResourceType
    {
        Link = 1,
        Attachment = 2,
        [Display(Name = "Hard Copy")]
        HardCopy = 3
    }

    public enum SettingVariableDataType
    {
        Int = 1,
        String = 2,
    }
    public enum InstructionMedia
    {
        [Display(Name = "On Line Self Paced")]
        OnLineSelfPaced = 1,
        [Display(Name = "On Line Virtual Class")]
        OnLineVirtualClass = 2,
        [Display(Name = "Class Room Based")]
        ClassRoomBased = 3
    }

    public enum TrainingStatus
    {
        New = 1,
        [Display(Name = "Open For Registration")]
        OpenForRegistration = 2,
        [Display(Name = "Registration Ended")]
        RegistrationEnded = 3,
        Closed = 5,
        Completed = 4
    }

    public enum RegistrationStatus
    {
        Apply = 1,
        [Display(Name = "Invoice Generated")]
        InvoiceGenerated = 2,
        PaymentMade = 3,
        Accepted = 4,
        Rejected = 5,
        Canceled = 6,
    }

    public enum DeliveryMethod
    {
        Online = 1,
        [Display(Name = "In Person")]
        InPerson = 2
    }
}
