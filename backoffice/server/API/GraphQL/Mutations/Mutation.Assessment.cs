
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<AssessmentPayload> CreateAssessment(
                 CreateAssessmentInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] AssessmentRepository  assessmentRepository
             // , [Service] IAssessmentEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateAssessmentInput>(data, userId);
            var val = await assessmentRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<AssessmentPayload> UpdateAssessment(
            UpdateAssessmentInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] AssessmentRepository  assessmentRepository
         //, [Service] IAssessmentEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateAssessmentInput>(data, userId);
            var val = await assessmentRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteAssessment(Assessment data
        , [Service] AssessmentRepository  assessmentRepository
            // , [Service] IAssessmentEventSender eventSender
            )
        {
           await assessmentRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
