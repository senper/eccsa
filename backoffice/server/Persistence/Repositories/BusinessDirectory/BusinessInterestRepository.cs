using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.BusinessDirectory
{
    public class BusinessInterestRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public BusinessInterestRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<List<BusinessInterestSource>> BusinessInterestSources()
        {
            return await _context.BusinessInterestSources
                .Include(s => s.Sectors).Include(s => s.Details).Include(s => s.Matches)
                .OrderBy(s => s.Name).ToListAsync();
        }
        public async Task<List<BusinessInterestSource>> BusinessInterestSources(Guid? sectorId)
        {
            if (sectorId == null)
            {
                return await _context.BusinessInterestSources.Where(s => !s.Sectors.Any())
                .Include(s => s.Sectors).Include(s => s.Details).Include(s => s.Matches)
                .OrderBy(s => s.Name).ToListAsync();
            }
            return await _context.BusinessInterestSources.Where(s =>
            s.Sectors.Any(t => t.Id == sectorId || t.BusinessSector.ParentId == sectorId))
                .Include(s => s.Sectors).Include(s => s.Details).Include(s => s.Matches)
                .OrderBy(s => s.Name).ToListAsync();
        }
        public async Task<List<BusinessInterestProduct>> BusinessInterestProducts()
        {
            return await _context.BusinessInterestProducts.ToListAsync();
        }
        public async Task<BusinessInterestSource> GetUserBusinessInterest(Guid userId)
        {
            return await _context.BusinessInterestSources
                .Include(s => s.Details).ThenInclude(s => s.ProductType)
                .Include(s => s.Sectors).ThenInclude(s => s.BusinessSector)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }
        public async Task SaveSource(BusinessInterestSource data)
        {
            var tmp = await _context.BusinessInterestSources.FirstOrDefaultAsync(l => l.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                {
                    data.Id = Guid.NewGuid();
                }
                _context.BusinessInterestSources.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveUserBusinessInterest(BusinessInterestSource data, Guid userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
            {
                throw new InvalidOperationException("Invalid user");
            }
            data.Name = user.CompanyName;
            data.Email = user.Email;
            data.Address = user.Address;
            data.Telephone = user.PhoneNumber;
            data.UserId = userId;
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
            }
            var sectors = new List<BusinessInterestSourceSector>();
            foreach(var sector in data.Sectors)
            {
                if(!sectors.Any(s => s.SectorId == sector.SectorId))
                {
                    sector.Id = Guid.NewGuid();
                    sectors.Add(sector);
                }
            }
            var details = new List<BusinessInterestSourceDetail>();
            foreach (var detail in data.Details)
            {
                if (!details.Any(s => s.ProductTypeId == detail.ProductTypeId && s.BusinessType == detail.BusinessType))
                {
                    detail.Id = Guid.NewGuid();
                    details.Add(detail);
                }
            }
            data.Sectors = null;
            data.Details = null;
            var tmp = await _context.BusinessInterestSources.FirstOrDefaultAsync(l => l.UserId == userId);
            if (tmp == null)
            {
                _context.BusinessInterestSources.Add(data);
            }
            else
            {
                data.Id = tmp.Id;
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            var oldList = await _context.BusinessInterestSourceSectors.Where(s => s.BusinessInterestSourceId == data.Id).ToListAsync();
            _context.BusinessInterestSourceSectors.RemoveRange(oldList);
            sectors.ForEach(s => s.BusinessInterestSourceId = data.Id);
            _context.BusinessInterestSourceSectors.AddRange(sectors);
            var oldDetailList = await _context.BusinessInterestSourceDetails.Where(s => s.BusinessInterestSourceId == data.Id).ToListAsync();
            _context.BusinessInterestSourceDetails.RemoveRange(oldDetailList);
            details.ForEach(s => s.BusinessInterestSourceId = data.Id);
            _context.BusinessInterestSourceDetails.AddRange(details);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveUserBusinessInterest(Guid userId)
        {
            var tmp = await _context.BusinessInterestSources.FirstOrDefaultAsync(l => l.UserId == userId);
            if (tmp == null) return;
            _context.BusinessInterestSources.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task SaveSourceProductType(BusinessInterestSourceDetail data)
        {
            if (_context.BusinessInterestSourceDetails.Any(d => d.Id != data.Id && d.BusinessInterestSourceId == data.BusinessInterestSourceId &&
             d.BusinessType == data.BusinessType && d.ProductTypeId == data.ProductTypeId))
            {
                throw new InvalidOperationException("Product type for the business type already added to the source.");
            }
            data.DescriptionNormalized = data.Description.ToLowerInvariant();
            var tmp = await _context.BusinessInterestSourceDetails.FirstOrDefaultAsync(l => l.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                {
                    data.Id = Guid.NewGuid();
                }
                _context.BusinessInterestSourceDetails.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddSourceSector(Guid id, Guid sectorId)
        {
            var tmp = await _context.BusinessInterestSourceSectors.FirstOrDefaultAsync(l => l.SectorId == sectorId && l.BusinessInterestSourceId == id);
            if (tmp == null)
            {
                _context.BusinessInterestSourceSectors.Add(new BusinessInterestSourceSector
                {
                    Id = Guid.NewGuid(),
                    BusinessInterestSourceId = id,
                    SectorId = sectorId
                });
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Sector already added to the source.");
            }
        }
        public async Task DeleteSource(Guid id)
        {
            var tmp = await _context.BusinessInterestSources.FirstOrDefaultAsync(l => l.Id == id);
            if (tmp == null) return;
            _context.BusinessInterestSources.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveSourceSector(Guid id)
        {
            var tmp = await _context.BusinessInterestSourceSectors.FirstOrDefaultAsync(l => l.Id == id);
            if (tmp == null) return;
            _context.BusinessInterestSourceSectors.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteSourceProductType(Guid id)
        {
            var tmp = await _context.BusinessInterestSourceDetails.FirstOrDefaultAsync(l => l.Id == id);
            if (tmp == null) return;
            _context.BusinessInterestSourceDetails.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task<BusinessInterestSource> BusinessInterestSource(Guid id)
        {
            return await _context.BusinessInterestSources.Include(s => s.Details)
                .ThenInclude(s => s.ProductType).Include(s => s.Sectors).ThenInclude(s => s.BusinessSector)
                .Include(s => s.Matches)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<List<BusinessInterestSource>> BusinessInterestSourcesForPublic(Guid sectorId)
        {
            return await _context.BusinessInterestSources
                .Where(b => b.Sectors.Any(s => s.SectorId == sectorId))
                .Select(s => new BusinessInterestSource
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Details = s.Details.Select(d => new BusinessInterestSourceDetail
                    {
                        ProductType = d.ProductTypeId != null ? new BusinessInterestProduct { Name = d.ProductType.Name } :
                        null,
                        BusinessType = d.BusinessType,
                        Description = d.Description,
                    }).ToList(),
                    Sectors = s.Sectors.Select(d => new BusinessInterestSourceSector
                    {
                        SectorId = d.SectorId,
                    }).ToList()
                })
                .OrderBy(s => s.Name).ToListAsync();
        }
        public async Task<List<BusinessInterestSource>> BusinessInterestSourcesForPublic(Guid bussinessTypeId, string key)
        {
            var lowerKey = key.ToLowerInvariant();
            return await _context.BusinessInterestSources
                .Where(b => b.Details.Any(s => s.BusinessType == bussinessTypeId &&
                 ((s.ProductType != null && s.ProductType.NameNormalized.Contains(lowerKey)) ||
                 s.DescriptionNormalized.Contains(lowerKey))))
                .Select(s => new BusinessInterestSource
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Details = s.Details.Select(d => new BusinessInterestSourceDetail
                    {
                        ProductType = d.ProductTypeId != null ? new BusinessInterestProduct { Name = d.ProductType.Name } :
                        null,
                        BusinessType = d.BusinessType,
                        Description = d.Description,
                    }).ToList(),
                    Sectors = s.Sectors.Select(d => new BusinessInterestSourceSector
                    {
                        SectorId = d.SectorId,
                    }).ToList()
                })
                .OrderBy(s => s.Name).ToListAsync();
        }
        public async Task<BusinessInterestSource> BusinessInterestSourceForPublic(Guid id)
        {
            return await _context.BusinessInterestSources.Include(s => s.Details)
                .ThenInclude(s => s.ProductType)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<List<CompanyCountBySector>> SourcesBySectorCount()
        {
            return await _context.BusinessInterestSourceSectors.GroupBy(s => s.SectorId).Select(c => new CompanyCountBySector
            {
                SectorId = c.Key,
                Count = c.Count()
            }).ToListAsync();
        }
        public async Task<List<BusinessSector>> SectorsWithSources(bool parentsOnly = false)
        {
            var list = await _context.BusinessSectors.
                Where(s => (_context.BusinessInterestSourceSectors.Any(c => c.SectorId == s.Id) ||
                _context.BusinessInterestSourceSectors.Any(c => s.SubSectors.Any(ss => ss.Id == c.SectorId)))
                && (!parentsOnly || s.ParentId == null)
                ).ToListAsync();
            list.ForEach(l =>
            {
                l.SubSectors = null;
                l.ParentName = list.FirstOrDefault(p => p.Id == l.ParentId)?.Name;
            });
            return list;
        }
        public async Task<List<BusinessSector>> RootSectorsWithSources()
        {
            var list = await _context.BusinessSectors.
                Where(s => (_context.BusinessInterestSourceSectors.Any(c => c.SectorId == s.Id) ||
                _context.BusinessInterestSourceSectors.Any(c => s.SubSectors.Any(ss => ss.Id == c.SectorId)))
                && s.ParentId == null
                ).ToListAsync();
            return list;
        }
        public async Task SaveInterest(Guid id, Guid userId)
        {
            var source = await _context.BusinessInterestSources.FirstOrDefaultAsync(s => s.Id == id);
            if (source == null)
            {
                throw new KeyNotFoundException("Source Not found");
            }
            if (!_context.MatchMaking.Any(s => s.SourceId == id && s.UserId == userId))
            {
                _context.MatchMaking.Add(new MatchMaking
                {
                    Id = Guid.NewGuid(),
                    SourceId = id,
                    UserId = userId,
                    RequestDate = DateTime.UtcNow,
                    Remark = "",
                    Status = Model.Common.StatusEnum.Active
                });

            }
            else
            {
                var mm = await _context.MatchMaking.FirstOrDefaultAsync(s => s.SourceId == id && s.UserId == userId);
                mm.Status = Model.Common.StatusEnum.Active;
                _context.MatchMaking.Update(mm);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<List<MatchMaking>> UserMatches(Guid userid)
        {
            return await _context.MatchMaking.Include(s => s.Source).Where(m => m.UserId == userid
            && m.Status == Model.Common.StatusEnum.Active).ToListAsync();
        }
        public async Task RemoveInterest(Guid id, Guid userId)
        {
            var match = await _context.MatchMaking.FirstOrDefaultAsync(s => s.SourceId == id && s.UserId == userId
            && s.Status == Model.Common.StatusEnum.Active);
            if (match == null)
            {
                throw new KeyNotFoundException("Match Not found");
            }
            match.Status = Model.Common.StatusEnum.Inactive;
            _context.MatchMaking.Update(match);
            await _context.SaveChangesAsync();
        }
        public async Task SetUserInterestRemark(Guid id, Guid userId, string remark)
        {
            var match = await _context.MatchMaking.FirstOrDefaultAsync(s => s.SourceId == id && s.UserId == userId
            && s.Status == Model.Common.StatusEnum.Active);
            if (match == null)
            {
                throw new KeyNotFoundException("Match Not found");
            }
            match.Remark = remark;
            _context.MatchMaking.Update(match);
            await _context.SaveChangesAsync();
        }
        public async Task<List<BusinessInterestSource>> UserMatchSources(Guid userid)
        {
            return await _context.BusinessInterestSources.Where(s =>
            s.Matches.Any(m => m.UserId == userid && m.Status == Model.Common.StatusEnum.Active)).
            Select(s => new BusinessInterestSource
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Telephone = s.Telephone,
                Address = s.Address,
                Email = s.Email,
                Remark = s.Matches.FirstOrDefault(m => m.UserId == userid && m.Status == Model.Common.StatusEnum.Active).Remark,
                Details = s.Details.Select(d => new BusinessInterestSourceDetail
                {
                    ProductType = d.ProductTypeId != null ? new BusinessInterestProduct { Name = d.ProductType.Name } :
                    null,
                    BusinessType = d.BusinessType,
                    Description = d.Description,
                }).ToList(),
                Sectors = s.Sectors.Select(d => new BusinessInterestSourceSector
                {
                    SectorId = d.SectorId,
                }).ToList()
            }).ToListAsync();
        }
        public async Task SaveBusinessInterestProduct(BusinessInterestProduct data)
        {
            data.NameNormalized = data.Name.ToLowerInvariant();
            var tmp = await _context.BusinessInterestProducts.FirstOrDefaultAsync(l => l.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                {
                    data.Id = Guid.NewGuid();
                }
                _context.BusinessInterestProducts.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBusinessInterestProduct(Guid id)
        {
            var tmp = await _context.BusinessInterestProducts.FirstOrDefaultAsync(l => l.Id == id);
            if (tmp == null) return;
            _context.BusinessInterestProducts.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}
