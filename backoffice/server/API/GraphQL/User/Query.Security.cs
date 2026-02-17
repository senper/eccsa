using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ECCSA.BackOffice.Server.Model.Security;

namespace ECCSA.BackOffice.Server.API.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<UserProfile> GetUserProfile([Service] UserManager<User> userManager,
            [Service] IHttpContextAccessor contextAccessor)
        {
            ClaimsPrincipal claimsPrincipal = contextAccessor.HttpContext.User;
            try
            {
                var user = await userManager.FindByNameAsync(claimsPrincipal.Identity.Name);

                var userRoles = await userManager.GetRolesAsync(user);
                return new UserProfile
                {
                    Id = user.Id,
                    Email = user.Email,
                    Email_verified = false,
                    AssociationId = user.AssociationId,
                    Name = user.FullName,
                    AssignedModules = (int)user.AssignedModules,
                    DefaultModule = (int)user.DefaultModule,
                    Roles = userRoles.ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
