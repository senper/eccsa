using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing
{
    public class ExportAgreementRepository
    {
        private readonly EccsaContext _context;
        public ExportAgreementRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
        }
        public List<ExportAgreement> UserExportAgreements(Guid userId, int status)
        {
            return _context.ExportAgreements.Where(r => r.UserId == userId &&
            (status == -1 || (int)r.Status == status)).ToList();
        }

        public List<LookupValue> UserExportAgreementLookup(Guid userId, int status)
        {
            return _context.AgreementCommodities.Where(r => r.Agreement.UserId == userId &&
            (status == -1 || (int)r.Agreement.Status == status)).OrderBy(s => s.Agreement.AgreementNo)
            .ThenBy(s => s.HSCode)
                .Select(s => new LookupValue
                {
                    Id = s.Id,
                    Name = $"Agreement: {s.Agreement.AgreementNo} - Commodity: {s.HSCode} {s.Name}",
                    Status = s.Agreement.Status
                }).ToList();
        }
        public async Task<ExportAgreement> UserExportAgreement(Guid userId, Guid id)
        {
            var request = await _context.ExportAgreements.Include(e => e.Commodities)
                    .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            return request;
        }

        public ExportAgreement UserExportAgreementByCommodityId(Guid userId, Guid id)
        {
            var request = _context.ExportAgreements.Include(e => e.Commodities)
                    .FirstOrDefault(i => i.Commodities.Any(c => c.Id == id) && i.UserId == userId);

            return request;
        }

        public async Task Save(Guid userId, ExportAgreement data)
        {
            var tmp = await _context.ExportAgreements.FirstOrDefaultAsync(x => x.Id == data.Id);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            data.UserId = userId;
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.ExportAgreements.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveCommodityList(Guid userId, Guid id, List<AgreementCommodity> data)
        {
            var tmp = await _context.ExportAgreements.Include(c => c.Commodities).FirstOrDefaultAsync(x => x.Id == id);
            if(tmp == null)
            {
                throw new InvalidOperationException("Invalid Export Agreement");
            }
            _context.AgreementCommodities.RemoveRange(tmp.Commodities);
            data.ForEach(d => d.AgreementId = id);
            _context.AgreementCommodities.AddRange(data);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Guid userId, Guid id)
        {
            var tmp = await _context.ExportAgreements.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == id);
            if (tmp == null) return;
            _context.ExportAgreements.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}
