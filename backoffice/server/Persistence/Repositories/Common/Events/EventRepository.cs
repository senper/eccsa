using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.EventManagement;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.Events
{
    public class EventRepository
    {
        private readonly EccsaContext _context;
        public EventRepository(EccsaContext context)
        {
            _context = context;
        }
        public async Task<List<Event>> PublicEvents()
        {
            return await _context.Events.Include(x => x.Details)
                .Where(x => x.Status == EventStatus.Published && x.EndDate > DateTime.Today)
                .OrderBy(s => s.StartDate).ToListAsync();
        }
        public async Task<List<EventDescription>> PublicPastEvents()
        {
            return (await _context.Events.Include(s => s.EventType)
                .Where(x => x.Status == EventStatus.Published && x.EndDate <= DateTime.Today)
                .OrderBy(s => s.Title)
                .ToListAsync())
                .Select(s => new EventDescription(s.Id, s.Title, s.EventType.Title)).ToList();
        }
        public async Task<List<Event>> Events(ECCSAModuleFlag subSystem)
        {
            return await _context.Events
               .Include(x => x.EventType)
               .Where(x => x.Status != EventStatus.Closed &&
                x.EventOwner == subSystem).ToListAsync();
        }
        public async Task<List<Event>> Events(ECCSAModuleFlag subSystem, EventStatus status)
        {
            return await _context.Events
                .Include(x => x.EventType)
                .Where(x => x.Status == status &&
                 x.EventOwner == subSystem).ToListAsync();
        }
        public async Task<Event> EventById(Guid id)
        {
            var request = await _context.Events
                    .Include(x => x.Details)
                    .Include(x => x.EventType)
                    .FirstOrDefaultAsync(i => i.Id == id);
            if (request == null) return null;
            foreach(var d in request.Details)
            {
                var st = await _context.ServiceTypes.FirstOrDefaultAsync(s => s.Id == d.DetailId);
                if(st == null) continue;
                d.RateBasis = st.RateBasis;
                d.MemberRate = st.MemberServiceCharge;
                d.OtherRate = st.NonMemberServiceCharge;
            }
            return request;
        }
       
        public async Task<EventDetail> DetailById(Guid id)
        {
            var request = await _context.EventDetails.Include(e => e.Event)
                    .FirstOrDefaultAsync(i => i.DetailId == id);
            return request;
        }
        public async Task Delete(Event data)
        {
            var tmp = await _context.Events.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Events.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task Save(Event data, Guid userId)
        {
            var tmp = await _context.Events.FirstOrDefaultAsync(x => x.Id == data.Id);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.Status = EventStatus.Draft;
                _context.Events.Add(data);
            }
            else
            {
                if (data.Status == EventStatus.NotSet)
                    data.Status = tmp.Status;
                _context.Entry(tmp).CurrentValues.SetValues(data);
                if(data.Status == EventStatus.Published)
                {
                    var details = await _context.EventDetails.Where(d => d.EventId == data.Id && d.ServiceTypeId.HasValue).ToListAsync();
                    foreach(var detail in details)
                    {
                        var tmpService = await _context.ServiceTypes.FirstOrDefaultAsync(s => s.Id == detail.ServiceTypeId.Value);
                        tmpService.Status = StatusEnum.Active;
                        _context.ServiceTypes.Update(tmpService);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
       
        public async Task Save(EventDetail data, Guid userId)
        {
            if (data.DetailId == Guid.Empty)
            {
                data.DetailId = Guid.NewGuid();
                _context.EventDetails.Add(data);
            }
            else
            {
                var tmp = await _context.EventDetails.FirstOrDefaultAsync(x => x.DetailId == data.DetailId);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Event Detail Not found");
                }
                tmp.Title = data.Title;
                tmp.StartDate = data.StartDate;
                tmp.EndDate = data.EndDate;
                if (tmp.ServiceTypeId.HasValue)
                {
                    var tmpServiceType = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == tmp.ServiceTypeId.Value);
                    if (tmpServiceType == null)
                    {
                        throw new KeyNotFoundException("Event Detail Service Type Not found");
                    }
                    tmpServiceType.StartDate = data.StartDate;
                    tmpServiceType.EndDate = data.EndDate;
                    _context.ServiceTypes.Update(tmpServiceType);
                }
                _context.EventDetails.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
    }

}