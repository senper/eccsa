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
    public class BoardMemberRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public BoardMemberRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<BoardMember> BoardMembers(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.BoardMembers.Include(x => x.Association)
.Include(x => x.PositionType)
;
        }

        public async Task<BoardMember> BoardMemberById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.BoardMembers.Include(x => x.Association)
.Include(x => x.PositionType)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<BoardMemberPayload> Create(GenericInput<CreateBoardMemberInput> input)
        {
            BoardMember data = new()
            {
                			Id = Guid.NewGuid()
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, PositionTypeId = input.PayLoad.PositionTypeId
			, AssociationId = input.PayLoad.AssociationId
			, StartDate = input.PayLoad.StartDate
			, EndDate = input.PayLoad.EndDate
			, Status = input.PayLoad.Status

            };
            await _context.BoardMembers.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new BoardMemberPayload(data);
        }

        public async Task<BoardMemberPayload> Update(GenericInput<UpdateBoardMemberInput> data)
        {
            var tmp = await  _context.BoardMembers.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.PositionTypeId = data.PayLoad.PositionTypeId;
			tmp.AssociationId = data.PayLoad.AssociationId;
			tmp.StartDate = data.PayLoad.StartDate;
			tmp.EndDate = data.PayLoad.EndDate;
			tmp.Status = data.PayLoad.Status;

            _context.BoardMembers.Update(tmp);
            await _context.SaveChangesAsync();
            return new BoardMemberPayload(tmp);

        }

        public async Task Delete(BoardMember data)
        {
            var tmp = await _context.BoardMembers.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.BoardMembers.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}