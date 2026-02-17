using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
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
        
        public IQueryable<AssociationSetting> AssociationSettings(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AssociationSettingRepository  associationSettingRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return associationSettingRepository.AssociationSettings(userId);
        }
    
        public async Task<AssociationSetting> AssociationSettingById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] AssociationSettingRepository  associationSettingRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await associationSettingRepository.AssociationSettingById(id, userId);
        }
    }
}
