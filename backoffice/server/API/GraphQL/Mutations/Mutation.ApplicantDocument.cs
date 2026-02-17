
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
        public async Task<ApplicantDocumentPayload> CreateApplicantDocument(
                 CreateApplicantDocumentInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ApplicantDocumentRepository  applicantDocumentRepository
             // , [Service] IApplicantDocumentEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateApplicantDocumentInput>(data, userId);
            var val = await applicantDocumentRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ApplicantDocumentPayload> UpdateApplicantDocument(
            UpdateApplicantDocumentInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ApplicantDocumentRepository  applicantDocumentRepository
         //, [Service] IApplicantDocumentEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateApplicantDocumentInput>(data, userId);
            var val = await applicantDocumentRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteApplicantDocument(ApplicantDocument data
        , [Service] ApplicantDocumentRepository  applicantDocumentRepository
            // , [Service] IApplicantDocumentEventSender eventSender
            )
        {
           await applicantDocumentRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
