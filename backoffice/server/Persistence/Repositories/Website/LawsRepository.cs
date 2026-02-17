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
    public class LawsRepository : BaseRepository
	{
		private readonly EccsaContext _context;

		public LawsRepository(EccsaContext context): base(context)
		{
			_context = context;
		}
		public async Task<List<Law>> List()
        {
			return await _context.Laws.ToListAsync();
        }
		public async Task<List<Law>> List(LawCategoryEnum category, LawTypeEnum lawType)
		{
			return await _context.Laws.Where(l=> l.LawType == lawType && l.Category == category).ToListAsync();
		}
		public async Task<Law> ById(Guid id)
		{
			return await _context.Laws.FirstOrDefaultAsync(l=> l.Id == id);
		}
		public async Task Save(Law data, Guid userId)
		{
			data.UpdateStamp = DateTime.UtcNow;
			data.UpdatedBy = userId;

			var tmp = await _context.Laws.FirstOrDefaultAsync(l => l.Id == data.Id);
			if(tmp == null)
            {
				if(data.Id == Guid.Empty)
                {
					data.Id = Guid.NewGuid();
                }
				data.CreatedBy = userId;
				data.CreateStamp = DateTime.UtcNow;
				
				_context.Laws.Add(data);
            }else
            {
				_context.Entry(tmp).CurrentValues.SetValues(data);
            }
			await GenerateKeyword( data);
			await _context.SaveChangesAsync();
		}
		
		public async Task Delete(Guid id)
        {
			var tmp = await ById(id);
			if (tmp == null) return;
			_context.Laws.Remove(tmp);
			await RemoveKeyword(id,id);
			await _context.SaveChangesAsync();
        }
	}
}


