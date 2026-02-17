using ECCSA.BackOffice.Server.Model.Investment;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Investment
{
    public class InvestmentRepository
    {
        private readonly EccsaContext _context;
        public InvestmentRepository(EccsaContext context)
        {
            _context = context;
        }



        public async Task<List<Business>> Businesses()
        {
            return await _context.Businesses.ToListAsync();

        }

        public async Task<List<Business>> Search(string term)
        {
            var terms = term.Split(" ");
            var searchKeys = new List<string>();
            foreach (var trm in terms)
            {
                if (BlackList.Contains(trm.ToLowerInvariant())) continue;
                searchKeys.Add(trm.ToLowerInvariant());
            }
            if (searchKeys.Count == 0) return await Businesses();
            var key = searchKeys[0];
            var tmp = await _context.Businesses.Where(
                x => x.Name.Contains(key) ||
                x.Sector.Contains(key) ||
                x.Profile.Contains(key) ||
                x.Country.Contains(key) ||
                x.Address.Contains(key)).ToListAsync();
            for (var i = 1; i < searchKeys.Count; i++)
            {
                key = searchKeys[i];
                tmp = tmp.Where(
                x => x.Name.Contains(key) ||
                x.Sector.Contains(key) ||
                x.Profile.Contains(key) ||
                x.Country.Contains(key) ||
                x.Address.Contains(key)).ToList();
            }
            return tmp;

        }
        private readonly List<string> BlackList = new List<string>()
        {
            ";", ",", ".", "%", "!", "@", "#", "$","^", "&", "*", "(", ")", "-", "_", "+","=","|", "/", "\\", "the"," "
        };
        public async Task<Business> Business(Guid id)
        {
            var request = await _context.Businesses
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }


        public async Task Save(Business data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            data.SMSCode =  string.Empty ;
            var tmp = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.EmailConfirmed = true;
                data.PhoneConfirmed = true;
                data.ConfirmationLink = string.Empty;
                data.SMSCode = string.Empty;
                data.LinkExpiryDate = DateTime.Today.AddDays(-1);
                _context.Businesses.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task PublicSave(Business data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            var tmp = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.Businesses.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ConfirmEmail(Guid id, Guid userId)
        {
            var tmp = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null || tmp.LinkExpiryDate < DateTime.Today)
            {
                throw new KeyNotFoundException("Business not found");
            }
            tmp.EmailConfirmed = true;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Businesses.Update(tmp);

            await _context.SaveChangesAsync();
        }

        public async Task ConfirmPhone(Guid id, string code, Guid userId)
        {
            var tmp = await _context.Businesses.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null || tmp.SMSCode.ToLowerInvariant() == code.ToLowerInvariant())
            {
                throw new KeyNotFoundException("Business not found");
            }
            tmp.PhoneConfirmed = true;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Businesses.Update(tmp);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBusiness(Guid id)
        {
            var tmp = await _context.Businesses.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Business not found");
            _context.Businesses.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task<RegionSector> Ethipia()
        {
            return await _context.RegionSectors.FirstOrDefaultAsync(r => r.IsRegion && r.Id == RegionSector.EthiopiaId);
        }
        public async Task<List<RegionSector>> Regions()
        {
            return await _context.RegionSectors.Where(r => r.IsRegion && r.Id != RegionSector.EthiopiaId).ToListAsync();
        }
        public async Task<List<RegionSector>> RegionSectors()
        {
            return await _context.RegionSectors.ToListAsync();
        }
        public async Task<List<RegionSector>> Sectors()
        {
            return await _context.RegionSectors.Where(r => !r.IsRegion && r.Id != RegionSector.EthiopiaId).ToListAsync();
        }
        public async Task<RegionSector> Ethiopia()
        {
            return await _context.RegionSectors.FirstOrDefaultAsync(a => a.Id == RegionSector.EthiopiaId);
        }
        public async Task<RegionSector> RegionSectorById(Guid id)
        {
            return await _context.RegionSectors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task SaveProfile(RegionSector data, Guid userId)
        {
            var tmp = await _context.RegionSectors.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null) return;
            tmp.Profile = data.Profile ?? "";
            _context.RegionSectors.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task SaveProfileLocal(RegionSector data, Guid userId)
        {
            var tmp = await _context.RegionSectors.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null) return;
            tmp.ProfileLocal = data.ProfileLocal ?? "";
            _context.RegionSectors.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task Save(RegionSector data, Guid userId)
        {
            var tmp = await _context.RegionSectors.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                data.Id = Guid.NewGuid();
                data.Profile = String.Empty;
                data.ProfileLocal = String.Empty;
                _context.RegionSectors.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Guid id, Guid userId)
        {
            if (id == RegionSector.EthiopiaId) return;
            var tmp = await _context.RegionSectors.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp != null)
            {
                _context.RegionSectors.Remove(tmp);
            }
            
            await _context.SaveChangesAsync();
        }
    }
}
