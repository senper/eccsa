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
        
        public IQueryable<BoardMember> BoardMembers(
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] BoardMemberRepository  boardMemberRepository
        )
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return boardMemberRepository.BoardMembers(userId);
        }
    
        public async Task<BoardMember> BoardMemberById(Guid id,
                [Service] IHttpContextAccessor contextAccessor
               ,[Service] BoardMemberRepository  boardMemberRepository)
        {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return await boardMemberRepository.BoardMemberById(id, userId);
        }
    }
}
