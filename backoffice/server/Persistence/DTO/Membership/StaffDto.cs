using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO.Membership
{
    public record CreateIndividualStaffInput(
    String Title
    , String NameEng
    , String NameLocal
    , Guid PositionTypeId
    , Guid IndividualId
    , DateTime StartDate
    , DateTime EndDate
    , string Email
    , string Mobile
    , StatusEnum Status
    , byte[] Photo
    , String Extension
);


    public record UpdateIndividualStaffInput(
        Guid Id
        , String Title
        , String NameEng
        , String NameLocal
        , Guid PositionTypeId
        , Guid IndividualId
        , DateTime StartDate
        , DateTime EndDate
        , string Email
        , string Mobile
        , StatusEnum Status
        , byte[] Photo
        , String Extension
    );






    public record IndividualStaffPayload(IndividualStaff PayLoad);

    public record CreateStaffInput(
    String Title
    , String NameEng
    , String NameLocal
    , Guid PositionTypeId
    , Guid AssociationId
    , DateTime StartDate
    , DateTime EndDate
    , string Email
    , string Mobile
    , StatusEnum Status
    , byte[] Photo 
    , String Extension
);


    public record UpdateStaffInput(
        Guid Id
        , String Title
        , String NameEng
        , String NameLocal
        , Guid PositionTypeId
        , Guid AssociationId
        , DateTime StartDate
        , DateTime EndDate
        , string Email
        , string Mobile
        , StatusEnum Status
        , byte[] Photo
        , String Extension
    );






    public record StaffPayload(Staff PayLoad);

}
