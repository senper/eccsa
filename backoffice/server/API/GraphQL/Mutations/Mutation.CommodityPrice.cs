
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
        public async Task<CommodityPricePayload> CreateCommodityPrice(
                 CreateCommodityPriceInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] CommodityPriceRepository  commodityPriceRepository
             // , [Service] ICommodityPriceEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateCommodityPriceInput>(data, userId);
            var val = await commodityPriceRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<CommodityPricePayload> UpdateCommodityPrice(
            UpdateCommodityPriceInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] CommodityPriceRepository  commodityPriceRepository
         //, [Service] ICommodityPriceEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateCommodityPriceInput>(data, userId);
            var val = await commodityPriceRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteCommodityPrice(CommodityPrice data
        , [Service] CommodityPriceRepository  commodityPriceRepository
            // , [Service] ICommodityPriceEventSender eventSender
            )
        {
           await commodityPriceRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
