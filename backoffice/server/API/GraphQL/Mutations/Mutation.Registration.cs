
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<RegistrationPayload> CreateRegistration(
                 CreateRegistrationInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] RegistrationRepository  registrationRepository
             // , [Service] IRegistrationEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateRegistrationInput>(data, userId);
            var val = await registrationRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<RegistrationPayload> UpdateRegistration(
            UpdateRegistrationInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] RegistrationRepository  registrationRepository
         //, [Service] IRegistrationEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateRegistrationInput>(data, userId);
            var val = await registrationRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteRegistration(Registration data
        , [Service] RegistrationRepository  registrationRepository
            // , [Service] IRegistrationEventSender eventSender
            )
        {
           await registrationRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
