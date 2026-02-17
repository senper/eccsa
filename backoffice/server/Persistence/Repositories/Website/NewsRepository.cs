using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Website
{
    public class NewsRepository : BaseRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;

        public NewsRepository(EccsaContext context, EccsaUserContext userContext) : base(context)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<List<News>> GetNews()
        {
            return await _context.News.OrderByDescending(d => d.NewsDate).ToListAsync();
        }

        //newsBYiD Isn't needed anymore
        public async Task<News> NewsById(Guid id)
        {
            var request = await _context.News
                    .FirstOrDefaultAsync(i => i.Id == id);
            return request;
        }

        public async Task<List<News>> TopFourNews()
        {
            return await _context.News.OrderByDescending(i => i.NewsDate).Take(4).ToListAsync();
        }
        public async Task Save(News data, Guid userId)
        {
            data.UpdateStamp = DateTime.UtcNow;
            data.UpdatedBy = userId;
            var tmp = await _context.News.FirstOrDefaultAsync(l => l.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                {
                    data.Id = Guid.NewGuid();
                }
                data.Extension = data.Extension ?? "";
                data.ExtensionHome = data.ExtensionHome ?? "";
                data.BriefLocal ??= "";
                data.Brief ??= "";
                data.Full ??= "";
                data.FullLocal ??= "";
                data.ExtensionHome = data.ExtensionHome ?? "";
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.News.Add(data);
            }
            else
            {
                data.Full = String.IsNullOrWhiteSpace(data.Full) ? tmp.Full : data.Full;
                data.Extension = String.IsNullOrWhiteSpace(data.Extension) ? tmp.Extension : data.Extension;
                data.ExtensionHome = String.IsNullOrWhiteSpace(data.ExtensionHome) ? tmp.ExtensionHome : data.ExtensionHome;
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await GenerateKeyword(ContentTypeEnum.News, data.Id, $"{data.Title} {data.Brief}", data.Title, data.Brief, data.Id);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Guid id)
        {
            var tmp = await NewsById(id);
            if (tmp == null) return;
            _context.News.Remove(tmp);
            await RemoveKeyword(id, id);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            var lst = await _context.Subscriptions.ToListAsync();
            await MapUser(lst);

            return lst;
        }
        public async Task<List<Subscription>> GetSubscriptions(Guid topicId)
        {
            var lst = await _context.Subscriptions.Where(s => s.TopicId == topicId && s.Status).ToListAsync();
            await MapUser(lst);

            return lst;
        }

        private async Task<List<Subscription>> MapUser(List<Subscription> src)
        {
            var users = await _userContext.Users.ToListAsync();

            foreach (var sub in src)
            {
                var user = users.FirstOrDefault(u => u.Id == sub.UserId);
                if (user == null) continue;
                sub.Email = user.Email;
                sub.CompanyName = user.CompanyName;
                sub.FullName = user.FullName;
                sub.UserName = user.UserName;
                sub.MobileNo = user.MobileNo;
                sub.IsStaff = user.AssociationId == Constants.ECCSA && user.AssignedModules > 0;
            }
            return src;
        }
        public async Task<List<LookupValue>> Topics()
        {
            var NewsLetterTypeId = Guid.Parse("00000000-0000-0000-0000-00000000002f");
            var lst = await _context.LookupValues.Where(l => l.LookupTypeId == NewsLetterTypeId).ToListAsync();
            return lst;
        }
        public async Task<List<NewsletterIssue>> GetNewsletterIssues(Guid topicId)
        {
            var list = await _context.Newsletters.Include(x => x.Dissiminations).Where(d => d.TopicId == topicId).ToListAsync();
            var subscribersCount = await _context.Subscriptions.CountAsync(s => s.TopicId == topicId && s.Status);
            list.ForEach(x => x.SubscribersCount = subscribersCount);
            return list;
        }

        public async Task<List<NewsletterIssue>> GetNewslettersToSend()
        {
            var lst = await _context.Newsletters.Include(x => x.Dissiminations).Where(d => d.DissiminationStatus == DissiminationStatusEnum.Sending).ToListAsync();

            var subscribers = await _context.Subscriptions.Where(s => s.Status).ToListAsync();
            subscribers = await MapUser(subscribers);
            foreach (var issue in lst)
            {
                issue.Subscribers = subscribers.Where(s => s.TopicId == issue.TopicId && s.Dissiminations.All(d => d.SubscriberId != s.Id)).ToList();
            }
            return lst.Where(s => s.Subscribers.Count > 0).ToList();
        }

        public async Task<NewsletterIssue> GetNewsletterIssue(Guid id)
        {
            var issue = await _context.Newsletters.Include(x => x.Dissiminations).FirstOrDefaultAsync(d => d.Id == id);
            var subscribers = await _context.Subscriptions.Where(s => s.TopicId == issue.TopicId && s.Status).ToListAsync();
            issue.Subscribers = await MapUser(subscribers);
            return issue;

        }

        public async Task Save(NewsletterIssue data, Guid userId)
        {
            data.UpdateStamp = DateTime.UtcNow;
            data.UpdatedBy = userId;
            var tmp = await _context.Newsletters.FirstOrDefaultAsync(l => l.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                {
                    data.Id = Guid.NewGuid();
                }
                data.Extension = data.Extension ?? "";
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.Newsletters.Add(data);
            }
            else
            {
                data.TopicId = tmp.TopicId;
                data.Extension = String.IsNullOrWhiteSpace(data.Extension) ? tmp.Extension : data.Extension;
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await GenerateKeyword(ContentTypeEnum.Newsletter, data.Id, $"{data.Title} {data.Brief}", data.Title, data.Brief, data.Id);
            await _context.SaveChangesAsync();
        }

        public async Task Send(Guid id, Guid userId)
        {
            var tmp = await _context.Newsletters.FirstOrDefaultAsync(l => l.Id == id);
            if (tmp == null)
            {
                throw new InvalidOperationException($"Newsletter Not found {id}");
            }
            tmp.UpdateStamp = DateTime.UtcNow;
            tmp.UpdatedBy = userId;
            tmp.DissiminationStatus = DissiminationStatusEnum.Sending;
            _context.Newsletters.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task Close(Guid id, Guid userId)
        {
            var tmp = await _context.Newsletters.FirstOrDefaultAsync(l => l.Id == id);
            if (tmp == null)
            {
                throw new InvalidOperationException($"Newsletter Not found {id}");
            }
            tmp.UpdateStamp = DateTime.UtcNow;
            tmp.UpdatedBy = userId;
            tmp.DissiminationStatus = DissiminationStatusEnum.Closed;
            _context.Newsletters.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(NewsletterIssue tmp)
        {
            if (tmp == null) return;
            _context.Newsletters.Remove(tmp);
            await RemoveKeyword(tmp.Id, tmp.Id);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSendStatus(Guid issueId)
        {
            var tmp = await GetNewsletterIssue(issueId);
            foreach (var sub in tmp.Subscribers)
            {
                if (tmp.Dissiminations.Any(x => x.SubscriberId == sub.Id)) continue;
                var diss = new SubscriptionDissimination()
                {
                    Id = Guid.NewGuid(),
                    NewsletterIssueId = issueId,
                    SubscriberId = sub.Id,
                    DisseminationDate = DateTime.UtcNow,
                };
                _context.SubscriptionDissiminations.Add(diss);
            }
            tmp.DissiminationStatus = DissiminationStatusEnum.Sending;
            _context.Newsletters.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Subscribe(Guid userId, Guid topicId)
        {
            var tmp = await _context.Subscriptions.FirstOrDefaultAsync(s => s.TopicId == topicId && s.UserId == userId);
            if (tmp == null)
            {
                tmp = new Subscription()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    TopicId = topicId,
                    SubscriptionDate = DateTime.Now,
                    Status = true,
                    CompanyName = "ECCSA",
                };
                _context.Subscriptions.Add(tmp);
            }
            else
            {
                tmp.UnsubscriptionDate = null;
                tmp.Status = true;
                _context.Subscriptions.Update(tmp);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UnSubscribe(Guid userId, Guid topicId)
        {
            var tmp = await _context.Subscriptions.FirstOrDefaultAsync(s => s.TopicId == topicId && s.UserId == userId);
            if (tmp == null)
                return;

            tmp.UnsubscriptionDate = DateTime.Now;
            tmp.Status = false;
            _context.Subscriptions.Update(tmp);


            await _context.SaveChangesAsync();
        }
    }
}


