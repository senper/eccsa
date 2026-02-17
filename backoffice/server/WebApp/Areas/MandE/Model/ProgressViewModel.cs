using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model
{
    public class ProgressViewModel : MandEViewModel<Plan>
    {
        public ProgressViewModel()
        {
            CurrentPage = "Progress";
            SelectedPeriod = CurrentMonth.ToString();
        }

        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        public string SelectedPeriod { get; set; }

        public int SelectedYear { get; set; } = 2015;

        public UnplannedActivity CurrentActivity { get; set; } 

        //public List<Planm>
        public int PlanYear { get; set; }
        public List<Plan> Parents { get; set; } = new List<Plan>();
        public List<Plan> RootPlans
        {
            get
            {
                return TheList.Where(i => !i.ParentId.HasValue && i.IsLabel).OrderBy(x => x.Code).ToList();
            }
        }
        public bool HasChildren(Guid id)
        {
            return TheList.Any(i => i.ParentId.HasValue && i.ParentId.Value == id);
        }

        public int SelectedUnit { get; set; }

        public int SelectedScore { get; set; }
        public UnplannedActivity UnplannedActivity { get; set; }

        public List< UnplannedActivity > UnplannedActivities { get; set; }
        public List<SelectListItem> ScoreList
        {
            get
            {
                var lst = new List<SelectListItem>
                {
                    new SelectListItem(" ", $"0"),
                    new SelectListItem( $"Below {Config.YelloValue}%", Config.YelloValue.ToString()),
                    new SelectListItem( $"{Config.YelloValue}-{Config.BlueValue}",Config.BlueValue.ToString()),
                    new SelectListItem( $"{Config.BlueValue}-{Config.BlueGreenValue}",Config.BlueGreenValue.ToString()),
                    new SelectListItem( $"{Config.BlueGreenValue}-{Config.GreenValue}", Config.GreenValue.ToString()),
                    new SelectListItem( $"Above {Config.GreenValue}%", "100")
                };
                return lst;
            }
        }
        public List<SelectListItem> GetMonths(int mNo)
        {
            var lst = new List<SelectListItem>();
            for (var i = 1; i <= mNo; i++)
            {
                lst.Add(new SelectListItem(GetMonthName(i), i.ToString()));
            }
            lst.Add(new SelectListItem("Quarter I", "Q1"));
            if (mNo is > 3)
            {
                lst.Add(new SelectListItem("Semi Annual", "Q2"));
            }
            if (mNo is > 6)
            {
                lst.Add(new SelectListItem("Quarter III", "Q3"));
            }

            lst.Add(new SelectListItem("Year To Date(Annual)", "Q4"));

            return lst;
        }


        public List<SelectListItem> GetMonthsOnly(int mNo)
        {
            var lst = new List<SelectListItem>();
            for (var i = 1; i <= mNo; i++)
            {
                lst.Add(new SelectListItem(GetMonthName(i), i.ToString()));
            }

            return lst;
        }

        private string GetMonthName(int mNo)
        {
            switch (mNo)
            {
                case 1: return "Hamle (July)";
                case 2: return "Nehase (August)";
                case 3: return "Meskerem (September)";
                case 4: return "Tikimt (October)";

                case 5: return "Hidar (November)";
                case 6: return "Tahisas (December)";
                case 7: return "Tir (January)";
                case 8: return "Yekatit (February)";

                case 9: return "Megabit (March)";
                case 10: return "Miazia (April)";
                case 11: return "Ginbot (May)";
                case 12: return "Sene (June)";
            }
            return String.Empty;
        }
        public bool AreAllLabelChildren(Guid id)
        {
            var subPlan = TheList.FirstOrDefault(i => i.Id == id).SubPlans;
            return subPlan.Count > 0 && subPlan.All(i => i.IsLabel);
        }
        public List<Plan> GetSubPlans(Guid parentId)
        {
            return TheList.Where(i => i.ParentId.HasValue && i.ParentId.Value == parentId).OrderBy(x => x.Code).ToList();
        }
        public List<ScoreCard> ScoreCards { get; set; } = new List<ScoreCard>();

        public Configuration Config { get; set; }
        public List<LookupValue> SupportingUnits { get; set; }
    }
}
