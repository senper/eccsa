
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
        public async Task<MediaAgencyPayload> CreateMediaAgency(
                 CreateMediaAgencyInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] MediaAgencyRepository  mediaAgencyRepository
             // , [Service] IMediaAgencyEventSender eventSender 
       )
        {
            
            throw new NotImplementedException();
        }

        public async Task<MediaAgencyPayload> UpdateMediaAgency(
            UpdateMediaAgencyInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] MediaAgencyRepository  mediaAgencyRepository
         //, [Service] IMediaAgencyEventSender eventSender
        )
        {
            throw new NotImplementedException();

        }

        public async Task DeleteMediaAgency(MediaAgency data
        , [Service] MediaAgencyRepository  mediaAgencyRepository
            // , [Service] IMediaAgencyEventSender eventSender
            )
        {
            throw new NotImplementedException();
        }
    }
    
}
