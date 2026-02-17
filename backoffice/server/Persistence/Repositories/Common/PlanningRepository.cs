using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common
{
	public class PlanningRepository
	{
		private readonly EccsaContext _context;
        public PlanningRepository(EccsaContext context)
		{
			_context = context;
		}

		public async Task<List<Indicator>> Indicators ()
		{
			return await _context.Indicators.ToListAsync();
		}

		public async Task<Indicator> IndicatorById(Guid id)
		{
			return await _context.Indicators.FirstOrDefaultAsync(i=> i.Id == id);
		}


		public async Task<List<Plan>> Plans(int planYear)
		{
			return await _context.Plans.Include(i => i.SubPlans).Where(p=> p.PlanYear == planYear ).ToListAsync();
		}

		public async Task<Plan> PlanById(Guid id)
		{
			return await _context.Plans.Include(i => i.SubPlans).Include(i=>i.SubPlans).FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<List<Progress>> Progresses(int planYear)
		{
			return await _context.Progresses.Include(i => i.Plan).Where(p => p.Plan.PlanYear == planYear).ToListAsync();
		}

		public async Task<Progress> ProgressById(Guid id)
		{
			return await _context.Progresses.FirstOrDefaultAsync(i => i.Id == id);
		}
		
		
		public async Task<Indicator> Save(Indicator data)
		{
			var lst = await _context.Indicators.ToListAsync();

			if (data.Id == Guid.Empty)
			{
				data.Id = Guid.NewGuid();
				if (data.IndicatorType == IndicatorTypeEnum.ROOT)
				{
					data.ParentId = null;
				} else {
					var parent = lst.First(i => i.ParentId.Value == i.Id);
					if(data.IndicatorType != IndicatorTypeEnum.HEADER && parent.IndicatorType == IndicatorTypeEnum.ROOT)
					{
						throw new Exception("Parent should be header");
					}


				}
				_context.Indicators.Add(data);
			}
			else
			{
				var tmp = await _context.Indicators.FirstOrDefaultAsync(r => r.Id == data.Id);

				if (tmp == null) throw new KeyNotFoundException("Indicator not found");
				//tmp.Code = data.Code;
				tmp.Name = data.Name;
				tmp.Description = data.Description;
				_context.Indicators.Update(tmp);
			}
			 await _context.SaveChangesAsync();
			return data;
		}

		public async Task PrepareAnnualPlan(int planYear, ECCSAModuleEnum module, DateTime startDate, DateTime endDate)
		{
			//var lst = await _context.Indicators.Where(i=> i.ResponsibleUnit == module && i.Status == StatusEnum.Active).ToListAsync();
			var plans = await _context.Plans
				.Where(i => i.PlanYear == planYear && i.ResponsibleUnit == module )
				.ToListAsync();
			
		}

		public async Task DeleteIndicator(Guid id)
		{
			var tmp = await _context.Indicators.Include(i=> i.SubIndicators).FirstOrDefaultAsync(r => r.Id == id);
			
			if (tmp == null) throw new KeyNotFoundException("Not found");
			if (tmp.SubIndicators.Count > 0 ) throw new Exception("Cannot Delete");
			_context.Indicators.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}