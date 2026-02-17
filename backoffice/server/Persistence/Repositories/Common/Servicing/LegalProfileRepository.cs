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
    public class LegalProfileRepository
    {
        private readonly EccsaContext _context;
        public LegalProfileRepository(EccsaContext context)
        {
            _context = context;
        }
        public List<LegalProfile> UserLegalProfiles(Guid userId, int status)
        {
            return _context.LegalProfiles.Where(r => r.UserId == userId &&
            (status == -1 || 
            (status == 1 && r.Status == StatusEnum.Active && (r.ExpiryDate >= DateTime.Today || r.ExpiryDate == null)) ||
            (status == 0 && (r.Status == StatusEnum.Inactive || r.ExpiryDate < DateTime.Today)))).ToList();
            ;
        }
        public List<LookupValue> UserLegalProfileLookup(Guid userId, int status)
        {
            return _context.LegalProfiles.Where(r => r.UserId == userId &&
            (status == -1 ||
            (status == 1 && r.Status == StatusEnum.Active && (r.ExpiryDate >= DateTime.Today || r.ExpiryDate == null)) ||
            (status == 0 && (r.Status == StatusEnum.Inactive || r.ExpiryDate < DateTime.Today)))).OrderBy(s => s.Title)
                .Select(s => new LookupValue
                {
                    Id = s.Id,
                    Name = s.Title,
                    Status = s.Status
                }).ToList();
            ;
        }
        public async Task<LegalProfile> UserLegalProfile(Guid userId, Guid id)
        {
            var request = await _context.LegalProfiles
                    .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            return request;
        }

        public async Task<Guid> Save(Guid userId, LegalProfile data)
        {
            var tmp = await _context.LegalProfiles.FirstOrDefaultAsync(x => x.Id == data.Id);
            if ((tmp == null || !tmp.IsLocked) && (string.IsNullOrEmpty(data.Title) ||
                data.LegalDocumentType == LegalDocumentTypes.NotSet))
            {
                throw new InvalidOperationException($"Title, Type fields are required.");
            }
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            data.UserId = userId;
            var id = data.Id;
            if (data.Id == Guid.Empty || tmp == null)
            {

                if (string.IsNullOrEmpty(data.FileExtension))
                {
                    throw new InvalidOperationException($"Invalid attachment file.");
                }
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                var result = await _context.LegalProfiles.AddAsync(data);
                id = result.Entity.Id;
            }
            else
            {
                if (tmp.IsLocked)
                {
                    tmp.Status = data.Status;
                    _context.LegalProfiles.Update(tmp);
                }
                else
                {
                    data.IsLocked = tmp.IsLocked;
                    data.FileExtension = !string.IsNullOrEmpty(data.FileExtension) ? data.FileExtension : tmp.FileExtension;
                    _context.Entry(tmp).CurrentValues.SetValues(data);
                }
            }
            await _context.SaveChangesAsync();
            return id;
        }
        public async Task<Guid> UpdateStatus(Guid userId, Guid id, StatusEnum status)
        {
            var tmp = await _context.LegalProfiles.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null)
            {
                throw new InvalidOperationException($"Legal profile not found.");
            }
            tmp.Status = status;
            _context.LegalProfiles.Update(tmp);
            await _context.SaveChangesAsync();
            return id;
        }
        public async Task Delete(Guid userId, Guid id, string filePath)
        {
            var tmp = await _context.LegalProfiles.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == id && !r.IsLocked);
            if (tmp == null) return;
            _context.LegalProfiles.Remove(tmp);
            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(tmp.FileExtension))
            {
                System.IO.File.Delete(filePath + tmp.FileExtension);
            }
        }
    }
}
