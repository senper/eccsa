using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class PositionTypeRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public PositionTypeRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<PositionType> PositionTypes(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.PositionTypes; //.Include(x => x.BoardMembers)
//.Include(x => x.Employees)
;
        }

        public async Task<PositionType> PositionTypeById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.PositionTypes.Include(x => x.BoardMembers)
.Include(x => x.Employees)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<PositionTypePayload> Create(GenericInput<CreatePositionTypeInput> input)
        {
            PositionType data = new()
            {
                			Id = Guid.NewGuid()
			, OrderNo = input.PayLoad.OrderNo
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, ForBoard = input.PayLoad.ForBoard
			, ForStaff = input.PayLoad.ForStaff
			, Status = input.PayLoad.Status

            };
            await _context.PositionTypes.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new PositionTypePayload(data);
        }

        public async Task<PositionTypePayload> Update(GenericInput<UpdatePositionTypeInput> data)
        {
            var tmp = await  _context.PositionTypes.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.OrderNo = data.PayLoad.OrderNo;
			tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.ForBoard = data.PayLoad.ForBoard;
			tmp.ForStaff = data.PayLoad.ForStaff;
			tmp.Status = data.PayLoad.Status;

            _context.PositionTypes.Update(tmp);
            await _context.SaveChangesAsync();
            return new PositionTypePayload(tmp);

        }

        public async Task Delete(PositionType data)
        {
            var tmp = await _context.PositionTypes.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.PositionTypes.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}