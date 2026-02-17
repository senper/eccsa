
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
        public async Task<TrainingSponsorPayload> CreateTrainingSponsor(
                 CreateTrainingSponsorInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] TrainingSponsorRepository  trainingSponsorRepository
             // , [Service] ITrainingSponsorEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateTrainingSponsorInput>(data, userId);
            var val = await trainingSponsorRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<TrainingSponsorPayload> UpdateTrainingSponsor(
            UpdateTrainingSponsorInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] TrainingSponsorRepository  trainingSponsorRepository
         //, [Service] ITrainingSponsorEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateTrainingSponsorInput>(data, userId);
            var val = await trainingSponsorRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteTrainingSponsor(TrainingSponsor data
        , [Service] TrainingSponsorRepository  trainingSponsorRepository
            // , [Service] ITrainingSponsorEventSender eventSender
            )
        {
           await trainingSponsorRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
