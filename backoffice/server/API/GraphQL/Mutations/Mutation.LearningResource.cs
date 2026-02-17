
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
        public async Task<LearningResourcePayload> CreateLearningResource(
                 CreateLearningResourceInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] LearningResourceRepository  learningResourceRepository
             // , [Service] ILearningResourceEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateLearningResourceInput>(data, userId);
            var val = await learningResourceRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<LearningResourcePayload> UpdateLearningResource(
            UpdateLearningResourceInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] LearningResourceRepository  learningResourceRepository
         //, [Service] ILearningResourceEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateLearningResourceInput>(data, userId);
            var val = await learningResourceRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteLearningResource(LearningResource data
        , [Service] LearningResourceRepository  learningResourceRepository
            // , [Service] ILearningResourceEventSender eventSender
            )
        {
           await learningResourceRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
