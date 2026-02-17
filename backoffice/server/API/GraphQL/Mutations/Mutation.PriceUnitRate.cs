
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
        public async Task<PriceUnitRatePayload> CreatePriceUnitRate(
                 CreatePriceUnitRateInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] PriceUnitRateRepository  priceUnitRateRepository
             // , [Service] IPriceUnitRateEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreatePriceUnitRateInput>(data, userId);
            var val = await priceUnitRateRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<PriceUnitRatePayload> UpdatePriceUnitRate(
            UpdatePriceUnitRateInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] PriceUnitRateRepository  priceUnitRateRepository
         //, [Service] IPriceUnitRateEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdatePriceUnitRateInput>(data, userId);
            var val = await priceUnitRateRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeletePriceUnitRate(PriceUnitRate data
        , [Service] PriceUnitRateRepository  priceUnitRateRepository
            // , [Service] IPriceUnitRateEventSender eventSender
            )
        {
           await priceUnitRateRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
