
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
        public async Task<SponsorPayload> CreateSponsor(
                 CreateSponsorInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SponsorRepository  sponsorRepository
             // , [Service] ISponsorEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSponsorInput>(data, userId);
            var val = await sponsorRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SponsorPayload> UpdateSponsor(
            UpdateSponsorInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SponsorRepository  sponsorRepository
         //, [Service] ISponsorEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSponsorInput>(data, userId);
            var val = await sponsorRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSponsor(Sponsor data
        , [Service] SponsorRepository  sponsorRepository
            // , [Service] ISponsorEventSender eventSender
            )
        {
           await sponsorRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
