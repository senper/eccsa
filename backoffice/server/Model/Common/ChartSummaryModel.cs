using System;
using System.Collections.Generic;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Common
{
    public class SummaryModel
    {
        public List<DetailModel> Details { get; set; } = new List<DetailModel>();
        public List<DetailModel> L1Summary { get; private set; } = new List<DetailModel>();
        public List<DetailModel> L2Summary { get; private set; } = new List<DetailModel>();
        public List<string> L1Values { get; private set; } = new List<string>();
        public List<string> L2Values { get; private set; } = new List<string>();
        public List<string> L3Values { get; private set; } = new List<string>();
        public string L1Label { get; set; }
        public string L2Label { get; set; }
        public string L3Label { get; set; }

        public void GenerateSummary()
        {
            L1Summary = Details.GroupBy(g => g.L1).Select(s => new DetailModel
            {
                L1 = s.Key,
                L2 = " ",
                L3 = " ",
                No = s.Sum(f => f.No),
            }).ToList();
            L1Values.Clear();
            L2Values.Clear();
            L3Values.Clear();
            foreach (var l1 in L1Summary)
            {
                L1Values.Add(l1.L1);
                l1.Details = Details.Where(l => l.L1 == l1.L1).GroupBy(g => g.L2).Select(s => new DetailModel
                {
                    L1 = l1.L1,
                    L2 = s.Key,
                    L3 = " ",
                    No = s.Sum(f => f.No),
                }).ToList();
                l1.Details2 = Details.Where(l => l.L1 == l1.L1).GroupBy(g => g.L3).Select(s => new DetailModel
                {
                    L1 = l1.L1,
                    L3 = s.Key,
                    L2 = " ",
                    No = s.Sum(f => f.No),
                }).ToList();
                foreach (var l2 in l1.Details)
                {
                    l2.Details = Details.Where(l => l.L1 == l2.L1 && l.L2 == l2.L2).ToList();
                }
                foreach (var l3 in l1.Details2)
                {
                    l3.Details2 = Details.Where(l => l.L1 == l1.L1 && l.L3 == l3.L3).ToList();
                }
            }

            L2Summary = Details.GroupBy(g => new { g.L1, g.L2 }).Select(s => new DetailModel
            {
                L1 = s.Key.L1,
                L2 = s.Key.L2,
                L3 = " ",
                No = s.Sum(f => f.No),
            }).ToList();
            L2Values.AddRange(Details.GroupBy(g => new { g.L2 }).Select(s => s.Key.L2).ToList());
            L3Values.AddRange(Details.GroupBy(g => new { g.L3 }).Select(s => s.Key.L3).ToList());
        }
    }


    public class DetailModel
    {
        public String L1 { get; set; }
        public String L2 { get; set; }

        public String L3 { get; set; }

        public long No { get; set; }

        public List<DetailModel> Details { get; set; } = new List<DetailModel>();
        public List<DetailModel> Details2 { get; set; } = new List<DetailModel>();
    }
}
