using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Communication
{
    public class MediaAgencyRepository
    {
        private readonly EccsaContext _context;
        public MediaAgencyRepository(EccsaContext context)
        {
            _context = context;
        }

        public Task<List<MediaCoverage>> MediaCoverages()
        {
            return _context.MediaCoverages.ToListAsync();
        }

        public async Task<MediaCoverage> MediaCoverageById(Guid id)
        {
            return await _context.MediaCoverages.Include(x => x.MediaAgency)
                                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task Save(MediaCoverage data, Guid userId)
        {
            var tmp = await _context.MediaCoverages.FirstOrDefaultAsync(r => r.Id == data.Id);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            data.Remark = data.Remark ?? "";
            data.Frequency = data.Frequency ?? "";
            data.About = data.About ?? "";
            data.CoveredIn = data.CoveredIn ?? "";
            data.Extension = data.Extension ?? "";
            if (data.StartDate > DateTime.Today ||
                data.StartDate < DateTime.Today.AddYears(-100) ||
                data.EndDate > DateTime.Today ||
                data.EndDate < DateTime.Today.AddYears(-100) ||
                data.StartDate > data.EndDate)
            {
                throw new InvalidOperationException("Invalid Date Range! Start End dates must be defore today.");
            }
            if (tmp == null)
            {
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;

             
                _context.MediaCoverages.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
                _context.MediaCoverages.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCoverage(Guid id)
        {
            var tmp = await _context.MediaCoverages.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Media Coverage not found");
            _context.MediaCoverages.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task<List<MediaAgency>> MediaAgencies()
        {
            var lst = await _context.MediaAgencies.Include(x => x.MediaCoverages)
                            .Include(x => x.CampaignPartnerAgencies)
                            .Where(i => i.Status == Model.Common.StatusEnum.Active).ToListAsync();

            //foreach (var ma in lst)
            //    ma.MediaType2Bool();
            return lst;
        }
        public async Task<MediaAgency> MediaAgencyById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.MediaAgencies.Include(x => x.MediaCoverages)
                            .Include(x => x.CampaignPartnerAgencies)
                            .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task Save(MediaAgency data, Guid userId)
        {
            if (!Validation.IsEmail(data.Email) || !Validation.IsPhoneNumber(data.Mobile))
            {
                throw new InvalidOperationException("Invalid Email or Mobile No");
            }
            data.Telephone = string.IsNullOrEmpty(data.Telephone) ? string.Empty : data.Telephone;
            data.Url = string.IsNullOrEmpty(data.Url) ? string.Empty : data.Url;
            if(!string.IsNullOrEmpty(data.Url) && !Validation.IsWebsite(data.Url))
            {
                throw new InvalidOperationException("Invalid URL");
            }
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.UpdatedBy = userId;
                data.UpdateStamp = DateTime.UtcNow;
                _context.MediaAgencies.Add(data);
            }
            else
            {
                var tmp = await _context.MediaAgencies.FirstOrDefaultAsync(a => a.Id == data.Id);
                tmp.IsLocalAgency = data.IsLocalAgency;
                tmp.MediaType = data.MediaType;
                tmp.Mobile = data.Mobile;
                tmp.Name = data.Name;
                tmp.OwnershipType = data.OwnershipType;
                tmp.RepresentativeName = data.RepresentativeName;
                tmp.ResponsiblePersonnel = data.ResponsiblePersonnel;
                tmp.Email = data.Email;
                tmp.Mobile = data.Mobile;
                tmp.Telephone = data.Telephone;
                tmp.Url = data.Url;
                tmp.Description = data.Description;
                tmp.Remark = data.Remark;
                tmp.Status = data.Status;
                _context.MediaAgencies.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var tmp = await _context.MediaAgencies.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.MediaAgencies.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Campaign>> Campaigns()
        {
            return await _context.Campaigns.Include(x => x.CampaignPartnerAgencies).ToListAsync();
            
        }

        public async Task<Campaign> CampaignById(Guid id)
        {
            var campaign= await _context.Campaigns.Include(x => x.CampaignPartnerAgencies)
                .ThenInclude(x => x.MediaAgency)
                                    .FirstOrDefaultAsync(i => i.Id == id);
            return campaign;
        }
        public async Task Save(Campaign data, Guid userId)
        {
            var tmp = await _context.Campaigns.FirstOrDefaultAsync(a => a.Id == data.Id);
            if (data.StartDate > DateTime.Today.AddYears(100) || 
                data.StartDate < DateTime.Today.AddYears(-100) ||
                data.EndDate > DateTime.Today.AddYears(100) || 
                data.EndDate < DateTime.Today.AddYears(-100) ||
                data.StartDate > data.EndDate.AddDays(-2))
            {
                throw new InvalidOperationException("Invalid Date Range");
            }
            
            if (tmp == null)
            {
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.UpdatedBy = userId;
                data.UpdateStamp = DateTime.UtcNow;
                _context.Campaigns.Add(data);
            }
            else
            {
                
                tmp.Title = data.Title;
                tmp.Description = data.Description;
                tmp.StartDate = data.StartDate;
                tmp.EndDate = data.EndDate;
                tmp.Purpose = data.Purpose;
                tmp.Status = data.Status;
                tmp.Extension = string.IsNullOrEmpty( data.Extension)? tmp.Extension:data.Extension;
                tmp.UpdatedBy = userId;
                tmp.UpdateStamp = DateTime.UtcNow;
                _context.Campaigns.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCampaign(Guid id)
        {
            var tmp = await _context.Campaigns.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Campaigns not found");
            _context.Campaigns.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CampaignPartnerAgency>> CampaignPartnerAgencies()
        {
            return await _context.CampaignPartnerAgencies.Include(x => x.Campaign).ToListAsync();
            ;
        }

        public async Task<CampaignPartnerAgency> CampaignPartnerAgencyById(Guid id)
        {
            return await _context.CampaignPartnerAgencies.Include(x => x.Campaign)
                                .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task Save(CampaignPartnerAgency data, Guid userId)
        {
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.UpdatedBy = userId;
                data.UpdateStamp = DateTime.UtcNow;
                _context.CampaignPartnerAgencies.Add(data);
            }
            else
            {
                var tmp = await _context.CampaignPartnerAgencies.FirstOrDefaultAsync(a => a.Id == data.Id);
                if (tmp == null) throw new KeyNotFoundException("Campaign Partner Agencies not found");
                tmp.CampaignId = data.CampaignId;
                tmp.MediaAgencyId = data.MediaAgencyId;
                tmp.Status = data.Status;
                tmp.CommunicatedThrough = data.CommunicatedThrough;
                tmp.Status = data.Status;
                tmp.UpdatedBy = userId;
                tmp.UpdateStamp = DateTime.UtcNow;
                _context.CampaignPartnerAgencies.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCampaignPartnerAgency(Guid id)
        {
            var tmp = await _context.CampaignPartnerAgencies.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Campaign Partner Agencies not found");
            _context.CampaignPartnerAgencies.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeParticipants(Guid id, List<Guid> agencies)
        {
            var partnerAgencies = _context.CampaignPartnerAgencies.Where(u => u.CampaignId == id);
            _context.CampaignPartnerAgencies.RemoveRange(partnerAgencies);
            agencies.ForEach(r =>
            _context.CampaignPartnerAgencies.Add(new() { CampaignId = id, MediaAgencyId = r, Status = 1 }));
            await _context.SaveChangesAsync();
        }
    }
}