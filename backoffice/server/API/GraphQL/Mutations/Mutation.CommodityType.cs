
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Trade;
using ECCSA.BackOffice.Server.Persistence.Repositories.Trade;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<CommodityTypePayload> CreateCommodityType(
                 CreateCommodityTypeInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] CommodityTypeRepository  commodityTypeRepository
             // , [Service] ICommodityTypeEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateCommodityTypeInput>(data, userId);
            var val = await commodityTypeRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<CommodityTypePayload> UpdateCommodityType(
            UpdateCommodityTypeInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] CommodityTypeRepository  commodityTypeRepository
         //, [Service] ICommodityTypeEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateCommodityTypeInput>(data, userId);
            var val = await commodityTypeRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteCommodityType(CommodityType data
        , [Service] CommodityTypeRepository  commodityTypeRepository
            // , [Service] ICommodityTypeEventSender eventSender
            )
        {
           await commodityTypeRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
