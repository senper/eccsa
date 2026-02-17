using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using System.Threading.Tasks;
using System.Linq;
using System;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECCSA.BackOffice.Server.API.GraphQL.Queries
{
    public partial class Query
    {
        
        public IQueryable<SettingVariable> SettingVariables(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SettingVariableRepository  settingVariableRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return settingVariableRepository.SettingVariables(userId);
        }
    
        public async Task<SettingVariable> SettingVariableById(int id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] SettingVariableRepository  settingVariableRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await settingVariableRepository.SettingVariableById(id, userId);
        }
    }
}
