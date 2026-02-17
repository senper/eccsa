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
    public class ProfileAttribureRepository 
	{
		private readonly EccsaContext _context;
		private readonly EccsaUserContext _userContext;
		public ProfileAttribureRepository(EccsaContext context, EccsaUserContext userContext)
		{
			_context = context;
			_userContext = userContext;
		}

		public IQueryable<ProfileAttribure> ProfileAttribures(Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return _context.ProfileAttribures
.Include(x => x.BusinessProfiles)

;
		}

		public async Task<ProfileAttribure> ProfileAttribureById(int id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.ProfileAttribures
.Include(x => x.BusinessProfiles)

.FirstOrDefaultAsync(i => i.Id == id);
		}
		public async Task<ProfileAttriburePayload> Create(GenericInput<CreateProfileAttribureInput> input)
		{
			ProfileAttribure data = new()
			{
			 NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal
			, Max = input.PayLoad.Max
			, Min = input.PayLoad.Min
			, AttributeType = input.PayLoad.Type
			,Source = input.PayLoad.Source

			};
			await _context.ProfileAttribures.AddAsync(data);
		   var val = await _context.SaveChangesAsync();
		   return new ProfileAttriburePayload(data);
		}

		public async Task<ProfileAttriburePayload> Update(GenericInput<UpdateProfileAttribureInput> data)
		{
			var tmp = await  _context.ProfileAttribures.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			tmp.NameEng = data.PayLoad.NameEng;
			tmp.NameLocal = data.PayLoad.NameLocal;
			tmp.Max = data.PayLoad.Max;
			tmp.Min = data.PayLoad.Min;
			tmp.AttributeType = data.PayLoad.Type;
			tmp.Source = data.PayLoad.Source;
			_context.ProfileAttribures.Update(tmp);
			await _context.SaveChangesAsync();
			return new ProfileAttriburePayload(tmp);

		}

		public async Task Delete(ProfileAttribure data)
		{
			var tmp = await _context.ProfileAttribures.FirstOrDefaultAsync(r => r.Id == data.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			_context.ProfileAttribures.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}