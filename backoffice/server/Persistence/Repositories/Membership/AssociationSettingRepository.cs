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
    public class AssociationSettingRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public AssociationSettingRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<AssociationSetting> AssociationSettings(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.AssociationSettings.Include(x => x.Association)
;
        }

        public async Task<AssociationSetting> AssociationSettingById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.AssociationSettings.Include(x => x.Association)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<AssociationSettingPayload> Create(GenericInput<CreateAssociationSettingInput> input)
        {
            AssociationSetting data = new()
            {
                Id = Guid.NewGuid()
            ,
                AssociationId = input.PayLoad.AssociationId
            ,
                ProposedBankName = input.PayLoad.BankName
            ,
                ProposedAccountNumber = input.PayLoad.AccountNumber
            ,
                ProposedBy = input.UserId
            ,
                DateProposed = DateTime.UtcNow
            ,
                SubSystem = input.PayLoad.SubSystem
            };

            await _context.AssociationSettings.AddAsync(data);
            var val = await _context.SaveChangesAsync();
            return new AssociationSettingPayload(data);
        }

        public async Task<AssociationSettingPayload> Update(GenericInput<UpdateAssociationSettingInput> data)
        {
            var tmp = await _context.AssociationSettings.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Setting not found");
            tmp.ProposedBankName = data.PayLoad.BankName;
            tmp.ProposedAccountNumber = data.PayLoad.AccountNumber;
            tmp.ProposedBy = data.UserId;
            tmp.DateProposed = DateTime.UtcNow;
            _context.AssociationSettings.Update(tmp);
            await _context.SaveChangesAsync();
            return new AssociationSettingPayload(tmp);

        }

        public async Task Delete(AssociationSetting data, Guid userId)
        {
            var tmp = await _context.AssociationSettings.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Setting not found");
            //_context.AssociationSettings.Remove(tmp);
            tmp.ProposedBankName = "";
            tmp.ProposedAccountNumber = "";
            tmp.ProposedBy = userId;
            tmp.DateProposed = DateTime.UtcNow;
            _context.AssociationSettings.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AssociationSetting>> AssociationSettingsNeedsApproval(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.AssociationSettings
                .Where(s => (s.BankName != s.ProposedBankName || s.AccountNumber != s.ProposedAccountNumber) && s.DateProposed.HasValue 
                && s.ProposedBy.HasValue)
                .Include(x => x.Association).ToListAsync()
;
        }

        public async Task ApproveSetting(Guid id, Guid userId)
        {
            var tmp = await _context.AssociationSettings.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Setting not found");
            tmp.UpdatedBy = userId;
            tmp.UpdatedDate = DateTime.UtcNow;
            if (string.IsNullOrWhiteSpace(tmp.ProposedBankName) && string.IsNullOrWhiteSpace(tmp.ProposedAccountNumber))
            {
                _context.AssociationSettings.Remove(tmp);
            }
            else
            {
                tmp.BankName = tmp.ProposedBankName;
                tmp.AccountNumber = tmp.ProposedAccountNumber;
                _context.AssociationSettings.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
    }
}