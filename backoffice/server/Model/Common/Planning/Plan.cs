using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;

namespace ECCSA.BackOffice.Server.Model.Common.Planning
{
    public sealed class DataMaintenance
    {
        public const string Create = "Create";
        public const string Update = "Update";
        public const string SubmitPlan = "SubmitPlan";
        public const string Reject = "Reject";

        public const string Approve = "Approve";
        public const string SaveProgress = "SaveProgress";
        public const string SaveQuarterlyProgress = "SaveQuarterlyProgress";
        public const string SubmitProgress = "SubmitProgress";
        public const string RejectProgress = "RejectProgress";
        public const string ApproveProgress = "ApproveProgress";
        public const string Revise = "Revise";
        public const string Complete = "Complete";
        public const string Close = "Close";
        public Guid Id { get; set; } = Guid.NewGuid();
        public string OperationType { get; set; } 
        public DateTime OperationDateTime { get; set; } = DateTime.MinValue;

        public Guid UserId { get; set; }

        public string FullName { get; set; }

        private static DataMaintenance GetDataMaintenance(string operationType, User theUser)
        {
            return new DataMaintenance
            {
                FullName = theUser.FullName,
                UserId = theUser.Id,
                OperationType = operationType,
                OperationDateTime = DateTime.Now
            };
        }

        public static DataMaintenance GetApprove(User theUser) => GetDataMaintenance(DataMaintenance.Create, theUser);
        public static DataMaintenance GetSubmitProgress(User theUser) => GetDataMaintenance(DataMaintenance.SubmitProgress, theUser);
        public static DataMaintenance GetRejectProgress(User theUser) => GetDataMaintenance(DataMaintenance.RejectProgress, theUser);
        public static DataMaintenance GetApproveProgress(User theUser) => GetDataMaintenance(DataMaintenance.ApproveProgress, theUser);
        public static DataMaintenance GetRevise(User theUser) => GetDataMaintenance(DataMaintenance.Revise, theUser);
        public static DataMaintenance GetComplete(User theUser) => GetDataMaintenance(DataMaintenance.Complete, theUser);
        public static DataMaintenance GetClose(User theUser) => GetDataMaintenance(DataMaintenance.Close, theUser);

        public static DataMaintenance GetCreate(User theUser) => GetDataMaintenance(DataMaintenance.Create, theUser);
        public static DataMaintenance GetUpdate(User theUser) => GetDataMaintenance(DataMaintenance.Update, theUser);
        public static DataMaintenance GetSubmitPlan(User theUser) => GetDataMaintenance(DataMaintenance.SubmitPlan, theUser);
        public static DataMaintenance GetReject(User theUser) => GetDataMaintenance(DataMaintenance.Reject, theUser);

        public static DataMaintenance GetSaveProgress(User theUser) => GetDataMaintenance(DataMaintenance.SaveProgress, theUser);
        public static DataMaintenance GetSaveQuarterlyProgress(User theUser) => GetDataMaintenance(DataMaintenance.SaveQuarterlyProgress, theUser);

    }
    public sealed class UnplannedActivity
    {
        public Guid Id { get; set; }
        public int ReportingUnit { get; set; }
        public int PlanYear { get; set; }
        public int MonthNo { get; set; }
        public String ActivitySummary { get; set; }
        public int NetIncome { get; set; }
        public int SupportingUnitId { get; set; } = 0;
        public PlanningStatusEnum Status { get; set; }
        public List<DataMaintenance> DataMaintenanceHistory { get; set; }

        [NotMapped]
        public string ResponsibleUnitCode
        {
            get
            {
                switch ((ECCSAModuleEnum)ReportingUnit)
                {
                    case ECCSAModuleEnum.Communication:
                        return "ኮ/ሀግ/መ";
                    case ECCSAModuleEnum.MandE:
                        return "ዕ/ክ/ግ/ባ";
                    case ECCSAModuleEnum.Membership:
                        return "አ/ጉ";
                    case ECCSAModuleEnum.Academy:
                        return "ቻም/አካ";
                    case ECCSAModuleEnum.Arbitration:
                        return "ህ/ከፍል";
                    case ECCSAModuleEnum.Investment:
                    case ECCSAModuleEnum.TradePromotion:
                        return "ኢ/ን/ማ/መ";
                    case ECCSAModuleEnum.Research:
                        return "ም/አ/ዳ";
                    case ECCSAModuleEnum.Administration:
                        return "ኢ/ኮቴክ";
                    default: return "ተ/ድ/መ";
                }
            }
        }
    }
    public sealed class ReportingUnit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<ECCSAModuleEnum> Units { get; set; } = new List<ECCSAModuleEnum>();

        private static string GetResponsibleUnitName(ECCSAModuleEnum unit)
        {
            return unit switch
            {
                ECCSAModuleEnum.Communication => "Communication",
                ECCSAModuleEnum.MandE => "Monitoring & Evaluation",
                ECCSAModuleEnum.Membership => "Membership",
                ECCSAModuleEnum.Academy => "Chamber Academy",
                ECCSAModuleEnum.Arbitration => "Arbitration",
                ECCSAModuleEnum.Research => "Research & Advocacy",
                ECCSAModuleEnum.Investment => "Trade & Investment",
                ECCSAModuleEnum.TradePromotion => "Trade & Investment",
                ECCSAModuleEnum.Administration => "Information Technology",
                ECCSAModuleEnum.Support => "Administrative Support",
                _ => ""
            };
        }

        private static string GetResponsibleUnitCode(ECCSAModuleEnum unit)
        {
            return unit switch
            {
                ECCSAModuleEnum.Communication => "PR",
                ECCSAModuleEnum.MandE => "M&E",
                ECCSAModuleEnum.Membership => "MB",
                ECCSAModuleEnum.Academy => "CA",
                ECCSAModuleEnum.Arbitration => "AR",
                ECCSAModuleEnum.Research => "Re",
                ECCSAModuleEnum.Investment => "TI",
                ECCSAModuleEnum.TradePromotion => "TI",
                ECCSAModuleEnum.Administration => "IT",
                ECCSAModuleEnum.Support => "SU",
                _ => ""
            };
        }
        public static List<ReportingUnit> List { get; private set; }
        static ReportingUnit()
        {
            var set = new Dictionary<string, ReportingUnit>();
            foreach (int mid in Enum.GetValues(typeof(ECCSAModuleEnum)))
            {
                var unitEnum = (ECCSAModuleEnum)mid;
                var unitName = GetResponsibleUnitName(unitEnum);
                var unitCode = GetResponsibleUnitCode(unitEnum);
                if (set.ContainsKey(unitCode))
                {
                    set[unitCode].Units.Add(unitEnum);
                }
                else
                {
                    var unit = new ReportingUnit()
                    {
                        Id = mid,
                        Code = unitCode,
                        Name = unitName,
                        Units = new List<ECCSAModuleEnum>(10)
                        {
                            unitEnum
                        }
                    };
                    set.Add(unitCode,unit);
                }
                
            }

            List = set.Values.ToList();
        }
    }

    public class QuarterMonthMap
    {
        public int Quarter { get; set; }
        public int Month { get; set; }
    }
    public class FinancialItem
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public String Item { get; set; }
        [NotMapped]
        public String Name
        {
            get { return Item; }
            set { Item = value; }
        }
        [NotMapped]
        public string NameWithCode => $"{Code} {Name}";



        public FinancialPlan Plan(int planYear)
        {

            if (Plans == null || Plans.Count == 0) return FinancialPlan.Default;
            if (Plans.All(p => p.PlanYear != planYear)) return FinancialPlan.Default;
            return Plans.FirstOrDefault(p => p.PlanYear == planYear);

        }
        public Guid? ParentId { get; set; }

        public virtual FinancialItem ParentItem { get; set; }
        public virtual ICollection<FinancialItem> SubItems { get; set; }
        public virtual ICollection<FinancialPlan> Plans { get; set; }
        public bool IsLabel { get; set; }
        public StatusEnum Status { get; set; }
    }
    public class FinancialPlan
    {
        private static FinancialPlan _default;
        static FinancialPlan()
        {
            _default = new FinancialPlan();
            _default.Id = Guid.Empty;
            _default.PlanYear = 0;
            _default.FinancialItemId = Guid.Empty;
        }
        public FinancialPlan()
        {

        }
        public FinancialPlan(Guid id, decimal q1, decimal q2, decimal q3, decimal q4)
        {
            Id = id;
            Quarter1Plan = q1;
            Quarter2Plan = q2;
            Quarter3Plan = q3;
            Quarter4Plan = q4;
            AnnualPlan = q1 + q2 + q3 + q4;
        }
        public FinancialPlan(Guid id
            , decimal q1
            , decimal q2
            , decimal q3
            , decimal q4
            , decimal a1
            , decimal a2
            , decimal a3
            , decimal a4) : this(id, q1, q2, q3, q4)
        {
            Quarter1Actual = a1;
            Quarter2Actual = a2;
            Quarter3Actual = a3;
            Quarter4Actual = a4;
        }
        public FinancialPlan(Guid id
           , decimal q1
           , decimal q2
           , decimal q3
           , decimal q4
           , decimal a1
           , decimal a2
           , decimal a3
           , decimal a4
           , String remark1
           , String remark2
           , String remark3
           , String remark4) : this(id, q1, q2, q3, q4, a1, a2, a3, a4)
        {
            Quarter1Remark = remark1;
            Quarter2Remark = remark2;
            Quarter3Remark = remark3;
            Quarter4Remark = remark4;
        }
        public FinancialPlan(Guid itemId, int planYear)
        {
            Id = Guid.NewGuid();
            FinancialItemId = itemId;
            PlanYear = planYear;
            Status = PlanningStatusEnum.Planning;
        }
        public FinancialPlan(FinancialPlan p) : this(p.Id
            , p.Quarter1Plan, p.Quarter2Plan, p.Quarter3Plan, p.Quarter4Plan
            , p.Quarter1Actual, p.Quarter2Actual, p.Quarter3Actual, p.Quarter4Actual
            , p.Quarter1Remark, p.Quarter2Remark, p.Quarter3Remark, p.Quarter4Remark)
        {
            Id = p.Id;
            FinancialItemId = p.FinancialItemId;
            PlanYear = p.PlanYear;
            Status = p.Status;
        }
        [NotMapped]
        public static FinancialPlan Default => _default;
        public Guid Id { get; set; }
        public int PlanYear { get; set; }

        public int VersionNo { get; set; }
        public Guid FinancialItemId { get; set; }

        public PlanningStatusEnum Status { get; set; }

        
        public virtual FinancialItem FinancialItem { get; set; }
        public decimal AnnualPlan { get; set; } = decimal.Zero;
        public decimal AnnualActual { get; set; } = decimal.Zero;
        public decimal Actual(int qNo)
        {
            switch (qNo)
            {
                case 1: return Quarter1Actual;
                case 2: return Quarter2Actual;
                case 3: return Quarter3Actual;
                case 4: return Quarter4Actual;
                default: return decimal.Zero;
            }
        }
        [NotMapped]
        public string AnnualProgress
        {
            get
            {
                if (AnnualPlan == decimal.Zero) return "-";
                return decimal.Round((100 * AnnualActual) / AnnualPlan, 0).ToString();
            }
        }
        public decimal Plan(int qNo)
        {
            switch (qNo)
            {
                case 1: return Quarter1Plan;
                case 2: return Quarter2Plan;
                case 3: return Quarter3Plan;
                case 4: return Quarter4Plan;
                default: return decimal.Zero;
            }
        }
        public String Remark(int qNo)
        {
            switch (qNo)
            {
                case 1: return Quarter1Remark;
                case 2: return Quarter2Remark;
                case 3: return Quarter3Remark;
                case 4: return Quarter4Remark;
                default: return String.Empty;
            }
        }
        public decimal Quarter1Plan { get; set; } = decimal.Zero;
        public decimal Quarter1Actual { get; set; } = decimal.Zero;
        public String Quarter1Remark { get; set; } = String.Empty;

        public decimal Quarter2Plan { get; set; } = decimal.Zero;
        public decimal Quarter2Actual { get; set; } = decimal.Zero;
        public String Quarter2Remark { get; set; } = String.Empty;

        public decimal Quarter3Plan { get; set; } = decimal.Zero;
        public decimal Quarter3Actual { get; set; } = decimal.Zero;
        public String Quarter3Remark { get; set; } = String.Empty;

        public decimal Quarter4Plan { get; set; } = decimal.Zero;
        public decimal Quarter4Actual { get; set; } = decimal.Zero;
        public String Quarter4Remark { get; set; } = String.Empty;
        public void SetPlan(FinancialPlan plan)
        {
            Quarter1Plan = plan.Quarter1Plan;
            Quarter2Plan = plan.Quarter2Plan;
            Quarter3Plan = plan.Quarter3Plan;
            Quarter4Plan = plan.Quarter4Plan;
            AnnualPlan = Quarter1Plan + Quarter2Plan + Quarter3Plan + Quarter4Plan;
        }
        public void UpdatePlan(FinancialPlan current, FinancialPlan previous)
        {
            Quarter1Plan = current.Quarter1Plan - previous.Quarter1Plan;
            Quarter2Plan = current.Quarter2Plan - previous.Quarter2Plan;
            Quarter3Plan = current.Quarter3Plan - previous.Quarter3Plan;
            Quarter4Plan = current.Quarter4Plan - previous.Quarter4Plan;
            AnnualPlan = Quarter1Plan + Quarter2Plan + Quarter3Plan + Quarter4Plan;
        }

        public void SetRemark(int qNo, String remark)
        {
            switch (qNo)
            {
                case 1:
                    Quarter1Remark = remark;
                    break;
                case 2:
                    Quarter2Remark = remark;
                    break;
                case 3:
                    Quarter3Remark = remark;
                    break;
                case 4:
                    Quarter4Remark = remark;
                    break;
            }
        }
        public void SetActual(int qNo, decimal value, String remark)
        {
            SetRemark(qNo, remark);
            switch (qNo)
            {
                case 1:
                    Quarter1Actual = value;

                    break;
                case 2:
                    Quarter2Actual = value;
                    break;
                case 3:
                    Quarter3Actual = value;
                    break;
                case 4:
                    Quarter4Actual = value;
                    break;
            }
            AnnualActual = Quarter1Actual + Quarter2Actual + Quarter3Actual + Quarter4Actual;
        }
    }
    public class QuarterlyProgress
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public int QNo { get; set; }
        public decimal Actual { get; set; }
        public String Remark { get; set; }

        public bool RemarkOnly { get; set; }
    }
    public class Plan
    {
        public List<DataMaintenance> DataMaintenanceHistory { get; set; }
        [NotMapped]
        public Configuration Config { get; set; } = new Configuration();
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string InternalCode { get; set; }
        public bool Planable { get; set; }

        [NotMapped]
        public bool IsLeaf
        {
            get => Planable;
            set => Planable = value;
        }

        public Guid? ParentId { get; set; }
        public String Name { get; set; }

        [NotMapped]
        public string NameWithCode => $"{Code} {Name}";
        public bool IsLabel { get; set; }
        public int PlanYear { get; set; }

       public UnitOfMeasureEnum Unit { get; set; } = UnitOfMeasureEnum.None;

        [NotMapped] public string UnitName => IsLabel ? "" : Unit.GetDisplayName();

        [NotMapped]
        public string UnitCode
        {
            get
            {
                if (IsLabel) return string.Empty;
                switch (Unit)
                {
                    case UnitOfMeasureEnum.Count:
                        return "Cn";
                    case UnitOfMeasureEnum.Money:
                        return "Mo";


                    default: return "OT";
                }
            }
        }

        [NotMapped]
        public string ResponsibleUnitCode
        {
            get
            {
                if (IsLabel) return string.Empty;
                switch (ResponsibleUnit)
                {
                    case ECCSAModuleEnum.Communication:
                        return "ኮ/ሀግ/መ";
                    case ECCSAModuleEnum.MandE:
                        return "ዕ/ክ/ግ/ባ";
                    case ECCSAModuleEnum.Membership:
                        return "አ/ጉ";
                    case ECCSAModuleEnum.Academy:
                        return "ቻም/አካ";
                    case ECCSAModuleEnum.Arbitration:
                        return "ህ/ከፍል";
                    case ECCSAModuleEnum.Investment:
                    case ECCSAModuleEnum.TradePromotion:
                        return "ኢ/ን/ማ/መ";
                    case ECCSAModuleEnum.Research:
                        return "ም/አ/ዳ";
                    case ECCSAModuleEnum.Administration:
                        return "ኢ/ኮቴክ";
                    default: return "ተ/ድ/መ";
                }
            }
        }

        public int SupportingUnitId { get; set; } = 0;
        public int PlanAmount { get; set; } = 0;
        [NotMapped]
        public string PlanAmountName => IsLabel ? "" : PlanAmount.ToString();

        [NotMapped]
        public bool CanBeSubmitted
        {
            get
            {
                if (IsLabel) return false;
                return PlanAmount == TotalPlanAmount && PlanAmount != 0 && Status == PlanningStatusEnum.Planning;
            }
        }
        public PlanningStatusEnum Status { get; set; } = PlanningStatusEnum.Planning;

        public ECCSAModuleEnum ResponsibleUnit { get; set; } = ECCSAModuleEnum.None;
        [NotMapped]
        public string ResponsibleUnitName => ResponsibleUnit.GetDisplayName();

        public String ExpectedResult { get; set; } = string.Empty;

        public int EstimatedBudget { get; set; } = 0;
        public int FromChamber { get; set; } = 0;
        public int FromDonor { get; set; } = 0;
        public Guid? FinancialItemId { get; set; } = Guid.Empty;

        [NotMapped]
        public string EstimatedBudgetName => IsLabel && EstimatedBudget == 0 ? "" : (EstimatedBudget).ToString("##,###.00");
        [NotMapped]
        public string FromChamberName => IsLabel && EstimatedBudget == 0 ? "" : FromChamber.ToString();
        [NotMapped]
        public string FromDonorName => IsLabel && EstimatedBudget == 0 ? "" : FromDonor.ToString();

        [NotMapped] public string TotalPlanString => TotalPlanAmount == 0 ? "" : TotalPlanAmount.ToString("##,###.00");
        [NotMapped]
        public int TotalPlanAmount => (int)decimal.Round(
            Month01.Plan + Month02.Plan + Month03.Plan + Month04.Plan +
            Month05.Plan + Month06.Plan + Month07.Plan + Month08.Plan +
            Month09.Plan + Month10.Plan + Month11.Plan + Month12.Plan, 0);

        public string GetQuarterReport(int quarterNo)
        {
            switch (quarterNo)
            {
                case 1:
                    if (Month03.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month03.CumulativeWorkDone))
                        return Month03.CumulativeWorkDone;
                    if (Month02.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month02.CumulativeWorkDone))
                        return Month02.CumulativeWorkDone;
                    if (Month01.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month01.CumulativeWorkDone))
                        return Month01.CumulativeWorkDone;
                    return string.Empty;
                case 2:
                    if (Month06.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month06.CumulativeWorkDone))
                        return Month06.CumulativeWorkDone;
                    
                    if (Month05.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month05.CumulativeWorkDone))
                        return Month05.CumulativeWorkDone;
                    if (Month04.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month04.CumulativeWorkDone))
                        return Month04.CumulativeWorkDone;
                    return string.Empty;
                case 3:
                    if (Month09.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month09.CumulativeWorkDone))
                        return Month09.CumulativeWorkDone;
                    if (Month08.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month08.CumulativeWorkDone))
                        return Month08.CumulativeWorkDone;
                    if (Month07.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month07.CumulativeWorkDone))
                        return Month07.CumulativeWorkDone;
                    return string.Empty;
                default:
                    if (Month12.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month12.CumulativeWorkDone))
                        return Month12.CumulativeWorkDone;
                    if (Month11.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month11.CumulativeWorkDone))
                        return Month11.CumulativeWorkDone;
                    if (Month10.Status == ProgressStatusEnum.Approved &&
                        !string.IsNullOrEmpty(Month10.CumulativeWorkDone))
                        return Month10.CumulativeWorkDone;
                    return string.Empty;
            }
            return String.Empty;
        }
        public MonthlyPlan Month01 { get; set; } = MonthlyPlan.Default;
        public MonthlyPlan Month02 { get; set; } = MonthlyPlan.Default;
        public MonthlyPlan Month03 { get; set; } = MonthlyPlan.Default;

        public MonthlyPlan Month04 { get; set; } = MonthlyPlan.Default;
        public MonthlyPlan Month05 { get; set; } = MonthlyPlan.Default;
        public MonthlyPlan Month06 { get; set; } = MonthlyPlan.Default;

        public MonthlyPlan Month07 { get; set; } = MonthlyPlan.Default;
        public MonthlyPlan Month08 { get; set; } = MonthlyPlan.Default;
        public MonthlyPlan Month09 { get; set; } = MonthlyPlan.Default;

        public MonthlyPlan Month10 { get; set; } = MonthlyPlan.Default;
        public MonthlyPlan Month11 { get; set; } = MonthlyPlan.Default;
        public MonthlyPlan Month12 { get; set; } = MonthlyPlan.Default;

        [NotMapped]
        public decimal Quarter1Plan => Month01.Plan + Month02.Plan + Month03.Plan;
        [NotMapped]
        public decimal Quarter1Actual => Month01.Actual + Month02.Actual + Month03.Actual;

        [NotMapped]
        public string Quarter1Output
        {
            get
            {
                if (!String.IsNullOrEmpty( Month03.CumulativeWorkDone))
                    return Month03.CumulativeWorkDone;
                if (!String.IsNullOrEmpty(Month02.CumulativeWorkDone))
                    return Month02.CumulativeWorkDone;
                if (!String.IsNullOrEmpty(Month01.CumulativeWorkDone))
                    return Month01.CumulativeWorkDone;
                return string.Empty;
            }
        }

        [NotMapped]
        public decimal Quarter2Plan => Month04.Plan + Month05.Plan + Month06.Plan;
        [NotMapped]
        public decimal Quarter2Actual => Month04.Actual + Month05.Actual + Month06.Actual;

        
        [NotMapped]
        public string Quarter2Output
        {
            get
            {
                if (!String.IsNullOrEmpty(Month06.CumulativeWorkDone))
                    return Month06.CumulativeWorkDone;
                if (!String.IsNullOrEmpty(Month05.CumulativeWorkDone))
                    return Month05.CumulativeWorkDone;
                if (!String.IsNullOrEmpty(Month04.CumulativeWorkDone))
                    return Month04.CumulativeWorkDone;
                return Quarter1Output;
            }
        }

        [NotMapped]
        public decimal Quarter3Plan => Month07.Plan + Month08.Plan + Month09.Plan;
        [NotMapped]
        public decimal Quarter3Actual => Month07.Actual + Month08.Actual + Month09.Actual;

        [NotMapped]
        public string Quarter3Output
        {
            get
            {
                if (!String.IsNullOrEmpty(Month09.CumulativeWorkDone))
                    return Month09.CumulativeWorkDone;
                if (!String.IsNullOrEmpty(Month08.CumulativeWorkDone))
                    return Month08.CumulativeWorkDone;
                if (!String.IsNullOrEmpty(Month07.CumulativeWorkDone))
                    return Month07.CumulativeWorkDone;
                return Quarter2Output;
            }
        }

        [NotMapped]
        public decimal Quarter4Plan => Month10.Plan + Month11.Plan + Month12.Plan;
        [NotMapped]
        public decimal Quarter4Actual => Month10.Actual + Month11.Actual + Month12.Actual;


        [NotMapped]
        public string Quarter4Output
        {
            get
            {
                if (!String.IsNullOrEmpty(Month12.CumulativeWorkDone))
                    return Month12.CumulativeWorkDone;
                if (!String.IsNullOrEmpty(Month11.CumulativeWorkDone))
                    return Month11.CumulativeWorkDone;
                if (!String.IsNullOrEmpty(Month10.CumulativeWorkDone))
                    return Month10.CumulativeWorkDone;
                return Quarter3Output;
            }
        }

        public String Quarter1Remark { get; set; } = string.Empty;
        public String Quarter2Remark { get; set; } = string.Empty;
        public String Quarter3Remark { get; set; } = string.Empty;
        public String Quarter4Remark { get; set; } = string.Empty;
        public decimal QuarterPlan(int monthNo, int currentPeriod)
        {

            switch (monthNo)
            {
                case < 1: return decimal.Zero;
                case <= 3: return Quarter1Plan;
                case <= 6: return Quarter1Plan + Quarter2Plan;
                case <= 9: return Quarter3Plan;
                case <= 12: 
                    if(currentPeriod == 12)
                      return Quarter1Plan + Quarter2Plan + Quarter3Plan + Quarter4Plan;
                    decimal tmp = decimal.Zero;
                    for(var i = 1; i <= currentPeriod; i++)
                        if (HasPlan(i))
                        {
                            tmp += PlanMonth(i).Plan;
                        }
                    return tmp; 
                default: return decimal.Zero;
            }
        }
        public ( decimal plan, decimal actual, string cummulativeWork) TotalPlanActual(int monthNo)
        {
            decimal tmpPlan = decimal.Zero;
            decimal tmpActual = decimal.Zero;
            string cummulativeWork = string.Empty;
            string workDone = string.Empty;
            for (var i = monthNo; i >= 1; i--)
                if (HasPlan(i))
                {
                    var pmonth = PlanMonth(i);
                    tmpPlan += pmonth.Plan;
                    tmpActual += pmonth.Actual;
                    if (!string.IsNullOrEmpty(pmonth.Output) && workDone.Length < 3)
                    {
                        workDone = pmonth.Output;
                    }
                    if (!string.IsNullOrEmpty(pmonth.CumulativeWorkDone) && cummulativeWork.Length < 3)
                    {
                        cummulativeWork = pmonth.CumulativeWorkDone;
                    }
                }
            if (cummulativeWork.Length < 3)
                cummulativeWork = workDone;
            return ( tmpPlan, tmpActual, cummulativeWork);
        }
        public decimal QuarterActual(int monthNo, int currentPeriod)
        {
            switch (monthNo)
            {
                case < 1: return decimal.Zero;
                case <= 3: return Quarter1Actual;
                case <= 6: return Quarter2Actual + Quarter1Actual;
                case <= 9: return Quarter3Actual;
                case <= 12:
                    if (currentPeriod == 12)
                        return Quarter2Actual + Quarter1Actual + Quarter3Actual + Quarter4Actual;
                    var tmp = decimal.Zero;
                    for (var i = 1; i <= currentPeriod; i++)
                        if (HasPlan(i))
                            tmp += PlanMonth(i).Actual;
                    return tmp;
                default: return decimal.Zero;
            }
        }
        public String QuarterRemark(int monthNo)
        {
            switch (monthNo)
            {
                case < 1: return String.Empty;
                case <= 3: return Quarter1Remark;
                case <= 6: return Quarter2Remark;
                case <= 9: return Quarter3Remark;
                case <= 12: return Quarter4Remark;
                default: return string.Empty;
            }
        }
        public String QuarterOutput(int monthNo)
        {
            switch (monthNo)
            {
                case < 1: return String.Empty;
                case <= 3: return Quarter1Output;
                case <= 6: return Quarter1Output + Quarter2Output;
                case <= 9: return Quarter3Output;
                case <= 12: return Quarter1Output + Quarter2Output + Quarter3Output + Quarter4Output;
                default: return string.Empty;
            }
        }
        public int QuarterNo(int monthNo)
        {
            switch (monthNo)
            {
                case < 1: return 0;
                case <= 3: return 1;
                case <= 6: return 2;
                case <= 9: return 3;
                case <= 12: return 4;
                default: return 0;
            }
        }
        public bool HasPlan(int monthNo)
        {
            if (IsLabel) return true;
            switch (monthNo)
            {
                case 1: return Month01.HasPlan ;
                case 2: return Month02.HasPlan ;
                case 3: return Month03.HasPlan ;
                case 4: return Month04.HasPlan ;
                case 5: return Month05.HasPlan ;
                case 6: return Month06.HasPlan ;
                case 7: return Month07.HasPlan ;
                case 8: return Month08.HasPlan ;
                case 9: return Month09.HasPlan ;
                case 10: return Month10.HasPlan ;
                case 11: return Month11.HasPlan ;
                case 12: return Month12.HasPlan ;
                default: return false;
            }
        }
        public bool HasPlan()
        {
            if (IsLabel) return false;
            for (var i = 1; i < 13; i++)
            {
                if (HasPlan(i)) return true;
            }
            return false;

        }
        public bool HasQuarterlyPlan(int monthNo)
        {
            if (IsLabel) return true;
            switch (monthNo)
            {
                case < 1: return false;
                case <= 3: return Month01.HasPlan || Month02.HasPlan || Month03.HasPlan;
                case <= 6: return Month01.HasPlan || Month02.HasPlan || Month03.HasPlan || 
                                  Month04.HasPlan || Month05.HasPlan || Month06.HasPlan;
                case <= 9: return Month07.HasPlan || Month08.HasPlan || Month09.HasPlan;
                case <= 12: return Month01.HasPlan || Month02.HasPlan || Month03.HasPlan ||
                                   Month04.HasPlan || Month05.HasPlan || Month06.HasPlan ||
                                   Month07.HasPlan || Month08.HasPlan || Month09.HasPlan || 
                                   Month10.HasPlan || Month11.HasPlan || Month12.HasPlan;

                default: return false;
            }
        }
        public MonthlyPlan PlanMonth(int monthNo)
        {
            switch (monthNo)
            {
                case 1: return Month01;
                case 2: return Month02;
                case 3: return Month03;
                case 4: return Month04;
                case 5: return Month05;
                case 6: return Month06;
                case 7: return Month07;
                case 8: return Month08;
                case 9: return Month09;
                case 10: return Month10;
                case 11: return Month11;
                case 12: return Month12;
                default: return MonthlyPlan.Default;
            }
        }
        public void InitMonthlyPlan()
        {
            Month01.MonthNo = 1;
            Month02.MonthNo = 2;
            Month03.MonthNo = 3;
            Month04.MonthNo = 4;
            Month05.MonthNo = 5;
            Month06.MonthNo = 6;
            Month07.MonthNo = 7;
            Month08.MonthNo = 8;
            Month09.MonthNo = 9;
            Month10.MonthNo = 10;
            Month11.MonthNo = 11;
            Month12.MonthNo = 12;

            Month01.PlanId = Id;
            Month02.PlanId = Id;
            Month03.PlanId = Id;
            Month04.PlanId = Id;
            Month05.PlanId = Id;
            Month06.PlanId = Id;
            Month07.PlanId = Id;
            Month08.PlanId = Id;
            Month09.PlanId = Id;
            Month10.PlanId = Id;
            Month11.PlanId = Id;
            Month12.PlanId = Id;
        }
        public int AnnualProgress { get; set; } = 0;
        public void RecalculateAnnual()
        {
            AnnualProgress = (int)decimal.Round(
            Month01.Actual + Month02.Actual + Month03.Actual + Month04.Actual +
            Month05.Actual + Month06.Actual + Month07.Actual + Month08.Actual +
            Month09.Actual + Month10.Actual + Month11.Actual + Month12.Actual, 0);


            Quarter1Remark = GetQuarterReport(1);
            Quarter2Remark = GetQuarterReport(2);
            Quarter3Remark = GetQuarterReport(3);
            Quarter4Remark = GetQuarterReport(4);
        }
        public String Remark { get; set; } = string.Empty;

        public virtual ICollection<Plan> SubPlans { get; set; } = new List<Plan>();

        public virtual Plan ParentPlan { get; set; }

        [NotMapped]
        public string ReportedPlan { get; set; } = String.Empty;

        [NotMapped]
        public string ReportedActual { get; set; } = String.Empty;

        [NotMapped]
        public string ReportedProgress { get; set; } = String.Empty;

        [NotMapped]
        public string ReportedProgressStyle
        {
            get
            {
                if(string.IsNullOrEmpty(ReportedProgress)) return string.Empty;
                if(Config ==null) return string.Empty;
                decimal.TryParse(ReportedProgress, out var val);
                val = val / 100;
                if (val <= Config.Yello) return $"background-color: {Configuration.RedColor};color: black;";
                if (val <= Config.Blue) return $"background-color: {Configuration.OrangeColor};color: black;";
                if (val <= Config.BlueGreen) return $"background-color: {Configuration.YelloColor};color: black;";
                if (val <= Config.Green) return $"background-color: {Configuration.YellowGreen};color: black;";
                return $"background-color: {Configuration.GreenColor};color: black;";
            }
        }

        [NotMapped]
        public string ReportedProgressRange
        {
            get
            {
                if (string.IsNullOrEmpty(ReportedProgress)) return string.Empty;
                if (Config == null) return string.Empty;
                decimal.TryParse(ReportedProgress, out var val);
                val = val / 100;
                if (val <= Config.Yello) return Config.YelloValue.ToString();
                if (val <= Config.Blue) return Config.BlueValue.ToString();
                if (val <= Config.BlueGreen) return Config.BlueGreenValue.ToString();
                if (val <= Config.Green) Config.GreenValue.ToString();
                return $"100";
            }
        }

        [NotMapped]
        public string ReportedPerformance { get; set; } = String.Empty;

        [NotMapped]
        public string ReportedYtDActual { get; set; } = String.Empty;

        [NotMapped]
        public string ReportedYtDPlan { get; set; } = String.Empty;

        [NotMapped]
        public string ReportedYtDProgress { get; set; } = String.Empty;

        [NotMapped]
        public string ReportedYtDProgressStyle
        {
            get
            {
                if (string.IsNullOrEmpty(ReportedYtDProgress)) return string.Empty;
                if (Config == null) return string.Empty;
                decimal.TryParse(ReportedYtDProgress, out var val);
                val = val / 100;
                if (val <= Config.Yello) return $"background-color: {Configuration.RedColor};color: black;";
                if (val <= Config.Blue) return $"background-color: {Configuration.OrangeColor};color: black;";
                if (val <= Config.BlueGreen) return $"background-color: {Configuration.YelloColor};color: black;";
                if (val <= Config.Green) return $"background-color: {Configuration.YellowGreen};color: black;";
                return $"background-color: {Configuration.GreenColor};color: black;";
            }
        }
        [NotMapped]
        public string ReportedYtDOutput { get; set; } = String.Empty;

        [NotMapped]
        public string ReportedOutput { get; set; } = String.Empty;

        public int VersionNo { get; set; } = 1;
        public Guid BasePlanId { get; set; }

        public virtual ICollection<DetailPlan> DetailPlans { get; set; }

        public void SetMonthlyPlan(int monthNo, decimal planAmount)
        {
            var planMonth =PlanMonth(monthNo);
            planMonth.Plan = planAmount;
            planMonth.HasPlan = true;
        }
        public Plan Copy()
        {
            var copy= new Plan
            {
                Id = Id,
                Status = Status,
                AnnualProgress = AnnualProgress,
                BasePlanId = BasePlanId,
                Code = Code,
                Remark = Remark,
                VersionNo = VersionNo,
                EstimatedBudget = EstimatedBudget,
                ExpectedResult = ExpectedResult,
                FromChamber = FromChamber,
                FromDonor = FromDonor,
                IsLabel = IsLabel,
                ParentId = ParentId,
                Name = Name,
                PlanYear = PlanYear,
                Unit = Unit ,
                SupportingUnitId = SupportingUnitId,
                PlanAmount = PlanAmount,
                ResponsibleUnit = ResponsibleUnit,
                Month01 = Month01,
                Month02 = Month02,
                Month03 = Month03,
                Month04 = Month04,
                Month05 = Month05,
                Month06 = Month06,
                Month07 = Month07,
                Month08 = Month08,
                Month09 = Month09,
                Month10 = Month10,
                Month11 = Month11,
                Month12 = Month12,
                Quarter1Remark = Quarter1Remark,
                Quarter2Remark = Quarter2Remark,
                Quarter3Remark = Quarter3Remark,
                Quarter4Remark = Quarter4Remark,
            };
            copy.SubPlans = new List<Plan>();
            return copy;
        }
    }

    public class DetailPlan
    {
        public Guid Id { get; set; }
        public Guid PlanId { get; set; }

        public Guid BasePlanId { get; set; }
        public int MonthNo { get; set; }
        public decimal Amount { get; set; }
        public virtual Plan Plan { get; set; }
        public decimal Actual { get; set; } = decimal.Zero;
        public decimal Progress { get; set; } = decimal.Zero;
        public virtual ICollection<DetailProgress> DetailProgresses { get; set; }
    }

    public class DetailProgress
    {
        public Guid Id { get; set; }
        public Guid DetailPlanId { get; set; }
        public int MonthNo { get; set; }
        public virtual DetailPlan DetailPlan { get; set; }
        public decimal Actual { get; set; } = decimal.Zero;
        public decimal Progress { get; set; } = decimal.Zero;
        public String Output { get; set; } = String.Empty;
        public String Remark { get; set; } = String.Empty;
        
    }


    public class MonthlyPlan
    {
        public int MonthNo { get; set; } = 0;
        public bool HasPlan { get; set; } = false;
        public String PlanText { get; set; } = String.Empty;
        public decimal Plan { get; set; } = decimal.Zero;
        public decimal Actual { get; set; } = decimal.Zero;
        public decimal Progress { get; set; } = decimal.Zero;
        public String Output { get; set; } = String.Empty;
        public String Remark { get; set; } = String.Empty;

        public ProgressStatusEnum Status { get; set; } = ProgressStatusEnum.None;
        public String CumulativeWorkDone { get; set; } = string.Empty;
        [NotMapped]
        public static MonthlyPlan Default => new MonthlyPlan();
        public Guid PlanId { get; set; }

        [NotMapped]
        public string Disable => HasPlan ? "" : " disabled ";
        [NotMapped]
        public bool CanBeSubmitted => Status == ProgressStatusEnum.Draft && (Actual != decimal.Zero || !String.IsNullOrEmpty(Output) || !string.IsNullOrEmpty(CumulativeWorkDone));
    }

    public class ScoreCard
    {


        public static List<string> ResponsibleUnits
        {
            get
            {
                var set = new HashSet<string>(20);
                foreach (int mid in Enum.GetValues(typeof(ECCSAModuleEnum)))
                {
                    var unitCode = (ECCSAModuleEnum)mid;
                    var unitName = GetResponsibleUnitCode(unitCode);
                    if(set.Contains(unitName))continue;
                    set.Add(unitName);
                }

                return set.ToList();
            }
        }
        public static string GetResponsibleUnitCode(ECCSAModuleEnum unit)
        {
            return unit switch
            {
                ECCSAModuleEnum.Communication => "Communication -PR",
                ECCSAModuleEnum.MandE => "Monitoring & Evaluation - M&E",
                ECCSAModuleEnum.Membership => "Membership - MB",
                ECCSAModuleEnum.Academy => "Chamber Academy - CA",
                ECCSAModuleEnum.Arbitration => "Arbitration - AR",
                ECCSAModuleEnum.Research => "Research & Advocacy - Re",
                ECCSAModuleEnum.Investment => "Trade Promotion - IP",
                ECCSAModuleEnum.TradePromotion => "Trade Facilitation - TF",
                ECCSAModuleEnum.Administration => "Information Technology - IT",
                _ => "Administrative Support - SU"
            };
        }
        public string ResponsibleUnit { get; set; } 
        public String PeriodName { get; set; }
        
        public int TotalIndicators { get; set; }
        public int FIndicators { get; set; }
        public int DIndicators { get; set; }

        public int BIndicators { get; set; }
        public int AIndicators { get; set; }
        public int CIndicators => TotalIndicators - FIndicators - DIndicators - BIndicators - AIndicators;

    }


}
