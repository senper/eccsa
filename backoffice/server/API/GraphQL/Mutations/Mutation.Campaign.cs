
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<CampaignPayload> CreateCampaign(
                 CreateCampaignInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] CampaignRepository  campaignRepository
             // , [Service] ICampaignEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateCampaignInput>(data, userId);
            var val = await campaignRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<CampaignPayload> UpdateCampaign(
            UpdateCampaignInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] CampaignRepository  campaignRepository
         //, [Service] ICampaignEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateCampaignInput>(data, userId);
            var val = await campaignRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteCampaign(Campaign data
        , [Service] CampaignRepository  campaignRepository
            // , [Service] ICampaignEventSender eventSender
            )
        {
           await campaignRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
