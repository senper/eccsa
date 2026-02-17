using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Academy
{
    public class TopicSettingRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public TopicSettingRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<TopicSetting> TopicSettings(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.TopicSettings.Include(x => x.Topic)
.Include(x => x.SettingVariable)
;
        }

        public async Task<TopicSetting> TopicSettingById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.TopicSettings.Include(x => x.Topic)
.Include(x => x.SettingVariable)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<TopicSettingPayload> Create(GenericInput<CreateTopicSettingInput> input)
        {
            TopicSetting data = new()
            {
                			Id = Guid.NewGuid()
			, TopicId = input.PayLoad.TopicId
			, SettingVariableId = input.PayLoad.SettingVariableId
			, SettingValue = input.PayLoad.SettingValue

            };
            await _context.TopicSettings.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new TopicSettingPayload(data);
        }

        public async Task<TopicSettingPayload> Update(GenericInput<UpdateTopicSettingInput> data)
        {
            var tmp = await  _context.TopicSettings.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Topic Setting not found");
            						tmp.TopicId = data.PayLoad.TopicId;
			tmp.SettingVariableId = data.PayLoad.SettingVariableId;
			tmp.SettingValue = data.PayLoad.SettingValue;

            _context.TopicSettings.Update(tmp);
            await _context.SaveChangesAsync();
            return new TopicSettingPayload(tmp);

        }

        public async Task Delete(TopicSetting data)
        {
            var tmp = await _context.TopicSettings.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Topic Setting not found");
            _context.TopicSettings.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}