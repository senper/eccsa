using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model
{
    public class PlanViewModel : MandEViewModel<Plan>
    {
        public PlanViewModel()
        {
            CurrentPage = "Planning";
        }
       
        public User TheUser { get; set; }

        public bool IsFinance
        {
            get
            {
                if (TheUser == null) return false;
                
                return TheUser.DefaultModule == ECCSAModuleFlag.Support && TheUser.DepartmentId.HasValue && TheUser.DepartmentId == Constants.Finance_SupportId;
            }
        }
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
        public bool AreAllLabelChildren(Guid id)
        {
            var subPlans = TheList.Where(i=> i.ParentId == id).ToList();
            return subPlans.Count > 0 && subPlans.All(i => i.IsLabel );
        }
        public bool IsEmptyHeader(Guid id, ECCSAModuleEnum subSystem, int supportingUnitId)
        {
            var thePlan = TheList.FirstOrDefault(i => i.Id == id);
            if (thePlan == null) return true;
            if (!thePlan.IsLabel) return false;
            if (thePlan.Code.StartsWith("1.1.0"))
            {
                var code = thePlan.Code;
            }
            var subPlan = subSystem == ECCSAModuleEnum.None? thePlan.SubPlans: thePlan.SubPlans.Where(s => (s.ResponsibleUnit == ECCSAModuleEnum.None || s.ResponsibleUnit == subSystem) && s.SupportingUnitId == supportingUnitId).ToList();
            if (subPlan.Count == 0) return true;
            foreach (var plan in subPlan)
            {
                if (!plan.IsLabel) return false;
                if (plan.SubPlans.Count > 0)
                    return IsEmptyHeader(plan.Id, subSystem, supportingUnitId);
            }

            return true;
        }
        public bool HasRejection(Guid parentId, bool first = true)
        {
            bool has = false;
            if (!first || TheList.Any(i => i.Status == PlanningStatusEnum.Rejected))
            {
                has = TheList.Any(i => i.ParentId.HasValue && i.ParentId.Value == parentId && i.Status == PlanningStatusEnum.Rejected);
                if (has)
                    return true;
                var list = GetSubPlans(parentId);
                foreach(var p in list)
                {
                    has = HasRejection(p.Id, false);
                    if (has)
                        return true;
                }
            }
            return false;
        }
        public List<Plan> GetSubPlans(Guid parentId)
        {
            return TheList.Where(i => i.ParentId.HasValue && i.ParentId.Value == parentId).OrderBy(x => x.Code).ToList();
        }
        
    }
}
