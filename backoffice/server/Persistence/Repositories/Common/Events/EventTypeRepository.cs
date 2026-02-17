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
    public class EventTypeRepository
    {
        private readonly EccsaContext _context;
        public EventTypeRepository(EccsaContext context)
        {
            _context = context;
        }
       
       
        public async Task<List<EventType>> EventTypes()
        {
            return await _context.EventTypes
                .Where(x => x.Status == StatusEnum.Active).ToListAsync();
            ;
        }


        public async Task<List<EventType>> AllEventTypes()
        {
            return await _context.EventTypes.ToListAsync();
            ;
        }
        public async Task<EventType> EventTypeById(Guid id)
        {
            var eventType = await _context.EventTypes
                .Include(x => x.Events)
                .FirstOrDefaultAsync(i => i.EventTypeId == id);

        
            return eventType;
        }

       
      
        public async Task Save(EventType data, Guid userId)
        {
            if (data.EventTypeId == Guid.Empty)
            {
                data.EventTypeId = Guid.NewGuid();
                _context.EventTypes.Add(data);
            }
            else
            {
                var tmp = await _context.EventTypes.FirstOrDefaultAsync(x => x.EventTypeId == data.EventTypeId);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Event Type Not found");
                }
                tmp.Title = data.Title;
                tmp.Description = data.Description;
             
                tmp.Status = data.Status;
                _context.EventTypes.Update(tmp);
            }
            await _context.SaveChangesAsync();

        }
      
        public async Task DeleteEventType(Guid id)
        {
            if (id == Guid.Empty) return;
            var tmp = await _context.EventTypes
                         .Include(x => x.Events)
                         .FirstOrDefaultAsync(x => x.EventTypeId == id);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Event Type Not found");
            }
            if (tmp.Events.Count > 0 )
            {
                tmp.Status = StatusEnum.Inactive;
                _context.EventTypes.Update(tmp);
            }
            else
            {
                _context.EventTypes.Remove(tmp);
            }
            await _context.SaveChangesAsync();
        }
    }
}