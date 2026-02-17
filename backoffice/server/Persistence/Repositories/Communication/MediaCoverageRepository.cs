using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Communication;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Communication
{
    public class MediaCoverageRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public MediaCoverageRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<List<MediaCoverage>> MediaCoverages()
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.MediaCoverages.Include(x => x.MediaAgency).ToListAsync();
;
        }

        public async Task<MediaCoverage> MediaCoverageById(Guid id)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.MediaCoverages.Include(x => x.MediaAgency)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<MediaCoveragePayload> Create(GenericInput<CreateMediaCoverageInput> input)
        {
            MediaCoverage data = new()
            {
                			Id = Guid.NewGuid()
			, MediaAgencyId = input.PayLoad.MediaAgencyId
			, StartDate = input.PayLoad.StartDate
			, EndDate = input.PayLoad.EndDate
			, DurationMinutes = input.PayLoad.DurationMinutes
			, About = input.PayLoad.About
			, Remark = input.PayLoad.Remark
			, CreatedBy = input.UserId
			, CreateStamp = DateTime.UtcNow
			, UpdatedBy = input.UserId
			, UpdateStamp = DateTime.UtcNow

            };
            await _context.MediaCoverages.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new MediaCoveragePayload(data);
        }

        public async Task<MediaCoveragePayload> Update(GenericInput<UpdateMediaCoverageInput> data)
        {
            var tmp = await  _context.MediaCoverages.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.MediaAgencyId = data.PayLoad.MediaAgencyId;
			tmp.StartDate = data.PayLoad.StartDate;
			tmp.EndDate = data.PayLoad.EndDate;
			tmp.DurationMinutes = data.PayLoad.DurationMinutes;
			tmp.About = data.PayLoad.About;
			tmp.Remark = data.PayLoad.Remark;
									tmp.UpdatedBy = data.UserId;
			tmp.UpdateStamp = DateTime.UtcNow;

            _context.MediaCoverages.Update(tmp);
            await _context.SaveChangesAsync();
            return new MediaCoveragePayload(tmp);

        }

        public async Task Delete(MediaCoverage data)
        {
            var tmp = await _context.MediaCoverages.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.MediaCoverages.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}