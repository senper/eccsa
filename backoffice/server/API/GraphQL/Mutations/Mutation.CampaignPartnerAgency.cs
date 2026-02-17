
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
        public async Task<CampaignPartnerAgencyPayload> CreateCampaignPartnerAgency(
                 CreateCampaignPartnerAgencyInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] CampaignPartnerAgencyRepository  campaignPartnerAgencyRepository
             // , [Service] ICampaignPartnerAgencyEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateCampaignPartnerAgencyInput>(data, userId);
            var val = await campaignPartnerAgencyRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<CampaignPartnerAgencyPayload> UpdateCampaignPartnerAgency(
            UpdateCampaignPartnerAgencyInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] CampaignPartnerAgencyRepository  campaignPartnerAgencyRepository
         //, [Service] ICampaignPartnerAgencyEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateCampaignPartnerAgencyInput>(data, userId);
            var val = await campaignPartnerAgencyRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteCampaignPartnerAgency(CampaignPartnerAgency data
        , [Service] CampaignPartnerAgencyRepository  campaignPartnerAgencyRepository
            // , [Service] ICampaignPartnerAgencyEventSender eventSender
            )
        {
           await campaignPartnerAgencyRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
