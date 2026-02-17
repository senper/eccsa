using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Common.Planning;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model
{
    public class PlanHeaderViewModel : MandEViewModel<Plan>
    {
        public PlanHeaderViewModel()
        {
            CurrentPage = "Financial Items";
        }
        public List<Plan> Parents { get; set; } = new List<Plan>();
        public List<Plan> RootPlans
        {
            get
            {
                return TheList.Where(i => (!i.ParentId.HasValue || i.ParentId.Value == Guid.Empty) && i.IsLabel).OrderBy(x => x.Code).ToList();
            }
        }

       
        public bool HasChildren(Guid id)
        {
            return TheList.Any(i => i.ParentId.HasValue && i.ParentId.Value == id);
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
    }
}
