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
    public class CampaignPartnerAgencyRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public CampaignPartnerAgencyRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<CampaignPartnerAgency> CampaignPartnerAgencies(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.CampaignPartnerAgencies.Include(x => x.Campaign)
.Include(x => x.MediaAgency)
;
        }

        public async Task<CampaignPartnerAgency> CampaignPartnerAgencyById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.CampaignPartnerAgencies.Include(x => x.Campaign)
.Include(x => x.MediaAgency)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<CampaignPartnerAgencyPayload> Create(GenericInput<CreateCampaignPartnerAgencyInput> input)
        {
            CampaignPartnerAgency data = new()
            {
                			Id = Guid.NewGuid()
			, CampaignId = input.PayLoad.CampaignId
			, MediaAgencyId = input.PayLoad.MediaAgencyId
			, Status = input.PayLoad.Status
			, CommunicatedThrough = input.PayLoad.CommunicatedThrough
			, CreatedBy = input.UserId
			, CreateStamp = DateTime.UtcNow
			, UpdatedBy = input.UserId
			, UpdateStamp = DateTime.UtcNow

            };
            await _context.CampaignPartnerAgencies.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new CampaignPartnerAgencyPayload(data);
        }

        public async Task<CampaignPartnerAgencyPayload> Update(GenericInput<UpdateCampaignPartnerAgencyInput> data)
        {
            var tmp = await  _context.CampaignPartnerAgencies.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.CampaignId = data.PayLoad.CampaignId;
			tmp.MediaAgencyId = data.PayLoad.MediaAgencyId;
			tmp.Status = data.PayLoad.Status;
			tmp.CommunicatedThrough = data.PayLoad.CommunicatedThrough;
									tmp.UpdatedBy = data.UserId;
			tmp.UpdateStamp = DateTime.UtcNow;

            _context.CampaignPartnerAgencies.Update(tmp);
            await _context.SaveChangesAsync();
            return new CampaignPartnerAgencyPayload(tmp);

        }

        public async Task Delete(CampaignPartnerAgency data)
        {
            var tmp = await _context.CampaignPartnerAgencies.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.CampaignPartnerAgencies.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}