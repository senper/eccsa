using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.BusinessDirectory
{
    public class BusinessRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public BusinessRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
            
        }

        public async Task<List<BusinessSector>> BusinessSectors()
        {
            return await _context.BusinessSectors.Include(x=> x.ParentSector).Include(x => x.BusinessSectors).ToListAsync();
        }
        
        public async Task<List<BusinessSector>> RootSectors()
        {
            return await _context.BusinessSectors
                .Include(x => x.BusinessSectors)
                .Include(x => x.SubSectors)
                .Where(x => !x.ParentId.HasValue).ToListAsync();
        }
        public async Task<List<BusinessSector>> ChildSectors(Guid parentId)
        {
            return await _context.BusinessSectors
                .Include(x => x.BusinessSectors)
                .Include(x => x.SubSectors)
                .Where(x => x.ParentId.Value == parentId).ToListAsync();
        }
        public async Task<BusinessSector> SectorById(Guid id)
        {
            return await _context.BusinessSectors
                .Include(x => x.SubSectors)
                .Include(x => x.BusinessSectors)
                .ThenInclude(x => x.Company)
                .Include(x => x.TempCompanies)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<Company>> Companies()
        {
            return await _context.Companies.Include(x => x.BusinessSectors).ToListAsync();
        }
        public async Task Save(TempCompany data, Guid userId)
        {
            var tmp = await _context.TempCompanies.FirstOrDefaultAsync(x => x.Id == data.Id);

            if (string.IsNullOrEmpty(data.Telephone))
                data.Telephone = string.Empty;
            if (string.IsNullOrEmpty(data.Mobile))
                data.Mobile = string.Empty;
            if (string.IsNullOrEmpty(data.Fax))
                data.Fax = string.Empty;
            if (string.IsNullOrEmpty(data.Email))
                data.Email = string.Empty;
            if (string.IsNullOrEmpty(data.PoBox))
                data.PoBox = string.Empty;
            if (string.IsNullOrEmpty(data.Web))
                data.Web = string.Empty;
            if (string.IsNullOrEmpty(data.Extension))
                data.Extension = string.Empty;
            if (tmp == null)
            {
                data.Id = Guid.NewGuid();

                _context.TempCompanies.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task Save(Company data, Guid userId)
        {
            var tmp = await _context.Companies.FirstOrDefaultAsync(x => x.Id == data.Id);

            if (string.IsNullOrEmpty(data.Telephone))
                data.Telephone = string.Empty;
            if (string.IsNullOrEmpty(data.Mobile))
                data.Mobile = string.Empty;
            if (string.IsNullOrEmpty(data.Fax))
                data.Fax = string.Empty;
            if (string.IsNullOrEmpty(data.Email))
                data.Email = string.Empty;
            if (string.IsNullOrEmpty(data.PoBox))
                data.PoBox = string.Empty;
            if (string.IsNullOrEmpty(data.Web))
                data.Web = string.Empty;
            if (string.IsNullOrEmpty(data.Extension))
                data.Extension = string.Empty;
            if (tmp == null)
            {
                if(data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();

                _context.Companies.Add(data);
                if (data.BusinessSectors.Count > 0)
                {
                    var bsector = data.BusinessSectors.ToList()[0];
                    _context.BusinessSectorCompanies.Add(bsector);
                }
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Company> Company(Guid id)
        {
            return await _context.Companies
                .Include(c => c.BusinessSectors)
                .ThenInclude(c => c.BusinessSector)
                .ThenInclude(c => c.ParentSector)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Save(BusinessSector data, Guid userId)
        {
            var tmp = await _context.BusinessSectors.FirstOrDefaultAsync(x => x.Id == data.Id);

            if (data.ParentId.HasValue && data.ParentId.Value == Guid.Empty)
                data.ParentId = null;
            if (tmp == null)
            {
                data.Id = Guid.NewGuid();

                _context.BusinessSectors.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            var lv = await _context.LookupValues.FirstOrDefaultAsync(l => l.Id == data.Id);
            if (lv != null)
            {
                lv.Name = data.Name;
                _context.LookupValues.Update(lv);
            }
            else
            {
                _context.LookupValues.Add(new Model.Common.LookupValue()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Status = Model.Common.StatusEnum.Active,
                    LookupTypeId = Constants.SectorLookupTypeId,
                    Code = string.Empty,
                    ParentId = !data.ParentId.HasValue ? null: data.ParentId.Value,
                    Value = data.Id.ToString(),

                });
            }
            
            await _context.SaveChangesAsync();
        }


        public async Task Save(BusinessSectorCompany data)
        {
            var tmp = await _context.BusinessSectorCompanies.FirstOrDefaultAsync(x => x.SectorId == data.SectorId && x.CompanyId == data.CompanyId);
            var sector = await SectorById(data.SectorId);
            var company = await Company(data.CompanyId);
            if(sector == null || company == null)
            {
                throw new InvalidOperationException("Invalid Sector or Company");
            }
            if (tmp != null) return;

            _context.BusinessSectorCompanies.Add(data);

            await _context.SaveChangesAsync();
        }

        public async Task ClearTempList(Guid sectorId)
        {
            var lst = await _context.TempCompanies.Where(x => x.SectorId == sectorId).ToListAsync();
            _context.TempCompanies.RemoveRange(lst);
           await _context.SaveChangesAsync();
        }
        public async Task MoveTempList(Guid sectorId, Guid tempId, Guid newId)
        {
            var lst = await _context.TempCompanies.FirstOrDefaultAsync(x => x.Id == tempId);
            var tmp = await _context.BusinessSectorCompanies.FirstOrDefaultAsync(x => x.SectorId == sectorId && x.CompanyId == newId);
            var data = new BusinessSectorCompany()
            {
                SectorId = sectorId,
                CompanyId = newId,
                Company = new Company
                {
                    Id = newId,
                    Name = lst.Name,
                    Email = lst.Email,
                    Fax = lst.Fax,
                    Extension = lst.Extension,
                    Mobile = lst.Mobile,
                    PoBox = lst.PoBox,
                    Telephone = lst.Telephone,
                    Web = lst.Web
                }
            };
            if(tmp == null)
            {
                data.Id = Guid.NewGuid();
                _context.BusinessSectorCompanies.Add(data);
            }
            else
            {
                var campany = await _context.Companies.FirstOrDefaultAsync(x => x.Id == newId);
                data.Id = tmp.Id;

                _context.Entry(campany).CurrentValues.SetValues(data.Company);
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            _context.TempCompanies.RemoveRange(lst);
            await _context.SaveChangesAsync();
        }
        public async Task<List<TempCompany>> GetTempList(Guid sectorId)
        {
            return await _context.TempCompanies.Where(x => x.SectorId == sectorId).ToListAsync();
            
        }


        public async Task Save(BusinessSectorCompany data, Guid userId)
        {
            var tmp = await _context.BusinessSectorCompanies.FirstOrDefaultAsync(x => x.Id == data.Id);


            if (tmp == null)
            {
                data.Id = Guid.NewGuid();

                _context.BusinessSectorCompanies.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            _context.BusinessSectorCompanies.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Company>> CompaniesBySector(Guid sectorId)
        {
            return await _context.Companies.Where(c => c.BusinessSectors.Any(s => s.SectorId == sectorId)).ToListAsync();
        }

        public async Task<List<CompanyCountBySector>> CompaniesBySectorCount()
        {
            return await _context.BusinessSectorCompanies.GroupBy(s => s.SectorId).Select(c => new CompanyCountBySector
            {
                SectorId = c.Key,
                Count = c.Count()
            }).ToListAsync();
        }
        public async Task<List<Company>> CompaniesByKey(string key)
        {
            return await _context.Companies.Include(s => s.BusinessSectors).
                Where(c => c.Name.ToLower().Contains(key.ToLower())).ToListAsync();
        }

        public async Task<List<BusinessSector>> SectorsWithCompany()
        {
            var list = await _context.BusinessSectors.
                Where(s => _context.BusinessSectorCompanies.Any(c => c.SectorId == s.Id) ||
                _context.BusinessSectorCompanies.Any(c => s.SubSectors.Any(ss => ss.Id == c.SectorId))
                ).ToListAsync();
            list.ForEach(l =>
            {
                l.SubSectors = null;
                l.ParentName = list.FirstOrDefault(p => p.Id == l.ParentId)?.Name;
            });
            return list;
        }

        public async Task DeleteCompanySector(Guid companyId, Guid sectorId, Guid userId)
        {
            var tmp = await _context.BusinessSectorCompanies.FirstOrDefaultAsync(x => x.SectorId == sectorId && x.CompanyId == companyId);
            _context.BusinessSectorCompanies.Remove(tmp);
            var cmp = await Company(companyId);
            if(cmp.BusinessSectors.Count == 1)
            {
                _context.Companies.Remove(cmp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSector(Guid sectorId, Guid userId)
        {
            var sector = await SectorById(sectorId);
            if(!sector.ParentId.HasValue ) {
                if (sector.SubSectors.Count > 0)
                    throw new InvalidOperationException("Clear Sub sectors");
            }else if (sector.BusinessSectors.Count > 0)
            {
                throw new InvalidOperationException("Clear Companies");
            }
            _context.TempCompanies.RemoveRange(sector.TempCompanies);
            _context.BusinessSectors.RemoveRange(sector);
            var lv = await _context.LookupValues.FirstOrDefaultAsync(l => l.Id == sectorId);
            _context.LookupValues.Remove(lv);
            await _context.SaveChangesAsync();


            
            

        }
    }
}