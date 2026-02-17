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
    public class SettingVariableRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public SettingVariableRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<SettingVariable> SettingVariables(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.SettingVariables.Include(x => x.TopicSettings)
;
        }

        public async Task<SettingVariable> SettingVariableById(int id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.SettingVariables.Include(x => x.TopicSettings)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<SettingVariablePayload> Create(GenericInput<CreateSettingVariableInput> input)
        {
            SettingVariable data = new()
            {
             Code = input.PayLoad.Code
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal ?? ""
			, DataType = input.PayLoad.DataType
			, MinValue = input.PayLoad.MinValue
			, MaxValue = input.PayLoad.MaxValue

            };
            var maxId = await _context.SettingVariables.MaxAsync(x => x.Id);
            data.Id = maxId + 1;
            await _context.SettingVariables.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new SettingVariablePayload(data);
        }

        public async Task<SettingVariablePayload> Update(GenericInput<UpdateSettingVariableInput> data)
        {
            var tmp = await  _context.SettingVariables.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Setting Variable not found");
            						tmp.Code = data.PayLoad.Code;
			tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal ?? "";
			tmp.DataType = data.PayLoad.DataType;
			tmp.MinValue = data.PayLoad.MinValue;
			tmp.MaxValue = data.PayLoad.MaxValue;

            _context.SettingVariables.Update(tmp);
            await _context.SaveChangesAsync();
            return new SettingVariablePayload(tmp);

        }

        public async Task Delete(SettingVariable data)
        {
            var tmp = await _context.SettingVariables.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Setting Variable not found");
            _context.SettingVariables.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}