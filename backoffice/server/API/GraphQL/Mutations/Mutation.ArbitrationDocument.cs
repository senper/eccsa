
using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<ArbitrationDocumentPayload> CreateArbitrationDocument(
                 CreateArbitrationDocumentInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ArbitrationDocumentRepository  arbitrationDocumentRepository
             // , [Service] IArbitrationDocumentEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateArbitrationDocumentInput>(data, userId);
            var val = await arbitrationDocumentRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ArbitrationDocumentPayload> UpdateArbitrationDocument(
            UpdateArbitrationDocumentInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ArbitrationDocumentRepository  arbitrationDocumentRepository
         //, [Service] IArbitrationDocumentEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateArbitrationDocumentInput>(data, userId);
            var val = await arbitrationDocumentRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteArbitrationDocument(ArbitrationDocument data
        , [Service] ArbitrationDocumentRepository  arbitrationDocumentRepository
            // , [Service] IArbitrationDocumentEventSender eventSender
            )
        {
           await arbitrationDocumentRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
