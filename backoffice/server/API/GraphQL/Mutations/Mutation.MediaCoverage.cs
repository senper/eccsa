
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.API.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<MediaCoveragePayload> CreateMediaCoverage(
                 CreateMediaCoverageInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] MediaCoverageRepository  mediaCoverageRepository
             // , [Service] IMediaCoverageEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateMediaCoverageInput>(data, userId);
            var val = await mediaCoverageRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<MediaCoveragePayload> UpdateMediaCoverage(
            UpdateMediaCoverageInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] MediaCoverageRepository  mediaCoverageRepository
         //, [Service] IMediaCoverageEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateMediaCoverageInput>(data, userId);
            var val = await mediaCoverageRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteMediaCoverage(MediaCoverage data
        , [Service] MediaCoverageRepository  mediaCoverageRepository
            // , [Service] IMediaCoverageEventSender eventSender
            )
        {
           await mediaCoverageRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
