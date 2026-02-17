
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
        public async Task<LegalDocumentTypePayload> CreateLegalDocumentType(
                 CreateLegalDocumentTypeInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] LegalDocumentTypeRepository  legalDocumentTypeRepository
             // , [Service] ILegalDocumentTypeEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateLegalDocumentTypeInput>(data, userId);
            var val = await legalDocumentTypeRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<LegalDocumentTypePayload> UpdateLegalDocumentType(
            UpdateLegalDocumentTypeInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] LegalDocumentTypeRepository  legalDocumentTypeRepository
         //, [Service] ILegalDocumentTypeEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateLegalDocumentTypeInput>(data, userId);
            var val = await legalDocumentTypeRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteLegalDocumentType(LegalDocumentType data
        , [Service] LegalDocumentTypeRepository  legalDocumentTypeRepository
            // , [Service] ILegalDocumentTypeEventSender eventSender
            )
        {
           await legalDocumentTypeRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
