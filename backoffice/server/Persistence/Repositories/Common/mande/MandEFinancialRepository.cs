using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common.Planning;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.mande
{
    public class MandEFinancialRepository
    {
        private readonly EccsaContext _context;
        public MandEFinancialRepository(EccsaContext context)
        {
            _context = context;
        }
        public async Task Save(QuarterlyProgress data, Guid id)
        {
            var tmp = await _context.FinancialPlans.FirstOrDefaultAsync(p => p.Id == data.Id);
            if (tmp == null)
                throw new KeyNotFoundException("Plan not found");
            if (data.RemarkOnly)
            {
                tmp.SetRemark(data.QNo, data.Remark);
                _context.FinancialPlans.Update(tmp);
                await _context.SaveChangesAsync();
                return;
            }
            var items = await _context.FinancialItems.Include(i => i.ParentItem).Include(i => i.SubItems).ToListAsync();
            var lst = await _context.FinancialPlans.Where(p => p.PlanYear == tmp.PlanYear).ToDictionaryAsync(i => i.FinancialItemId);
            Dictionary<Guid, FinancialPlan> plans = new Dictionary<Guid, FinancialPlan>();
            foreach (var k in lst.Keys)
            {
                var x = lst[k];
                var t = new FinancialPlan(x);
                plans.Add(k, t);
            }

            tmp.SetActual(data.QNo, data.Actual, data.Remark);
            _context.FinancialPlans.Update(tmp);

            FinancialPlan parent;
            FinancialItem item = items.Find(f => f.Id == tmp.FinancialItemId);
            FinancialItem parentItem;
            while (item.ParentId.HasValue)
            {
                parentItem = item.ParentItem;
                parent = lst[item.ParentId.Value];
                var p = new FinancialPlan();
                foreach (var si in parentItem.SubItems)
                {
                    var sp = si.Id == tmp.FinancialItemId ? tmp : lst[si.Id];
                    p.Quarter1Actual += sp.Quarter1Actual;
                    p.Quarter2Actual += sp.Quarter2Actual;
                    p.Quarter3Actual += sp.Quarter3Actual;
                    p.Quarter4Actual += sp.Quarter4Actual;

                }
                parent.SetActual(data.QNo,p.Actual(data.QNo),parent.Remark(data.QNo));

                _context.FinancialPlans.Update(parent);
                item = parentItem;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Save(List<Plan> plans)
        {
            foreach (var plan in plans)
            {
                _context.Plans.Update(plan);
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveProgress(MonthlyPlan data)
        {
            var tmp = await _context.Plans.FirstOrDefaultAsync(p => p.Id == data.PlanId);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Plan Not found");
            }
            MonthlyPlan monthly;
            switch (data.MonthNo)
            {
                case 1:
                    monthly = tmp.Month01;
                    break;
                case 2:
                    monthly = tmp.Month02;
                    break;
                case 3:
                    monthly = tmp.Month03;
                    break;
                case 4:
                    monthly = tmp.Month04;
                    break;
                case 5:
                    monthly = tmp.Month05;
                    break;
                case 6:
                    monthly = tmp.Month06;
                    break;

                case 7:
                    monthly = tmp.Month07;
                    break;
                case 8:
                    monthly = tmp.Month08;
                    break;
                case 9:
                    monthly = tmp.Month09;
                    break;
                case 10:
                    monthly = tmp.Month10;
                    break;
                case 11:
                    monthly = tmp.Month11;
                    break;
                case 12:
                    monthly = tmp.Month12;
                    break;
                default:
                    throw new KeyNotFoundException("Month Not found");
            }
            monthly.Actual = data.Actual;
            monthly.Output = data.Output;
            monthly.Remark = data.Remark;
            if (monthly.Plan > 0)
            {
                monthly.Progress = decimal.Round(data.Actual / monthly.Plan, 2);
            }
            _context.Plans.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task Save(FinancialItem data, Guid userId)
        {
            var tmp = await _context.FinancialItems.FirstOrDefaultAsync(p => p.Id == data.Id);
            if (data.ParentId.HasValue && data.ParentId.Value == Guid.Empty)
                data.ParentId = null;
            if (tmp != null)
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            else
            {
                if (data.Id == Guid.Empty)
                {
                    data.Id = Guid.NewGuid();
                }
                _context.FinancialItems.Add(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Save(FinancialPlan data, Guid userId)
        {
            var tmp = await _context.FinancialPlans.FirstOrDefaultAsync(p => p.Id == data.Id);
            if (tmp == null)
                throw new KeyNotFoundException("Plan not found");
            var items = await _context.FinancialItems.Include(i => i.ParentItem).Include(i=> i.SubItems).ToListAsync();
            var lst = await _context.FinancialPlans.Where(p => p.PlanYear == tmp.PlanYear).ToDictionaryAsync(i => i.FinancialItemId);
            Dictionary<Guid, FinancialPlan> plans = new Dictionary<Guid, FinancialPlan>();
            foreach(var k in lst.Keys)
            {
                var x = lst[k];
                var t = new FinancialPlan(x);
                plans.Add(k, t);
            }
            
                tmp.SetPlan( data);
                _context.FinancialPlans.Update(tmp);
            
            FinancialPlan parent;
            FinancialItem item = items.Find(f=> f.Id == tmp.FinancialItemId);
            FinancialItem parentItem;
            while (item.ParentId.HasValue)
            {
                parentItem = item.ParentItem;
                parent = lst[item.ParentId.Value];
                var p = new FinancialPlan();
                foreach(var si in parentItem.SubItems)
                {
                    var sp = si.Id == tmp.FinancialItemId? tmp: lst[si.Id];

                    p.Quarter1Plan += sp.Quarter1Plan;
                    p.Quarter2Plan += sp.Quarter2Plan;
                    p.Quarter3Plan += sp.Quarter3Plan;
                    p.Quarter4Plan += sp.Quarter4Plan;
                    
                }
                parent.SetPlan(p);
                
                _context.FinancialPlans.Update(parent);
                item = parentItem;
            }
            await _context.SaveChangesAsync();
        }
        public async Task GeneratePlan(bool overWrite = false)
        {
            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();
            if (setting == null)
                throw new KeyNotFoundException("Plan Configuration not found");
            if (setting.PlanYear < 2000)
                throw new KeyNotFoundException("Plan Year not valid");
            var tmp = await _context.FinancialPlans.Where(f => f.PlanYear == setting.PlanYear).ToListAsync();
            if (tmp.Count > 0)
            {
                if (!overWrite)
                    throw new InvalidOperationException($"There is data for {setting.PlanYear}");
                else
                {
                    _context.FinancialPlans.RemoveRange(tmp);
                    await _context.SaveChangesAsync();
                }
            }
            var items = await _context.FinancialItems.Where(i => !i.IsLabel).OrderBy(i => i.Code).ToListAsync();

            foreach (var item in items)
            {
                _context.FinancialPlans.Add(new FinancialPlan(item.Id, setting.PlanYear));
            }
            await _context.SaveChangesAsync();

        }
        public async Task DeleteItem(Guid id)
        {
            var tmp = await _context.FinancialItems.Include(i => i.Plans).Include(i => i.SubItems).FirstOrDefaultAsync(i => i.Id == id);
            if (tmp == null)
                return;
            if (tmp.SubItems.Count > 0) return;
            if (tmp.Plans.Count > 0)
            {
                tmp.Status = StatusEnum.Inactive;
                _context.FinancialItems.Update(tmp);
            }
            else
            {
                _context.FinancialItems.Remove(tmp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlan(Guid id)
        {
            var tmp = await FinancialPlan(id);
            if (tmp == null) return;
            if (tmp.Status != PlanningStatusEnum.Planning) return;
            _context.FinancialPlans.Remove(tmp);
            await _context.SaveChangesAsync();
        }


        public async Task<Model.Common.Planning.Configuration> Configuration()
        {
            return await _context.PlanConfigurations.FirstOrDefaultAsync();
        }

        public async Task<List<FinancialPlan>> Financials(int planYear = 0)
        {
            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();
            if (setting == null)
                return new List<FinancialPlan>();
            planYear = planYear == 0 ? setting.PlanYear : planYear;
            if (planYear == 0)
                return new List<FinancialPlan>();
            var plans = await _context.FinancialPlans
                .Include(f => f.FinancialItem)
                .Where(p => p.PlanYear == planYear ).OrderBy(i => i.FinancialItem.Code).ToListAsync();

            return plans;
        }
        public async Task<FinancialPlan> FinancialPlan(Guid id)
        {
            return await _context.FinancialPlans
                .Include(f => f.FinancialItem)
                .ThenInclude(f => f.ParentItem)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task<FinancialItem> FinancialItem(Guid id)
        {
            return await _context.FinancialItems
                .Include(f => f.ParentItem)
                .Include(f => f.SubItems)
                .Include(f => f.Plans)
                .FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task<List<FinancialItem>> FinancialItems()
        {
            return await _context.FinancialItems
                .Include(f => f.ParentItem)
                .Include(f => f.SubItems)
                .Include(f => f.Plans)
                .ToListAsync();
        }
        public async Task<List<FinancialItem>> ActiveFinancialItems()
        {
            return await _context.FinancialItems
                .Include(f => f.ParentItem)
                .Include(f => f.SubItems)
                .Include(f => f.Plans)
                .Where(s => s.Status == StatusEnum.Active).ToListAsync();
        }
    }

}





