using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Communication;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Communication
{
    public class CampaignRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public CampaignRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Campaign> Campaigns(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Campaigns.Include(x => x.CampaignPartnerAgencies)
;
        }

        public async Task<Campaign> CampaignById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Campaigns.Include(x => x.CampaignPartnerAgencies)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<CampaignPayload> Create(GenericInput<CreateCampaignInput> input)
        {
            Campaign data = new()
            {
                			Id = Guid.NewGuid()
			, Title = input.PayLoad.Title
			, Description = input.PayLoad.Description
			, StartDate = input.PayLoad.StartDate
			, EndDate = input.PayLoad.EndDate
			, Purpose = input.PayLoad.Purpose
			, Status = input.PayLoad.Status
			, CreatedBy = input.UserId
			, CreateStamp = DateTime.UtcNow
			, UpdatedBy = input.UserId
			, UpdateStamp = DateTime.UtcNow

            };
            await _context.Campaigns.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new CampaignPayload(data);
        }

        public async Task<CampaignPayload> Update(GenericInput<UpdateCampaignInput> data)
        {
            var tmp = await  _context.Campaigns.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.Title = data.PayLoad.Title;
			tmp.Description = data.PayLoad.Description;
			tmp.StartDate = data.PayLoad.StartDate;
			tmp.EndDate = data.PayLoad.EndDate;
			tmp.Purpose = data.PayLoad.Purpose;
			tmp.Status = data.PayLoad.Status;
									tmp.UpdatedBy = data.UserId;
			tmp.UpdateStamp = DateTime.UtcNow;

            _context.Campaigns.Update(tmp);
            await _context.SaveChangesAsync();
            return new CampaignPayload(tmp);

        }

        public async Task Delete(Campaign data)
        {
            var tmp = await _context.Campaigns.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Campaigns.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}