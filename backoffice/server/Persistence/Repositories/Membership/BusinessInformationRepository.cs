using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class BusinessInformationRepository
    {
        private readonly EccsaContext _context;
        public BusinessInformationRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
        }
        public async Task<List<BusinessInformation>> BusinessInformationList(DateTime endDateAfter)
        {
            return await _context.BusinessInformations.Where(s => s.EndDate >= endDateAfter)
                .OrderBy(s => s.Title).ToListAsync();
        }
        public async Task<List<BusinessInformation>> ActiveBusinessInformationList()
        {
            return await _context.BusinessInformations.Where(s => s.EndDate >= DateTime.Today).OrderBy(s => s.Title).ToListAsync();
        }
        public async Task<List<BusinessInfoDescription>> PastBusinessInformationList()
        {
            return await _context.BusinessInformations.Where(s => s.EndDate < DateTime.Today).OrderBy(s => s.Title)
                .Select(s => new BusinessInfoDescription(s.Id, s.Title, s.EndDate)).ToListAsync();
        }
        public async Task<BusinessInformation> BusinessInformation(Guid id)
        {
            var request = await _context.BusinessInformations
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }

        public async Task Save(Guid userId, BusinessInformation data, bool removeExtension = false)
        {
            var tmp = await _context.BusinessInformations.FirstOrDefaultAsync(x => x.Id == data.Id);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.BusinessInformations.Add(data);
            }
            else
            {
                data.Extension = string.IsNullOrEmpty(data.Extension) && !removeExtension ? tmp.Extension : data.Extension;
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Guid userId, Guid id)
        {
            var tmp = await _context.BusinessInformations.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) return;
            _context.BusinessInformations.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}
