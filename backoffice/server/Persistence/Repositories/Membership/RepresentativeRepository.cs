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
	public class RepresentativeRepository 
	{
		private readonly EccsaContext _context;
		private readonly EccsaUserContext _userContext;
		public RepresentativeRepository(EccsaContext context, EccsaUserContext userContext)
		{
			_context = context;
			_userContext = userContext;
		}

		public IQueryable<Representative> Representatives(Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return _context.Representatives.Include(x => x.RepresentativeContacts)
.Include(x => x.Stakeholder)
;
		}

		public async Task<Representative> RepresentativeById(Guid id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.Representatives.Include(x => x.RepresentativeContacts)
.Include(x => x.Stakeholder)
.FirstOrDefaultAsync(i => i.Id == id);
		}
		public async Task<RepresentativePayload> Create(GenericInput<CreateRepresentativeInput> input)
		{
			Representative data = new()
			{
							Id = Guid.NewGuid()
			, MemberId = input.PayLoad.MemberId
			, Title = input.PayLoad.Title
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, OrderNo = input.PayLoad.OrderNo
			, Status = input.PayLoad.Status
			, Position = input.PayLoad.Position
			, StartDate = input.PayLoad.StartDate
			, EndDate = input.PayLoad.EndDate

			};
			await _context.Representatives.AddAsync(data);
		   var val = await _context.SaveChangesAsync();
		   return new RepresentativePayload(data);
		}

		public async Task<RepresentativePayload> Update(GenericInput<UpdateRepresentativeInput> data)
		{
			var tmp = await  _context.Representatives.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
									tmp.MemberId = data.PayLoad.MemberId;
			tmp.Title = data.PayLoad.Title;
			tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.OrderNo = data.PayLoad.OrderNo;
			tmp.Status = data.PayLoad.Status;
			tmp.Position = data.PayLoad.Position;
			tmp.StartDate = data.PayLoad.StartDate;
			tmp.EndDate = data.PayLoad.EndDate;

			_context.Representatives.Update(tmp);
			await _context.SaveChangesAsync();
			return new RepresentativePayload(tmp);

		}

		public async Task Delete(Representative data)
		{
			var tmp = await _context.Representatives.FirstOrDefaultAsync(r => r.Id == data.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			_context.Representatives.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}