
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
        public async Task<ContactPayload> CreateContact(
                 CreateContactInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] ContactRepository  contactRepository
             // , [Service] IContactEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateContactInput>(data, userId);
            var val = await contactRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<ContactPayload> UpdateContact(
            UpdateContactInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] ContactRepository  contactRepository
         //, [Service] IContactEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateContactInput>(data, userId);
            var val = await contactRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteContact(Contact data
        , [Service] ContactRepository  contactRepository
            // , [Service] IContactEventSender eventSender
            )
        {
           await contactRepository.Delete(data.Id );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
