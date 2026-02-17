using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.WebApp.Models;
using System.Collections.Generic;
using ECCSA.BackOffice.Server.Model.Common.Planning;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model
{
    public class MandEViewModel<T>: BaseViewModel
    {
        public MandEViewModel()
        {
            ChangeActive(MenuItemEnum.Dashboard);
            CurrentModule = ECCSAModuleFlag.MandE;
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.Header, "" },
                { MenuItemEnum.Plan, "" },
                { MenuItemEnum.ApprovePlan, "" },
                { MenuItemEnum.Progress, "" },
                { MenuItemEnum.AnnualPlan, "" },
                { MenuItemEnum.AnnualProgress, "" },
                { MenuItemEnum.FinancialItems, "" },
                { MenuItemEnum.FinancialPlan, "" },
                { MenuItemEnum.ReviewProgress, "" },
                { MenuItemEnum.ProgressReport, "" },
                { MenuItemEnum.AnnualReport, "" },
                { MenuItemEnum.ReviewPlan, "" },
                { MenuItemEnum.AggregateReport, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }


        public List<T> TheList { get; set; }

        public T TheModel { get; set; }

        public List<int> PlanYears { get; set; } = new List<int>();

        public PlanningPhaseEnum PlanPhase { get; set; }

        public int PlanVersionNo { get; set; }
        public List<int> AllPlanYears { get; set; } = new List<int>();
        public PlanningStatusEnum PlanStatus { get; set; } = PlanningStatusEnum.Planning;

        public string PlanStatusText
        {
            get
            {
                return PlanStatus switch
                {
                    PlanningStatusEnum.Planning => "Planning",
                    PlanningStatusEnum.Rejected => "Rejected",
                    PlanningStatusEnum.Submitted => "Submitted",
                    PlanningStatusEnum.Approved => "Approved",
                    PlanningStatusEnum.Completed => "Completed",
                    PlanningStatusEnum.Closed => "Closed",
                    _ => "",
                };
            }
        }



        public string PlanStatusClass
        {
            get
            {
                return PlanStatus switch
                {
                    PlanningStatusEnum.Planning => " btn-info",
                    PlanningStatusEnum.Rejected => " btn-danger",
                    PlanningStatusEnum.Submitted => " btn-primary",
                    PlanningStatusEnum.Approved => " btn-success",
                    PlanningStatusEnum.Completed => " btn-info",
                    PlanningStatusEnum.Closed => " btn-secondary",
                    _ => "",
                };
            }
        }

        public string StatusStyle(PlanningStatusEnum status)
        {
            
                return status switch
                {
                    PlanningStatusEnum.Planning => "background-color: #adb5bd ; color: white; ",
                    PlanningStatusEnum.Rejected => "background-color:#dc3545  ; color: white;",
                    PlanningStatusEnum.Submitted => "background-color: #6f42c1 ; color: white;",
                    PlanningStatusEnum.Approved => "background-color:#198754  ; color: white;",
                    PlanningStatusEnum.Completed => "background-color: #ffc107 ; color: black;",
                    PlanningStatusEnum.Closed => "background-color:#0d6efd  ; color: white;",
                    _ => "",
                };
            
        }
        public string StatusStyle(ProgressStatusEnum status)
        {

            return status switch
            {
                ProgressStatusEnum.Draft => "background-color: #adb5bd ; color: white; ",
                ProgressStatusEnum.Rejected => "background-color:#dc3545  ; color: white;",
                ProgressStatusEnum.Submitted => "background-color: #6f42c1 ; color: white;",
                ProgressStatusEnum.Approved => "background-color:#198754  ; color: white;",
                ProgressStatusEnum.Completed => "background-color: #ffc107 ; color: black;",
                ProgressStatusEnum.Closed => "background-color:#0d6efd  ; color: white;",
                _ => "",
            };

        }
        public int PlanYear { get; set; }

        

        public List<LookupValue> SupportingUnits { get; set; }
        public List<FinancialItem> FinancialItems { get; set;}
    }
}
