
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
        public async Task<TopicSettingPayload> CreateTopicSetting(
                 CreateTopicSettingInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] TopicSettingRepository  topicSettingRepository
             // , [Service] ITopicSettingEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateTopicSettingInput>(data, userId);
            var val = await topicSettingRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<TopicSettingPayload> UpdateTopicSetting(
            UpdateTopicSettingInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] TopicSettingRepository  topicSettingRepository
         //, [Service] ITopicSettingEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateTopicSettingInput>(data, userId);
            var val = await topicSettingRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteTopicSetting(TopicSetting data
        , [Service] TopicSettingRepository  topicSettingRepository
            // , [Service] ITopicSettingEventSender eventSender
            )
        {
           await topicSettingRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
