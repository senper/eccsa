
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<LegalDocumentPayload> CreateLegalDocument(
                 CreateLegalDocumentInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] LegalDocumentRepository  legalDocumentRepository
             // , [Service] ILegalDocumentEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateLegalDocumentInput>(data, userId);
            var val = await legalDocumentRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<LegalDocumentPayload> UpdateLegalDocument(
            UpdateLegalDocumentInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] LegalDocumentRepository  legalDocumentRepository
         //, [Service] ILegalDocumentEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateLegalDocumentInput>(data, userId);
            var val = await legalDocumentRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteLegalDocument(LegalDocument data
        , [Service] LegalDocumentRepository  legalDocumentRepository
            // , [Service] ILegalDocumentEventSender eventSender
            )
        {
           await legalDocumentRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
