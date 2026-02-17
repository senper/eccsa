
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
        public async Task<TrainingPayload> CreateTraining(
                 CreateTrainingInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] TrainingRepository  trainingRepository
             // , [Service] ITrainingEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateTrainingInput>(data, userId);
            var val = await trainingRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<TrainingPayload> UpdateTraining(
            UpdateTrainingInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] TrainingRepository  trainingRepository
         //, [Service] ITrainingEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateTrainingInput>(data, userId);
            var val = await trainingRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteTraining(Training data
        , [Service] TrainingRepository  trainingRepository
            // , [Service] ITrainingEventSender eventSender
            )
        {
           await trainingRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
