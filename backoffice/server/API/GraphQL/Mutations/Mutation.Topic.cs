
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
        public async Task<TopicPayload> CreateTopic(
                 CreateTopicInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] TopicRepository  topicRepository
             // , [Service] ITopicEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateTopicInput>(data, userId);
            var val = await topicRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }


        public async Task<TopicPayload> UpdateTopic(
            UpdateTopicInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] TopicRepository  topicRepository
         //, [Service] ITopicEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateTopicInput>(data, userId);
            var val = await topicRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteTopic(Topic data
        , [Service] TopicRepository  topicRepository
            // , [Service] ITopicEventSender eventSender
            )
        {
           await topicRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
