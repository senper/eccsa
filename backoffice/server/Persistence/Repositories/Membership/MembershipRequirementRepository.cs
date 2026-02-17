using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class MembershipRequirementRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public MembershipRequirementRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<MembershipRequirement> MembershipRequirements(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.MembershipRequirements.Include(x => x.Association)
;
        }

        public async Task<MembershipRequirement> MembershipRequirementById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.MembershipRequirements.Include(x => x.Association)
.FirstOrDefaultAsync(i => i.Id == id);
        }
       
      

        public async Task Delete(MembershipRequirement data)
        {
            var tmp = await _context.MembershipRequirements.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.MembershipRequirements.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}