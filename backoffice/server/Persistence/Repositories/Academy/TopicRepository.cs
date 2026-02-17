using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Academy
{
    public class TopicRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public TopicRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Topic> Topics(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Topics.Include(x => x.Resources)
.Include(x => x.TopicSettings)
.Include(x => x.Trainings)
.Include(x => x.Category)
;
        }

        public async Task<Topic> TopicById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            //            return await _context.Topics.Include("Resources.ResourceDeliveries")
            //.Include("TopicSettings.SettingVariable")
            //.Include(x => x.Category)
            //.IncludeFilter(x => x.Trainings.Where(t => t.Status != TrainingStatus.Closed))
            //.FirstOrDefaultAsync(i => i.Id == id);

            var result = await _context.Topics.Include("Resources.ResourceDeliveries")
.Include("TopicSettings.SettingVariable")
.Include(x => x.Category)
.Include(x => x.Trainings)
.FirstOrDefaultAsync(i => i.Id == id);

            if (result != null)
            {
                result.Trainings = result.Trainings.Where(t => t.Status != TrainingStatus.Closed).ToList();
            }

            return result;
        }
        public async Task Save(Topic data, Guid userId)
        {
            var luStatus = data.IsPublic ? Model.Common.StatusEnum.Active : Model.Common.StatusEnum.Inactive;
            data.UpdatedBy = userId;
            data.NameLocal = String.IsNullOrEmpty(data.NameLocal) ? string.Empty : data.NameLocal;
            data.UpdateStamp = DateTime.UtcNow;
            var tmp = await _context.Topics.FirstOrDefaultAsync(t => t.Id == data.Id);
            if (tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.Topics.Add(data);

                _context.LookupValues.Add(new Model.Common.LookupValue()
                {
                    Id = data.Id,
                    Name = data.NameEng,
                    Status = luStatus,
                    Value = data.Id.ToString(),
                    Code = data.Code,
                    LookupTypeId = Constants.TrainingTopicLookupTypeId

                });
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
                var lv = await _context.LookupValues.FirstOrDefaultAsync(l => l.Id == tmp.Id);
                if (lv != null)
                {
                    lv.Name = tmp.NameEng;
                    lv.Code = tmp.Code;
                    lv.Status = luStatus;
                    _context.LookupValues.Update(lv);
                }
                else
                {
                    _context.LookupValues.Add(new Model.Common.LookupValue()
                    {
                        Id = tmp.Id,
                        Name = tmp.NameEng,
                        Status = luStatus,
                        LookupTypeId = Constants.TrainingTopicLookupTypeId,
                        Code = tmp.Code,
                        Value = tmp.Id.ToString(),

                    });
                }
            }
            await _context.SaveChangesAsync();
        }


        public async Task Delete(Topic data)
        {
            var tmp = await _context.Topics.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Topic not found");
            _context.Topics.Remove(tmp);
            var lv = await _context.LookupValues.FirstOrDefaultAsync(l => l.Id == data.Id);

            if (lv != null)
            {
                _context.LookupValues.Remove(lv);
            }
            await _context.SaveChangesAsync();
        }
    }
}