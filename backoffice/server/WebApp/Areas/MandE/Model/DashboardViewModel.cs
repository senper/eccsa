using ECCSA.BackOffice.Server.Model.Common;
using System.Collections.Generic;
using ECCSA.BackOffice.Server.Model.Common.Planning;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model
{
    public class DashboardViewModel : MandEViewModel<Plan>
    {

        public Configuration Configuration { get; set; }

        public List<FinancialPlan> List { get; set; }
        
        

        public string NextPeriodName { get
            {
                 if(Configuration.CurrentMonth < 12)
                    return MonthFullName((Configuration.CurrentMonth + 1).ToString());
                 return $"{MonthFullName("1")} {Configuration.CurrentYear + 1}";
            } }
    }
}
