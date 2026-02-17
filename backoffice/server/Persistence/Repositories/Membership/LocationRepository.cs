using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class LocationRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public LocationRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Location> Locations(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Locations;//.Include(x => x.Associations)
//.Include(x => x.Individuals)
;
        }

        public async Task<Location> LocationById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Locations.Include(x => x.Associations)
.Include(x => x.Individuals)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<LocationPayload> Create(GenericInput<CreateLocationInput> input)
        {
            Location data = new()
            {
                			Id = Guid.NewGuid()
			, Code = input.PayLoad.Code
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, ParentId = input.PayLoad.ParentId
			, Level = input.PayLoad.Level
			, Level1Id = input.PayLoad.Level1Id
			, Level1NameEng = input.PayLoad.Level1NameEng
			, Level1NameLocal = input.PayLoad.Level1NameLocal
			, Level2Id = input.PayLoad.Level2Id
			, Level2NameEng = input.PayLoad.Level2NameEng
			, Level2NameLocal = input.PayLoad.Level2NameLocal

            };
            await _context.Locations.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new LocationPayload(data);
        }

        public async Task<LocationPayload> Update(GenericInput<UpdateLocationInput> data)
        {
            var tmp = await  _context.Locations.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.Code = data.PayLoad.Code;
			tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.ParentId = data.PayLoad.ParentId;
			tmp.Level = data.PayLoad.Level;
			tmp.Level1Id = data.PayLoad.Level1Id;
			tmp.Level1NameEng = data.PayLoad.Level1NameEng;
			tmp.Level1NameLocal = data.PayLoad.Level1NameLocal;
			tmp.Level2Id = data.PayLoad.Level2Id;
			tmp.Level2NameEng = data.PayLoad.Level2NameEng;
			tmp.Level2NameLocal = data.PayLoad.Level2NameLocal;

            _context.Locations.Update(tmp);
            await _context.SaveChangesAsync();
            return new LocationPayload(tmp);

        }

        public async Task Delete(Location data)
        {
            var tmp = await _context.Locations.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Locations.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}