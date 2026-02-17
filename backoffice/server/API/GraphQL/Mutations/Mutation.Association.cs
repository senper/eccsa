
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
        public async Task<AssociationPayload> CreateAssociation(
                 CreateAssociationInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] AssociationRepository  associationRepository
             // , [Service] IAssociationEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateAssociationInput>(data, userId);
            var val = await associationRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<AssociationPayload> UpdateAssociation(
            UpdateAssociationInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] AssociationRepository  associationRepository
         //, [Service] IAssociationEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateAssociationInput>(data, userId);
            var val = await associationRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteAssociation(Association data
        , [Service] AssociationRepository  associationRepository
            // , [Service] IAssociationEventSender eventSender
            )
        {
           await associationRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
