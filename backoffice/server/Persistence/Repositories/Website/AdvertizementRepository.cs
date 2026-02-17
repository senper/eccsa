using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Website
{
    public class AdvertizementRepository: BaseRepository
	{
		private readonly EccsaContext _context;

		public AdvertizementRepository(EccsaContext context) : base(context)
		{
			_context = context;
		}
		public async Task<List<Advertizement>> List()
		{
			return await _context.Advertizements.ToListAsync();
		}
		public async Task<List<Advertizement>> List(AdvertisementSectionEnum section)
		{
			return await _context.Advertizements.Where(s => s.Section == section).ToListAsync();
		}
		public async Task<Advertizement> ById(Guid id)
		{
			return await _context.Advertizements.FirstOrDefaultAsync(l => l.Id == id);
		}
		public async Task Save(Advertizement data, Guid userId)
		{
			data.UpdateStamp = DateTime.UtcNow;
			data.UpdatedBy = userId;
			var tmp = await _context.Advertizements.FirstOrDefaultAsync(l => l.Id == data.Id);
			if (tmp == null)
			{
				if (data.Id == Guid.Empty)
				{
					data.Id = Guid.NewGuid();
				}
				data.CreatedBy = userId;
				data.CreateStamp = DateTime.UtcNow;
				_context.Advertizements.Add(data);
			}
			else
			{
				data.Extension = string.IsNullOrWhiteSpace(data.Extension) ? tmp.Extension : data.Extension;
				_context.Entry(tmp).CurrentValues.SetValues(data);
			}
			
			await _context.SaveChangesAsync();
		}
		
		
		
		public async Task Delete(Guid id)
		{
			var tmp = await ById(id);
			if (tmp == null) return;
			_context.Advertizements.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}


