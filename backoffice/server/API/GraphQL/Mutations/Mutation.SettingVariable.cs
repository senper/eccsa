
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
        public async Task<SettingVariablePayload> CreateSettingVariable(
                 CreateSettingVariableInput data
               , [Service] IHttpContextAccessor contextAccessor
               , [Service] SettingVariableRepository  settingVariableRepository
             // , [Service] ISettingVariableEventSender eventSender 
       )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<CreateSettingVariableInput>(data, userId);
            var val = await settingVariableRepository.Create(data2);
            // await eventSender.SendAsync(data.TypeId, val);
            return val;
        }

        public async Task<SettingVariablePayload> UpdateSettingVariable(
            UpdateSettingVariableInput data
            , [Service] IHttpContextAccessor contextAccessor
            , [Service] SettingVariableRepository  settingVariableRepository
         //, [Service] ISettingVariableEventSender eventSender
        )
        {
            var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var data2 = new GenericInput<UpdateSettingVariableInput>(data, userId);
            var val = await settingVariableRepository.Update(data2);
            // await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            return val;

        }

        public async Task DeleteSettingVariable(SettingVariable data
        , [Service] SettingVariableRepository  settingVariableRepository
            // , [Service] ISettingVariableEventSender eventSender
            )
        {
           await settingVariableRepository.Delete(data );
            //await eventSender.SendAsync(data.ExchangeRateTypeId, val);
            //return val;
        }
    }
    
}
