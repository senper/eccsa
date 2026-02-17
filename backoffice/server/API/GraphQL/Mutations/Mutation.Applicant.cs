
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
        public async Task<ApplicantPayload> CreateApplicant(
                 CreateApplicantInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ApplicantRepository  applicantRepository
             // , [Service] IApplicantEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateApplicantInput>(data, userId);
            var val = await applicantRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ApplicantPayload> UpdateApplicant(
            UpdateApplicantInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ApplicantRepository  applicantRepository
         //, [Service] IApplicantEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateApplicantInput>(data, userId);
            var val = await applicantRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteApplicant(Applicant data
        , [Service] ApplicantRepository  applicantRepository
            // , [Service] IApplicantEventSender eventSender
            )
        {
           await applicantRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
