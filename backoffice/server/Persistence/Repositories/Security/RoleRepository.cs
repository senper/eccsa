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
    public class RoleRepository 
    {
        private readonly EccsaUserContext _context;
        public RoleRepository(EccsaUserContext context)
        {
            _context = context;
        }

        public IQueryable<Role> Roles()
        {

            return _context.Roles.Include(x => x.UserRoles)
;
        }

        public async Task<Role> RoleById(Guid id)
        {
            return await _context.Roles.Include(x => x.UserRoles)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<RolePayload> Create(CreateRoleInput input)
        {
            Role data = new()
            {
                			Id = Guid.NewGuid()
			, Name = input.Name

            };
            await _context.Roles.AddAsync(data);
           await _context.SaveChangesAsync();
           return new RolePayload(data);
        }

        public async Task<RolePayload> Update(UpdateRoleInput data)
        {
            var tmp = await  _context.Roles.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.Name = data.Name;

            _context.Roles.Update(tmp);
            await _context.SaveChangesAsync();
            return new RolePayload(tmp);

        }

        public async void Delete(Role data)
        {
            var tmp = await _context.Roles.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Roles.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}