using System;

namespace ECCSA.BackOffice.Server.Persistence.DTO
{
    public record GenericInput<T>(T PayLoad, Guid UserId);
}
