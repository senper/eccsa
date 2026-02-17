using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Common.Planning;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model
{
    public class FinancialItemViewModel : MandEViewModel<FinancialItem>
    {
        public FinancialItemViewModel()
        {
            CurrentPage = "Planning";
        }
        public int PlanYear { get; set; }
        public List<FinancialItem> Parents { get; set; } = new List<FinancialItem>();
        public List<FinancialItem> RootPlans
        {
            get
            {
                return TheList.Where(i => !i.ParentId.HasValue).OrderBy(x => x.Code).ToList();
            }
        }
        public bool HasChildren(Guid id)
        {
            return TheList.Any(i => i.ParentId.HasValue && i.ParentId.Value == id);
        }
        public bool AreAllLabelChildren(Guid id)
        {
            var subPlan = TheList.FirstOrDefault(i => i.Id == id).SubItems;
            return subPlan.Count > 0 && subPlan.All(i => i.IsLabel);
        }
        public List<FinancialItem> GetSubItems(Guid parentId)
        {
            return TheList.Where(i => i.ParentId.HasValue && i.ParentId.Value == parentId).OrderBy(x => x.Code).ToList();
        }
        public bool ShowActive { get; set; } = true;
        public bool ShowInactive { get; set; } = false;
    }

    public class FinancialPlanViewModel : MandEViewModel<FinancialPlan>
    {
        public FinancialPlanViewModel()
        {
            CurrentPage = "Planning";
        }
        public int CurrentMonth { get; set; }
        public int CurrentYear { get; set; }

        public int PlanYear { get; set; }
        public List<FinancialItem> Items { get; set; } = new List<FinancialItem>();

        public List<FinancialItem> Parents { get; set; } = new List<FinancialItem>();
        public List<FinancialItem> RootPlans
        {
            get
            {
                return Items.Where(i => !i.ParentId.HasValue).OrderBy(x => x.Code).ToList();
            }
        }
        public bool HasChildren(Guid id)
        {
            return Items.Any(i => i.ParentId.HasValue && i.ParentId.Value == id);
        }
        public bool AreAllLabelChildren(Guid id)
        {
            var subPlan = Items.FirstOrDefault(i => i.Id == id).SubItems;
            return subPlan.Count > 0 && subPlan.All(i => i.IsLabel);
        }
        public List<FinancialItem> GetSubItems(Guid parentId)
        {
            return Items.Where(i => i.ParentId.HasValue && i.ParentId.Value == parentId).OrderBy(x => x.Code).ToList();
        }
    }

    public class QuarterlyPlan
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public int PlanYear { get; set; }
        public decimal Q1 { get; set; }
        public decimal Q2 { get; set; }
        public decimal Q3 { get; set; }
        public decimal Q4 { get; set; }



        public decimal Annual => Q1 + Q2 + Q3 + Q4;
    }

    
}
