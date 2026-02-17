
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
        public async Task<AssociationSettingPayload> CreateAssociationSetting(
                 CreateAssociationSettingInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] AssociationSettingRepository  associationSettingRepository
             // , [Service] IAssociationSettingEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateAssociationSettingInput>(data, userId);
            var val = await associationSettingRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<AssociationSettingPayload> UpdateAssociationSetting(
            UpdateAssociationSettingInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] AssociationSettingRepository  associationSettingRepository
         //, [Service] IAssociationSettingEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateAssociationSettingInput>(data, userId);
            var val = await associationSettingRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteAssociationSetting(AssociationSetting data
        , [Service] AssociationSettingRepository  associationSettingRepository
            // , [Service] IAssociationSettingEventSender eventSender
            )
        {
           await associationSettingRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
