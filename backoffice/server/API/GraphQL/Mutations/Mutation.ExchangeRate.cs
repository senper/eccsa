
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
        public async Task<ExchangeRatePayload> CreateExchangeRate(
                 CreateExchangeRateInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ExchangeRateRepository  exchangeRateRepository
             // , [Service] IExchangeRateEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateExchangeRateInput>(data, userId);
            var val = await exchangeRateRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ExchangeRatePayload> UpdateExchangeRate(
            UpdateExchangeRateInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ExchangeRateRepository  exchangeRateRepository
         //, [Service] IExchangeRateEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateExchangeRateInput>(data, userId);
            var val = await exchangeRateRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteExchangeRate(ExchangeRate data
        , [Service] ExchangeRateRepository  exchangeRateRepository
            // , [Service] IExchangeRateEventSender eventSender
            )
        {
           await exchangeRateRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
