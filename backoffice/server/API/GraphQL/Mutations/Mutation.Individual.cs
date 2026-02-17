
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<IndividualPayload> CreateIndividual(
                 CreateIndividualInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] IndividualRepository  individualRepository
             // , [Service] IIndividualEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateIndividualInput>(data, userId);
            var val = await individualRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<IndividualPayload> UpdateIndividual(
            UpdateIndividualInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] IndividualRepository  individualRepository
         //, [Service] IIndividualEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateIndividualInput>(data, userId);
            var val = await individualRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteIndividual(Individual data
        , [Service] IndividualRepository  individualRepository
            // , [Service] IIndividualEventSender eventSender
            )
        {
           await individualRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
