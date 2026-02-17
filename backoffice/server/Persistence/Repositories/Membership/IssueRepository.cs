using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class IssueRepository
    {
        private readonly EccsaContext _context;
        public IssueRepository(EccsaContext context)
        {
            _context = context;
        }

        public async Task<List<IssueType>> IssueTypes(StatusEnum status = StatusEnum.Active)
        {
            return await _context.IssueTypes.Where(i => i.Status == status).OrderBy(i => i.NameEng).ThenBy(i => i.NameLocal).ToListAsync();
        }
        public async Task<List<IssueLocation>> IssueLocations(Guid locationTypeId, string countryRegion)
        {
            return await _context.IssueLocations.Where(i => i.TypeOfLocationId == locationTypeId
            && i.CountryRegion == countryRegion).
                OrderBy(i => i.Name).ToListAsync();
        }
        public async Task<List<IssueLocation>> IssueLocations(string countryRegion)
        {
            return await _context.IssueLocations.Where(i => i.CountryRegion == countryRegion).
                OrderBy(i => i.Name).ToListAsync();
        }
        public async Task<IssueType> IssueType(Guid id)
        {
            return await _context.IssueTypes.Include(i => i.Issues).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Issue>> Issues(
              ECCSAModuleFlag module = ECCSAModuleFlag.TradePromotion
            , IssueStatus status = IssueStatus.New
            , Guid? typeId = null)
        {
            if (typeId.HasValue)
                return await _context.Issues.Include(i => i.IssueType).Where(i => i.Status == status && i.TypeId == typeId.Value && (module == ECCSAModuleFlag.TradePromotion || i.ActionOwner == module)).ToListAsync();
            return await _context.Issues.Include(i => i.IssueType).Where(i => i.Status == status && (module == ECCSAModuleFlag.TradePromotion || i.ActionOwner == module)).ToListAsync();
        }

        public async Task<List<Issue>> Issues(DateTime issueDate,
              ECCSAModuleFlag module = ECCSAModuleFlag.TradePromotion
             )
        {
            return await _context.Issues.Include(i => i.IssueType).Where(i => i.IssueDate >= issueDate.Date && (module == ECCSAModuleFlag.TradePromotion || i.ActionOwner == module)).ToListAsync();
        }

        public async Task<List<IssueSummary>> IssuesSummary(DateTime issueDate)
        {
            return await _context.Issues.Include(i => i.IssueType).
                Where(i => i.IssueDate >= issueDate.Date).
                GroupBy(s => new { Type = s.IssueType.NameEng, Status = s.Status }).
                Select(i => new IssueSummary
                {
                    Type = i.Key.Type,
                    Status = i.Key.Status,
                    Count = i.Count()
                }).OrderBy(i => i.Type).ToListAsync();
        }

        public async Task<Issue> Issue(Guid id)
        {
            var issue = await _context.Issues.Include(i => i.IssueType).
                Include(l => l.Location).Include(s => s.Attachments).FirstOrDefaultAsync(i => i.Id == id);
            issue.PreferredHandler = _context.LookupValues.FirstOrDefault(l => l.Id == issue.PreferredHandlerId &&
            l.LookupTypeId == new Guid("00000000-0000-0000-0000-00000000002c"))?.Name;
            issue.LocationType = _context.LookupValues.FirstOrDefault(l => l.Id == issue.TypeOfLocationId &&
            l.LookupTypeId == new Guid("00000000-0000-0000-0000-00000000002d"))?.Name;
            return issue;
        }

        public async Task Save(IssueType issueType, Guid userId)
        {
            if (issueType.Id == Guid.Empty)
            {
                issueType.Id = Guid.NewGuid();
                _context.IssueTypes.Add(issueType);
            }
            else
            {
                var tmp = await _context.IssueTypes.FirstOrDefaultAsync(i => i.Id == issueType.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException($"Issue not found {issueType.Id}");
                }
                _context.Entry(tmp).CurrentValues.SetValues(issueType);
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveContactUs(ContactUs contactus)
        {
            if (contactus.Id == Guid.Empty)
            {
                contactus.Id = Guid.NewGuid();
                _context.ContactUs.Add(contactus);
            }
            else
            {
                var tmp = await _context.ContactUs.FirstOrDefaultAsync(i => i.Id == contactus.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException($"Not found {contactus.Id}");
                }
                _context.Entry(tmp).CurrentValues.SetValues(contactus);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Save(Issue issue, Guid userId)
        {
            if (issue.Id == Guid.Empty)
            {
                issue.Id = Guid.NewGuid();
                _context.Issues.Add(issue);
                foreach (var att in issue.Attachments)
                {
                    att.IssueId = issue.Id;
                    _context.IssueAttachments.Add(att);
                }
            }
            else
            {
                var tmp = await _context.Issues.FirstOrDefaultAsync(i => i.Id == issue.Id);
                if (tmp == null)
                {
                    _context.Issues.Add(issue);
                    foreach (var att in issue.Attachments)
                    {
                        att.IssueId = issue.Id;
                        _context.IssueAttachments.Add(att);
                    }
                }
                else
                {
                    _context.Entry(tmp).CurrentValues.SetValues(issue);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<ContactUs>> ContactUsList(DateTime date)
        {
            return await _context.ContactUs.OrderByDescending(d => d.Date).Where(i => i.Date >= date).ToListAsync();
        }

        public async Task<ContactUs> ContactUs(Guid id)
        {
            return await _context.ContactUs.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task DeleteContactUs(Guid id)
        {
            var tmp = await ContactUs(id);
            if (tmp == null) return;
            _context.ContactUs.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task CloseIssue(Guid id, String remark, Guid userId)
        {
            var tmp = await _context.Issues.FirstOrDefaultAsync(i => i.Id == id);
            if (tmp == null)
            {
                throw new KeyNotFoundException($"Issue not found {id}");
            }
            tmp.Status = IssueStatus.Resolved;
            tmp.DateActionTaken = DateTime.UtcNow;
            tmp.Remark = remark;
            _context.Issues.Update(tmp);

            await _context.SaveChangesAsync();
        }
        public async Task UpdateIssueRemark(Guid id, String remark, Guid userId)
        {
            var tmp = await _context.Issues.FirstOrDefaultAsync(i => i.Id == id);
            if (tmp == null)
            {
                throw new KeyNotFoundException($"Issue not found {id}");
            }
            if (tmp.Status == IssueStatus.Resolved) return;
            tmp.Remark = remark;
            _context.Issues.Update(tmp);

            await _context.SaveChangesAsync();
        }
        public async Task AssignIssue(Guid id, ECCSAModuleFlag owner, String remark, Guid userId)
        {
            var tmp = await _context.Issues.FirstOrDefaultAsync(i => i.Id == id);
            if (tmp == null)
            {
                throw new KeyNotFoundException($"Issue not found {id}");
            }
            tmp.Status = IssueStatus.Assigned;
            tmp.ActionOwner = owner;
            tmp.Remark = remark;
            tmp.DateAssigned = DateTime.UtcNow;
            _context.Issues.Update(tmp);

            await _context.SaveChangesAsync();
        }
    }
}