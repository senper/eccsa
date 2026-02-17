using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Academy
{
    public record CreateTrainingInput(
        Guid TopicId
    , DateTime StartDate
    , DateTime EndDate
    , String Schedule
    , String Instructor
    , InstructionMedia InstructionMedia
        // , TrainingStatus Status
        , decimal MemberFee
        , decimal NonMemberFee
        , RateBasisEnum RateBasis
        , bool HasMorning
        , bool HasAfternoon
        , bool HasEvening
                );

    public record UpdateTrainingInput(
        Guid Id
        , Guid TopicId
        , DateTime StartDate
        , DateTime EndDate
    , String Schedule
        , String Instructor
        , InstructionMedia InstructionMedia
      //  , TrainingStatus Status
        , decimal MemberFee
        , decimal NonMemberFee
        , RateBasisEnum RateBasis
        , Guid? ServiceTypeId
        , bool HasMorning
        , bool HasAfternoon
        , bool HasEvening
                    );
    public record UpdateTrainingStatusInput(
        Guid Id
        , TrainingStatus Status
                    );

    public record TrainingPayload(Training PayLoad);

}
