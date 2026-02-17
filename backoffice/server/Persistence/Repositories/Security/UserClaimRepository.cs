using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Security;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Security
{
    public class UserClaimRepository 
    {
        private readonly EccsaUserContext _context;
        public UserClaimRepository(EccsaUserContext context)
        {
            _context = context;
        }

        public IQueryable<UserClaim> UserClaims()
        {

            return _context.UserClaims.Include(x => x.User)
;
        }

        public async Task<UserClaim> UserClaimById(Guid id)
        {
            return await _context.UserClaims.Include(x => x.User)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<UserClaimPayload> Create(CreateUserClaimInput input)
        {
            UserClaim data = new()
            {
                			Id = Guid.NewGuid()
			, UserId = input.UserId
			, ClaimType = input.ClaimType
			, ClaimValue = input.ClaimValue

            };
            await _context.UserClaims.AddAsync(data);
           await _context.SaveChangesAsync();
           return new UserClaimPayload(data);
        }

        public async Task<UserClaimPayload> Update(UpdateUserClaimInput data)
        {
            var tmp = await  _context.UserClaims.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.UserId = data.UserId;
			tmp.ClaimType = data.ClaimType;
			tmp.ClaimValue = data.ClaimValue;

            _context.UserClaims.Update(tmp);
            await _context.SaveChangesAsync();
            return new UserClaimPayload(tmp);

        }

        public async void Delete(UserClaim data)
        {
            var tmp = await _context.UserClaims.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.UserClaims.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}