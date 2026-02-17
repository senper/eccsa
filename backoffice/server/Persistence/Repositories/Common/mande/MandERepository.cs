using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using ECCSA.BackOffice.Server.Model.Security;
using System.Numerics;
using ECCSA.BackOffice.Server.Persistence.Migrations;
using System.Xml.Linq;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.mande
{
    public class MandERepository
    {
        private readonly EccsaContext _context;
        public MandERepository(EccsaContext context)
        {
            _context = context;
        }


        public async Task<List<int>> PlanYears()
        {
            return await _context.Plans.Select(x => x.PlanYear).Distinct().ToListAsync();
        }


        public async Task SaveProgress(MonthlyPlan data, User theUser)
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
            monthly.CumulativeWorkDone = data.CumulativeWorkDone;

            monthly.Status = ProgressStatusEnum.Draft;
            if (monthly.Plan > 0)
            {
                monthly.Progress = decimal.Round(data.Actual / monthly.Plan, 2);

            }
            tmp.RecalculateAnnual();
            tmp.DataMaintenanceHistory.Add(DataMaintenance.GetSaveProgress(theUser));

            _context.Plans.Update(tmp);
            if (tmp.FinancialItemId.HasValue)
            {
                await SaveFinancePlan(tmp, theUser);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ChangeCurrentPeriod()
        {
            var config = await Configuration();
            var planYears = await PlanYears();
            if (planYears.Count == 0)
            {
                throw new InvalidOperationException("Create Plan First");
            }
            if (config.CurrentYear == 0)
            {
                try
                {
                    config.CurrentYear = planYears.Where(p => p > config.ClosedYears.Max()).Min();
                    config.CurrentMonth = 1;
                }
                catch
                {
                    throw new InvalidOperationException("Create Plan First");
                }
            }
            else
            {
                if (config.CurrentMonth < 12)
                {
                    config.CurrentMonth += 1;
                }
                else
                {

                    
                    if (planYears.Any(p => p == config.CurrentYear + 1))
                    {
                        config.ClosedYears.Add(config.CurrentYear);
                        config.CurrentYear = config.CurrentYear + 1;
                        config.CurrentMonth = 1;
                        
                    }
                    else
                    {
                        config.CurrentYear = planYears[0];
                        config.CurrentMonth = 1;
                    }
                }
            }
            _context.PlanConfigurations.Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task ChangePlanYear(int planYear, int templateYear, User theUser)
        {
            var config = await Configuration();
            var planYears = await PlanYears();
            if (config.ClosedYears.Exists(p => p == planYear))
            {
                throw new InvalidOperationException("Period is closed");
            }

            if (config.PlanYear == planYear)
            {
                if (config.PlanningPhase != PlanningPhaseEnum.Closed)
                {
                    throw new InvalidOperationException($"{planYear} is already in planning mode");
                }

                config.PlanVersionNo += config.PlanVersionNo + 1;
            }
            else
            {
                config.PlanVersionNo = 1;
                config.PlanYear = planYear;
            }

            if (templateYear > 0 && !planYears.Exists(p => p == planYear))
            {
                var plans = await _context.Plans.Include(p => p.SubPlans).Where(p => p.PlanYear == templateYear).ToListAsync();
                if (plans != null)
                {
                    CopyPlan(plans, planYear, theUser, config);
                }
            }
            // await GenerateFinancialPlan(config);
            _context.PlanConfigurations.Update(config);
            await _context.SaveChangesAsync();
        }

        public async Task ClosePeriod(int planYear)
        {
            var config = await Configuration();
            var planYears = await PlanYears();
            if (config.ClosedYears.Exists(p => p == planYear)) return;
            try
            {
                var nextYear = planYears.Where(p => p > planYear).Min();
                config.CurrentYear = nextYear;
                config.CurrentMonth = 1;
            }
            catch
            {
                config.CurrentYear = 0;
                config.CurrentMonth = 0;
            }
            config.ClosedYears.Add(planYear);



            _context.PlanConfigurations.Update(config);
            await _context.SaveChangesAsync();
        }
        private void CopyPlan(List<Plan> sourcePlan, int newYear, User theUser, Model.Common.Planning.Configuration config, Guid? parentId = null, Guid? newParentId = null)
        {
            var children = sourcePlan.Where(p => p.ParentId == parentId).OrderBy(p => p.Code).ToList();
            foreach (var r in children)
            {
                if (!r.IsLabel) continue;
                var id = Guid.NewGuid();
                var p = new Plan()
                {
                    Id = id,
                    ParentId = newParentId,
                    Code = r.Code,
                    Name = r.Name,
                    Unit = r.Unit,
                    IsLabel = r.IsLabel,
                    ResponsibleUnit = r.ResponsibleUnit,
                    ExpectedResult = r.ExpectedResult,
                    PlanYear = newYear,
                    Planable = r.Planable,
                    VersionNo = config.PlanVersionNo,
                    BasePlanId = id,
                    Status = r.IsLabel ? PlanningStatusEnum.None : PlanningStatusEnum.Planning,
                    DataMaintenanceHistory = new List<DataMaintenance>()
                    {
                        DataMaintenance.GetCreate(theUser)
                    }

                };
                _context.Plans.Add(p);
                if (r.SubPlans.Count > 0)
                {
                    CopyPlan(sourcePlan, newYear, theUser, config, r.Id, p.Id);
                }
            }


        }
        private async Task GenerateFinancialPlan(Model.Common.Planning.Configuration setting)
        {

            var tmp = await _context.FinancialPlans.Where(f => f.PlanYear == setting.PlanYear).ToListAsync();
            if (tmp.Count > 0)
            {
                return;
            }
            var items = await _context.FinancialItems.Where(i => !i.IsLabel).OrderBy(i => i.Code).ToListAsync();

            foreach (var item in items)
            {
                _context.FinancialPlans.Add(new FinancialPlan(item.Id, setting.PlanYear));
            }


        }

        public async Task CreatePlanDetail()
        {
            var plans = await _context.Plans.ToListAsync();
            var conf = await _context.PlanConfigurations.FirstOrDefaultAsync();
            var currentMonth = conf.CurrentMonth;
            foreach (var plan in plans)
            {
                for (var i = 1; i < 13; i++)
                {
                    var pmonth = plan.PlanMonth(i);
                    if (!pmonth.HasPlan) continue;
                    var detailPlan = new DetailPlan
                    {
                        Id = Guid.NewGuid(),
                        PlanId = plan.Id,
                        BasePlanId = plan.BasePlanId,
                        Amount = pmonth.Plan,
                        Actual = pmonth.Actual,
                        MonthNo = i,
                        Progress = pmonth.Progress,

                    };
                    if (i <= currentMonth)
                    {
                        var detailProgress = new DetailProgress
                        {
                            Id = Guid.NewGuid(),
                            DetailPlanId = detailPlan.Id,
                            Actual = pmonth.Actual,
                            MonthNo = i,
                            Progress = pmonth.Progress,
                            Remark = pmonth.Remark,
                            Output = pmonth.Output,
                        };
                        detailPlan.DetailProgresses.Add(detailProgress);
                    }
                    plan.DetailPlans.Add(detailPlan);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RevisePlan(int planYear, int monthNo, User theUser)
        {
            var conf = await _context.PlanConfigurations.FirstOrDefaultAsync();
            conf.PlanVersionNo += 1;
            conf.PlanningPhase = PlanningPhaseEnum.Planning;

            var plans = await _context.Plans.Include(p => p.DetailPlans.Where(d => d.MonthNo >= monthNo))
                .Where(p => p.PlanYear == planYear && p.Status == PlanningStatusEnum.Approved).ToListAsync();
            foreach (var plan in plans)
            {
                var newPlan = new Plan
                {
                    Id = Guid.NewGuid(),
                    PlanYear = planYear,
                    Status = PlanningStatusEnum.Planning,
                    BasePlanId = plan.BasePlanId,
                    ResponsibleUnit = plan.ResponsibleUnit,
                    Code = plan.Code,
                    Unit = plan.Unit,
                    EstimatedBudget = plan.EstimatedBudget,
                    FromChamber = plan.FromChamber,
                    FromDonor = plan.FromDonor,
                    ExpectedResult = plan.ExpectedResult,
                    SupportingUnitId = plan.SupportingUnitId,
                    FinancialItemId = plan.FinancialItemId,
                    VersionNo = conf.PlanVersionNo,
                    DataMaintenanceHistory = new List<DataMaintenance>
                    {
                        new DataMaintenance
                        {
                            FullName = theUser.FullName,
                            UserId = theUser.Id,
                            OperationDateTime = DateTime.Now,
                            OperationType = DataMaintenance.Revise
                        }
                    },
                    Month01 = monthNo > 1 ? MonthlyPlan.Default : plan.Month01,
                    Month02 = monthNo > 2 ? MonthlyPlan.Default : plan.Month02,
                    Month03 = monthNo > 3 ? MonthlyPlan.Default : plan.Month03,
                    Month04 = monthNo > 4 ? MonthlyPlan.Default : plan.Month04,
                    Month05 = monthNo > 5 ? MonthlyPlan.Default : plan.Month05,
                    Month06 = monthNo > 6 ? MonthlyPlan.Default : plan.Month06,
                    Month07 = monthNo > 7 ? MonthlyPlan.Default : plan.Month07,
                    Month08 = monthNo > 8 ? MonthlyPlan.Default : plan.Month08,
                    Month09 = monthNo > 9 ? MonthlyPlan.Default : plan.Month09,
                    Month10 = monthNo > 10 ? MonthlyPlan.Default : plan.Month10,
                    Month11 = monthNo > 11 ? MonthlyPlan.Default : plan.Month11,
                    Month12 = monthNo > 12 ? MonthlyPlan.Default : plan.Month12,
                };
                foreach (var detailPlan in plan.DetailPlans)
                {
                    var newDetail = new DetailPlan
                    {
                        Id = Guid.NewGuid(),
                        PlanId = detailPlan.PlanId,
                        BasePlanId = detailPlan.BasePlanId,
                        MonthNo = detailPlan.MonthNo,
                        Amount = detailPlan.Amount,
                        Actual = 0,
                        Progress = 0,
                    };
                    newPlan.DetailPlans.Add(newDetail);
                }
                newPlan.InitMonthlyPlan();
                plan.Status = PlanningStatusEnum.Closed;
                if (plan.FinancialItemId.HasValue)
                {
                    await SaveFinancePlan(plan, theUser);
                    await SaveFinancePlan(newPlan, theUser);
                }
                plan.DataMaintenanceHistory.Add(new DataMaintenance
                {
                    FullName = theUser.FullName,
                    UserId = theUser.Id,
                    OperationDateTime = DateTime.Now,
                    OperationType = DataMaintenance.Close
                });
                _context.Plans.Add(newPlan);
            }


            await _context.SaveChangesAsync();

        }

        public async Task RejectPlan(Plan data, User theUser)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == data.Id);
            if (plan == null)
            {
                throw new InvalidOperationException($"Plan {data.Code} not found");
            }
            if (plan.Status != PlanningStatusEnum.Submitted)
            {
                throw new InvalidOperationException($"Plan {data.Code} not submitted");
            }
            plan.Status = PlanningStatusEnum.Rejected;
            plan.Remark = data.Remark;
            if (plan.FinancialItemId.HasValue)
                await SaveFinancePlan(plan, theUser);
            plan.DataMaintenanceHistory.Add(DataMaintenance.GetReject(theUser));
            await _context.SaveChangesAsync();
        }

        public async Task SubmitPlan(Plan data, int planYear, User theUser)
        {
            var plans = await _context.Plans.Where(p => p.PlanYear == planYear && p.ResponsibleUnit == data.ResponsibleUnit && p.SupportingUnitId == data.SupportingUnitId && (p.Status == PlanningStatusEnum.Planning || p.Status == PlanningStatusEnum.Rejected)).ToListAsync();

            foreach (var plan in plans)
            {
                if (!plan.CanBeSubmitted) continue;
                plan.Status = PlanningStatusEnum.Submitted;
                plan.DataMaintenanceHistory.Add(DataMaintenance.GetSubmitPlan(theUser));
                if (plan.FinancialItemId.HasValue)
                    await SaveFinancePlan(plan, theUser);
            }

            await _context.SaveChangesAsync();
        }
        public async Task ApprovePlan(Plan data, int planYear, User theUser)
        {

            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == data.Id);

            plan.Status = PlanningStatusEnum.Approved;
            plan.DataMaintenanceHistory.Add(DataMaintenance.GetApprove(theUser));
            if (plan.FinancialItemId.HasValue)
                await SaveFinancePlan(plan, theUser);

            await _context.SaveChangesAsync();
        }

        public async Task ApproveAllPlan(Plan data, int planYear, User theUser)
        {

            var submittedPlans = await _context.Plans.Where(p => p.ResponsibleUnit == data.ResponsibleUnit && p.SupportingUnitId == data.SupportingUnitId && p.Status == PlanningStatusEnum.Submitted).ToListAsync();
            foreach (var plan in submittedPlans)
            {
                plan.Status = PlanningStatusEnum.Approved;
                plan.DataMaintenanceHistory.Add(DataMaintenance.GetApprove(theUser));
                if (plan.FinancialItemId.HasValue)
                    await SaveFinancePlan(plan, theUser);
            }

            await _context.SaveChangesAsync();
        }

        public async Task CompletePlan(Plan data, User theUser)
        {
            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == data.Id);
            if (plan == null)
            {
                throw new InvalidOperationException($"Plan {data.Code} not found");
            }

            if (plan.Status != PlanningStatusEnum.Approved)
            {
                throw new InvalidOperationException($"Plan {data.Code} not in the correct status {data.Status}");
            }

            plan.Status = PlanningStatusEnum.Completed;
            plan.DataMaintenanceHistory.Add(DataMaintenance.GetComplete(theUser));
            if (plan.FinancialItemId.HasValue)
            {
                await SaveFinancePlan(plan, theUser);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Save(Plan data, User theUser)
        {
            if (data.ParentId.HasValue && data.ParentId.Value == Guid.Empty)
                data.ParentId = null;
            var tmp = await _context.Plans.FirstOrDefaultAsync(p => p.Id == data.Id);

            if (tmp != null)
            {
                tmp.DataMaintenanceHistory ??= new List<DataMaintenance>();
                var dmHistory = tmp.DataMaintenanceHistory;
                dmHistory.Add(DataMaintenance.GetUpdate(theUser));
                data.InternalCode = tmp.InternalCode;
                _context.Entry(tmp).CurrentValues.SetValues(data);
                tmp.DataMaintenanceHistory = dmHistory;
            }
            else
            {
                if (data.Id == Guid.Empty)
                {
                    data.Id = Guid.NewGuid();
                    data.InitMonthlyPlan();
                    data.DataMaintenanceHistory = new List<DataMaintenance>() { DataMaintenance.GetCreate(theUser) };
                }
                _context.Plans.Add(data);
            }
            if (data.FinancialItemId.HasValue && data.FinancialItemId != Guid.Empty)
            {
                await SaveFinancePlan(data, theUser);
            }


            await _context.SaveChangesAsync();
        }

        private async Task SaveFinancePlan(Plan data, User theUser)
        {
            if (!data.FinancialItemId.HasValue || data.FinancialItemId.Value == Guid.Empty)
                return;
            var fItem = await _context.FinancialItems.FirstOrDefaultAsync(f => f.Id == data.FinancialItemId && f.Status == StatusEnum.Active && !f.IsLabel);
            if (fItem == null || fItem.IsLabel) return;
            data.Unit = UnitOfMeasureEnum.Money;
            var financialPlan = await _context.FinancialPlans.FirstOrDefaultAsync(f => f.FinancialItemId == fItem.Id && f.PlanYear == data.PlanYear && f.VersionNo == data.VersionNo);
            if (financialPlan == null)
            {
                financialPlan = new FinancialPlan
                {
                    Id = Guid.NewGuid(),
                    FinancialItemId = fItem.Id,
                    PlanYear = data.PlanYear,
                    VersionNo = data.VersionNo,
                    AnnualPlan = data.EstimatedBudget,
                    Quarter1Plan = data.Quarter1Plan,
                    Quarter2Plan = data.Quarter2Plan,
                    Quarter3Plan = data.Quarter3Plan,
                    Quarter4Plan = data.Quarter4Plan,
                    Status = data.Status,
                    Quarter1Actual = data.Quarter1Actual,
                    Quarter2Actual = data.Quarter2Actual,
                    Quarter3Actual = data.Quarter3Actual,
                    Quarter4Actual = data.Quarter4Actual,
                    Quarter1Remark = data.Quarter1Remark,
                    Quarter2Remark = data.Quarter2Remark,
                    Quarter3Remark = data.Quarter3Remark,
                    Quarter4Remark = data.Quarter4Remark
                };

                _context.FinancialPlans.Add(financialPlan);

            }
            else
            {
                financialPlan.AnnualPlan = data.EstimatedBudget;
                financialPlan.Quarter1Plan = data.Quarter1Plan;
                financialPlan.Quarter2Plan = data.Quarter2Plan;
                financialPlan.Quarter3Plan = data.Quarter3Plan;
                financialPlan.Quarter4Plan = data.Quarter4Plan;
                financialPlan.Quarter1Actual = data.Quarter1Actual;
                financialPlan.Quarter2Actual = data.Quarter2Actual;
                financialPlan.Quarter3Actual = data.Quarter3Actual;
                financialPlan.Quarter4Actual = data.Quarter4Actual;

                financialPlan.Quarter1Remark = data.Quarter1Remark;
                financialPlan.Quarter2Remark = data.Quarter2Remark;
                financialPlan.Quarter3Remark = data.Quarter3Remark;
                financialPlan.Quarter4Remark = data.Quarter4Remark;
                financialPlan.Status = data.Status;
            }



        }



        private string PlanPeriod(int planYear, int periodNo, FrequencyEnum planType)
        {
            if (periodNo < 1 || periodNo > 12) return string.Empty;
            var periodCode = $"0{periodNo}";
            periodCode = periodCode[^2..];
            switch (planType)
            {
                case FrequencyEnum.None:
                    return string.Empty;
                case FrequencyEnum.Monthly:
                    return $"{planYear}_M{periodCode}";
                case FrequencyEnum.Quarterly:
                    return $"{planYear}_Q{periodNo}";
                default:
                    return planYear.ToString();
            }
        }


        private async Task<string> GetNextCode(IndicatorTypeEnum indicatorType, Guid parentId)
        {
            string code;
            int i;
            switch (indicatorType)
            {
                case IndicatorTypeEnum.ROOT:
                    code = await _context.Indicators.Where(x => x.IndicatorType == IndicatorTypeEnum.ROOT).MaxAsync(x => x.Code);
                    i = int.Parse(code) + 1;
                    code = $"0{i}";
                    code = code[^2..];
                    return code;
                default:
                    code = await _context.Indicators.Where(x => x.ParentId == parentId).MaxAsync(x => x.Code);
                    var parentCode = (await _context.Indicators.FirstOrDefaultAsync(x => x.Id == parentId)).Code;
                    i = int.Parse(code.Replace(parentCode, "").Replace(".", "")) + 1;
                    code = $"0{i}";
                    code = code[^2..];
                    return $"{parentCode}.{code}";
            }
        }

        public async Task<Model.Common.Planning.Configuration> Configuration()
        {
            var config = await _context.PlanConfigurations.FirstOrDefaultAsync();
            var plans = await _context.Plans.Where(p => p.PlanYear > 0).OrderBy(p => p.PlanYear).Take(1).ToListAsync();
            config.MinOperationYear = plans[0].PlanYear;
            config.MaxOperationYear = config.CurrentYear;
            return config;
        }

        public async Task<Plan> Plan(Guid id)
        {
            return await _context.Plans.Include(x => x.ParentPlan).Include(x => x.SubPlans).FirstOrDefaultAsync(p => p.Id == id);
        }

        private void AdjustInternalCode(List<Plan> allPlans, Plan parent)
        {
            var subPlans = allPlans.Where(p => p.ParentId.HasValue && p.ParentId.Value == parent.Id).OrderBy(p => p.Code)
                .ToList();
            var i = 1;
            foreach (var plan in subPlans)
            {

                var code = ($"00{i}");

                code = plan.IsLabel ? code.Substring(code.Length - 2) : code.Substring(code.Length - 3);
                plan.InternalCode = $"{parent.InternalCode}.{code}";
                i++;
                AdjustInternalCode(allPlans, plan);
            }
        }



        public async Task<List<Plan>> GetMonitoring()
        {
            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();

            return await _context.Plans.Where(p => p.PlanYear == setting.CurrentYear && p.VersionNo == setting.VersionNo)
                .ToListAsync();
        }
       
       
        public async Task<List<Plan>> Plans()
        {
         
            var setting = await _context.PlanConfigurations.AsNoTracking().FirstOrDefaultAsync();
            if (setting == null || setting.PlanningPhase == PlanningPhaseEnum.Closed || setting.PlanningPhase == PlanningPhaseEnum.Unknown)
                return new List<Plan>();

            var allPlans = await _context.Plans.ToListAsync();
           

            var planYear = setting.PlanYear;
            var versionNo = setting.PlanVersionNo;
            if (planYear == 0)
                return new List<Plan>();
            var plans = await _context.Plans
                .Where(p => p.PlanYear == planYear && p.VersionNo == versionNo).OrderBy(i => i.Code).ToListAsync();

            foreach (var plan in plans)
                plan.Config = setting;
            var rnd = new Random();
            if (setting.PlanningPhase == PlanningPhaseEnum.PrePlanning)
            {
                return plans.Where(p => p.IsLabel).ToList();
            }

            var hasChange = false;

            foreach (var plan in plans)
            {
                
                for (var i = 1; i < 13; i++)
                {
                    if (!plan.IsLabel && plan.HasPlan(i))
                    {
                        if (plan.PlanMonth(i).CumulativeWorkDone == null)
                        {
                            plan.PlanMonth(i).CumulativeWorkDone = string.Empty;
                            hasChange = true;
                        }
                    }
                }
                if (plan.DataMaintenanceHistory != null) continue;
                plan.DataMaintenanceHistory = new List<DataMaintenance>();
                var u1 = rnd.Next(60, 180);
                var u2 = rnd.Next(30, u1);
                var hr = rnd.Next(23);
                var min = rnd.Next(59);
                var sec = rnd.Next(59);
                plan.DataMaintenanceHistory.Add(new DataMaintenance
                {
                    FullName = "Haria Woldegebriel",
                    UserId = Guid.Parse("8b553a8a-2638-463a-97e0-faf423ad5de8"),
                    OperationType = "Create",
                    OperationDateTime = DateTime.Today.AddMonths(-6)
                });
                plan.DataMaintenanceHistory.Add(new DataMaintenance
                {
                    FullName = "Ermiyas Abay",
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000221"),
                    OperationType = "Update",
                    OperationDateTime = DateTime.Today.AddMonths(-6).AddDays(-u1).AddHours(hr).AddMinutes(min)
                        .AddSeconds(sec)
                });
                hr = rnd.Next(23);
                min = rnd.Next(59);
                sec = rnd.Next(59);
                plan.DataMaintenanceHistory.Add(new DataMaintenance
                {
                    FullName = "Ermiyas Abay",
                    UserId = Guid.Parse("00000000-0000-0000-0000-000000000221"),
                    OperationType = "Update",
                    OperationDateTime = DateTime.Today.AddMonths(-6).AddDays(-u2).AddHours(hr).AddMinutes(min)
                        .AddSeconds(sec)
                });
            }

            await _context.SaveChangesAsync();


            return plans.Where(p=> p.InternalCode != null).OrderBy(p=> p.InternalCode).ToList();
        }

        public async Task<List<QuarterMonthMap>> QuarterMonths()
        {
            return await _context.QuarterMonths.ToListAsync();
        }
        public async Task<List<Plan>> PlanProgresses(ECCSAModuleFlag unit)
        {

            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();
            var plans = await _context.Plans
                .Where(p => p.ResponsibleUnit.HasFlag(unit) &&
                        p.PlanYear == setting.CurrentYear && p.VersionNo == setting.VersionNo).OrderBy(i => i.Code).ToListAsync();

            return plans;
        }

        public async Task<Plan> PlanProgress(Guid id)
        {
            var plans = await _context.Plans
                .FirstOrDefaultAsync(p => p.Id == id);

            return plans;
        }

        public async Task Save(UnplannedActivity data)
        {
            var tmp = await _context.UnplannedActivities.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (string.IsNullOrEmpty(data.ActivitySummary))
                data.ActivitySummary = string.Empty;

            if (tmp == null)
            {
                data.Id = Guid.NewGuid();
                _context.UnplannedActivities.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<UnplannedActivity> GetUnplannedActivity(int planYear, int monthNo, int reportingUnit, int? departmentId)
        {
            var tmp = await _context.UnplannedActivities.FirstOrDefaultAsync(x => x.PlanYear == planYear && x.MonthNo == monthNo
            && x.ReportingUnit == reportingUnit && ((departmentId == null && x.SupportingUnitId == 0) || x.SupportingUnitId == departmentId));
            return tmp ?? new UnplannedActivity()
            {
                Id = Guid.Empty,
                PlanYear = planYear,
                MonthNo = monthNo,
                ReportingUnit = reportingUnit,
                ActivitySummary = string.Empty,
                NetIncome = 0,
                Status = PlanningStatusEnum.Planning
            };
        }

        public async Task<List<UnplannedActivity>> GetLatestUnplannedActivity(int planYear)
        {
            var lst = await _context.UnplannedActivities
                .Where(x => x.PlanYear == planYear).ToListAsync();
            var groupedList = lst.GroupBy(x => new { x.ReportingUnit, x.SupportingUnitId });
            List<UnplannedActivity> finalList = new List<UnplannedActivity>();
            foreach (var x in groupedList)
            {
                var tmp = new UnplannedActivity
                {
                    ReportingUnit = x.Key.ReportingUnit,
                    SupportingUnitId = x.Key.SupportingUnitId,
                    MonthNo = x.Max(l => l.MonthNo)
                };
                tmp.ActivitySummary = lst.FirstOrDefault(l =>
                    l.SupportingUnitId == tmp.SupportingUnitId && l.ReportingUnit == tmp.ReportingUnit &&
                    l.MonthNo == tmp.MonthNo)?.ActivitySummary;
                finalList.Add(tmp);
            }

            return finalList;
        }
        public async Task Save(Progress data)
        {
            var tmp = await _context.Progresses.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                data.Id = Guid.NewGuid();
                _context.Progresses.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProgress(Guid id)
        {
            var tmp = await _context.Progresses.FirstOrDefaultAsync(i => i.Id == id);
            _context.Progresses.Remove(tmp);
            await _context.SaveChangesAsync();
        }


        public async Task DeletePlan(Guid id)
        {
            var tmp = await _context.Plans.FirstOrDefaultAsync(i => i.Id == id);
            var prog = tmp.HasPlan();
            var child = await _context.Plans.AnyAsync(i => i.ParentId == id);
            if (prog || child)
            {
                throw new InvalidOperationException("Cannot Delete Plan with Progress or Child");
            }
            _context.Plans.Remove(tmp);
            await _context.SaveChangesAsync();
        }






        public async Task<List<Progress>> FindProgress(Progress data)
        {
            return await _context.Progresses.ToListAsync();

        }

        public async Task<Progress> FindProgressById(Progress data)
        {
            return await _context.Progresses.FirstOrDefaultAsync(p => p.Id == data.Id);

        }


        public async Task SaveProgress(Progress data)
        {
            var temp = await _context.Progresses.FirstOrDefaultAsync(p => p.PlanId == data.PlanId);
            if (temp == null)
            {
                data.Id = temp.Id;
                data.PlanId = temp.PlanId;
                data.Plan = temp.Plan;
                data.ReportingDate = temp.ReportingDate;
                data.ReportNo = temp.ReportNo;
                data.Physical = temp.Physical;
                data.Financial = temp.Financial;


                _context.Progresses.Add(data);
            }

            else
            {
                _context.Entry(temp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
            _context.Update(temp);


        }

        public async Task DeleteProgress(Progress data)
        {
            var temp = await _context.Progresses.FirstOrDefaultAsync(p => p.Id == data.Id && p.PlanId == data.PlanId);
            _context.Progresses.Remove(temp);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Plan>> GetRootPlans(int planYear)
        {
            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();
            return await _context.Plans.Where(p => p.PlanYear == planYear && p.Status != PlanningStatusEnum.Closed && !p.ParentId.HasValue).OrderBy(p => p.InternalCode).ToListAsync();
        }

        public async Task<List<Plan>> Performance()
        {
            var setting = await _context.PlanConfigurations.AsNoTracking().FirstOrDefaultAsync();
            return await _context.Plans
                .Where(p => p.PlanYear == setting.CurrentYear && p.VersionNo == setting.VersionNo).ToListAsync();
        }
        public async Task<List<Plan>> Performance(int planYear, int versionNo)
        {
            return await _context.Plans
                .Where(p => p.PlanYear == planYear).ToListAsync();
        }
        public async Task ClosePlanning()
        {
            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();
            setting.CurrentYear = setting.PlanYear;
            setting.VersionNo = setting.PlanVersionNo;
            setting.PlanningPhase = PlanningPhaseEnum.Closed;
            await _context.SaveChangesAsync();
        }

        public async Task<List<FinancialItem>> FinancialItems()
        {
            return await _context.FinancialItems.OrderBy(f => f.Code).ToListAsync();
        }



        public async Task RejectProgress(MonthlyPlan data, User theUser)
        {
            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();
            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == data.PlanId);


            if (plan == null)
            {
                throw new InvalidOperationException($"Plan  not found");
            }
            var tmpData = plan.PlanMonth(setting.CurrentMonth);
            if (tmpData.Status != ProgressStatusEnum.Submitted)
            {
                throw new InvalidOperationException($"Progress {plan.Code} period {data.MonthNo} not submitted");
            }
            tmpData.Status = ProgressStatusEnum.Rejected;
            tmpData.Remark = data.Remark;

            plan.DataMaintenanceHistory.Add(DataMaintenance.GetRejectProgress(theUser));
            _context.Plans.Update(plan);
            await _context.SaveChangesAsync();
        }

        public async Task SubmitProgress(Plan thePlan, User theUser)
        {
            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();
            var plans = await _context.Plans.Where(p => p.VersionNo == setting.VersionNo && p.PlanYear == setting.CurrentYear && p.ResponsibleUnit == thePlan.ResponsibleUnit && p.SupportingUnitId == thePlan.SupportingUnitId && p.Status == PlanningStatusEnum.Approved).ToListAsync();

            foreach (var plan in plans)
            {
                var monthlyData = plan.PlanMonth(setting.CurrentMonth);
                if (!monthlyData.CanBeSubmitted) continue;

                monthlyData.Status = ProgressStatusEnum.Submitted;
                plan.DataMaintenanceHistory.Add(DataMaintenance.GetSubmitProgress(theUser));
                _context.Plans.Update(plan);

            }

            await _context.SaveChangesAsync();
        }
        public async Task ApproveProgress(MonthlyPlan data, int planYear, User theUser)
        {
            var setting = await _context.PlanConfigurations.FirstOrDefaultAsync();

            var plan = await _context.Plans.FirstOrDefaultAsync(p => p.Id == data.PlanId);
            var monthlyData = plan.PlanMonth(setting.CurrentMonth);
            monthlyData.Status = ProgressStatusEnum.Approved;
            plan.DataMaintenanceHistory.Add(DataMaintenance.GetApproveProgress(theUser));
            if (plan.FinancialItemId.HasValue)
                await SaveFinancePlan(plan, theUser);

            _context.Plans.Update(plan);
            await _context.SaveChangesAsync();
        }



    }

}





