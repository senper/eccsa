using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class BusinessProfileRepository 
	{
		private readonly EccsaContext _context;
		private readonly EccsaUserContext _userContext;
		public BusinessProfileRepository(EccsaContext context, EccsaUserContext userContext)
		{
			_context = context;
			_userContext = userContext;
		}

		public IQueryable<BusinessProfile> BusinessProfiles(Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return _context.BusinessProfiles.Include(x => x.Individual)
.Include(x => x.ProfileAttribure)
;
		}

		public async Task<BusinessProfile> BusinessProfileById(Guid id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.BusinessProfiles.Include(x => x.Individual)
.Include(x => x.ProfileAttribure)
.FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<AssociationProfile> AssociationProfileById(Guid id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.AssociationProfiles.Include(x => x.Association)
.Include(x => x.ProfileAttribure)
.FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<List<AssociationProfile>> AssociationProfileByAssociationId(Guid id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.AssociationProfiles.Include(x => x.Association)
.Include(x => x.ProfileAttribure)
.Where(i => i.AssociationId == id).ToListAsync();
		}
		public async Task<List<BusinessProfile>> BusinessProfileByIndividualId(Guid id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.BusinessProfiles.Include(x => x.Individual)
								.Include(x => x.ProfileAttribure)
								.Where(i => i.IndividualId == id).ToListAsync();
		}

		public async Task<List<ProfileAttribure>> Attributes()
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.ProfileAttribures.ToListAsync();
		}
		public async Task SaveIndividualProfile(BusinessProfile data, Guid userId)
        {
			var parent = await _context.Individuals.Include(i => i.BusinessProfiles).FirstOrDefaultAsync(i => i.Id == data.IndividualId);
			if (parent == null) throw new KeyNotFoundException("Individual Not found");
			var tmp = parent.BusinessProfiles.FirstOrDefault(b => b.Id == data.Id);
			if(tmp == null)
            {
				data.Id = Guid.NewGuid();
				_context.BusinessProfiles.Add(data);
            }else
            {
				tmp.ProfileAttribureId = data.ProfileAttribureId;
				tmp.AttributeValue = data.AttributeValue;
				_context.BusinessProfiles.Update(tmp);
            }
			await _context.SaveChangesAsync();
		}
		public async Task SaveAssociationProfile(AssociationProfile data, Guid userId)
		{
			var parent = await _context.Associations.Include(i => i.AssociationProfiles).FirstOrDefaultAsync(i => i.Id == data.AssociationId);
			if (parent == null) throw new KeyNotFoundException("Association Not found");
			var tmp = parent.AssociationProfiles.FirstOrDefault(b => b.Id == data.Id);
			if (tmp == null)
			{
				data.Id = Guid.NewGuid();
				_context.AssociationProfiles.Add(data);
			}
			else
			{
				tmp.ProfileAttribureId = data.ProfileAttribureId;
				tmp.AttributeValue = data.AttributeValue;
				_context.AssociationProfiles.Update(tmp);
			}
			await _context.SaveChangesAsync();
		}

		//public async Task<BusinessProfilePayload> Create(GenericInput<CreateBusinessProfileInput> input)
		//{
		//	BusinessProfile data = new()
		//	{
		//					Id = Guid.NewGuid()
		//	, IndividualId = input.PayLoad.IndividualId
		//	, ProfileAttribureId = input.PayLoad.ProfileAttribureId
		//	, AttributeValue = input.PayLoad.AttributeValue

		//	};
		//	await _context.BusinessProfiles.AddAsync(data);
		//   var val = await _context.SaveChangesAsync();
		//   return new BusinessProfilePayload(data);
		//}

		//public async Task<BusinessProfilePayload> Update(GenericInput<UpdateBusinessProfileInput> data)
		//{
		//	var tmp = await  _context.BusinessProfiles.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
		//	if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
		//							tmp.IndividualId = data.PayLoad.IndividualId;
		//	tmp.ProfileAttribureId = data.PayLoad.ProfileAttribureId;
		//	tmp.AttributeValue = data.PayLoad.AttributeValue;

		//	_context.BusinessProfiles.Update(tmp);
		//	await _context.SaveChangesAsync();
		//	return new BusinessProfilePayload(tmp);

		//}

		public async Task Delete(BusinessProfile data)
		{
			var tmp = await _context.BusinessProfiles.FirstOrDefaultAsync(r => r.Id == data.Id);
			if (tmp == null) throw new KeyNotFoundException("Business Profile not found");
			_context.BusinessProfiles.Remove(tmp);
			await _context.SaveChangesAsync();
		}

		public async Task Delete(AssociationProfile data)
		{
			var tmp = await _context.AssociationProfiles.FirstOrDefaultAsync(r => r.Id == data.Id);
			if (tmp == null) throw new KeyNotFoundException("Association Profile not found");
			_context.AssociationProfiles.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}