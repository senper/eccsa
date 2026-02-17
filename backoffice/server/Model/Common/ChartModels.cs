using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Common
{

    public class BarChartModel
    {
        public string key { get; set; }
        public string color { get; set; }
        public List<BarChartValues> values { get; set; }
    }

    public class BarChartValues
    {
        public decimal y { get; set; }
        public decimal x { get; set; }
    }

    public class LineCustomChartModel
    {
        public string key { get; set; }
        public string color { get; set; }
        public List<LineCustomChartValues> values { get; set; }
    }

    public class LineCustomChartValues
    {
        public decimal y { get; set; }
        public decimal x { get; set; }
        public string shape { get; set; }
        public decimal size { get; set; }
    }

    public class BarChartModelStringX
    {
        public string key { get; set; }
        public string color { get; set; }
        public List<BarChartValuesStringX> values { get; set; }
    }

    public class BarChartValuesStringX
    {
        public decimal y { get; set; }
        public string x { get; set; }

        public string label { get { return x; } set { x = value; } }

        public decimal value { get { return y; } set { y = value; } }
    }

    public class AreaChartModel
    {
        public string key { get; set; }
        public List<List<decimal>> values { get; set; }
    }
    public class PieModel
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "y")]
        public double Y { get; set; }

        public string Category { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public decimal Value { get; set; }
        public decimal Value1 { get; set; }
        public decimal Value2 { get; set; }
        public decimal Value3 { get; set; }
    }

    public class BarWithLineChartModel
    {
        public string key { get; set; }
        public bool bar { get; set; }
        public List<List<decimal>> values { get; set; }
    }
    public class TreeData
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }

        [JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }

        [JsonProperty(PropertyName = "children")]
        public List<TreeData> Children { get; set; }

        [JsonProperty(PropertyName = "x0")]
        public int X0 { get; set; }

        [JsonProperty(PropertyName = "y0")]
        public int Y0 { get; set; }


    }
    public class ChartDataModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Parent { get; set; }

        public TreeData ConvertToTreeData(List<ChartDataModel> dataModels, string rootId = "0", string parentName = null)
        {
            var rootData = dataModels.FirstOrDefault(d => d.Id == rootId);
            var rootChilds = dataModels.Where(d => d.Parent == rootId).ToList();

            if (rootData == null)
                return null;
            var treeData = new TreeData
            {
                Name = rootData.Name,
                Description = rootData.Description,
                Value = int.Parse(rootData.Value),
                Parent = parentName,
                Children = rootChilds.Any() ? new List<TreeData>() : null
            };

            foreach (var rc in rootChilds)
            {
                var ctreeData = dataModels.Any(d => d.Parent == rc.Id) ?
                    ConvertToTreeData(dataModels, rc.Id, treeData.Name) :
                    new TreeData
                    {
                        Name = rc.Name,
                        Description = rc.Description,
                        Value = int.Parse(rc.Value),
                        Parent = treeData.Name
                    };
                treeData.Children.Add(ctreeData);
            }

            return treeData;
        }
    }
    public class FlatChartDataModel
    {
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Value { get; set; }

        public List<ChartTreeAreaModel> ConvertToTreeArea(List<FlatChartDataModel> dataModels)
        {
            var resData = new List<ChartTreeAreaModel>();

            foreach (var d in dataModels)
            {
                resData.Add(new ChartTreeAreaModel
                {
                    Key = d.Level3,
                    Subregion = d.Level2,
                    Region = d.Level1,
                    Value = long.Parse(d.Value)
                });
            }

            return resData;
        }

        //public List<ChartTreeAreaModel> ConvertToTreeArea<T>(List<T> dataModels)
        //{
        //    var resData = new List<ChartTreeAreaModel>();

        //    foreach (var d in dataModels)
        //    {
        //        resData.Add(new ChartTreeAreaModel
        //        {
        //            Key = d.SubRegion,
        //            Subregion = d.SubRegion,
        //            Region = d.Region,
        //            Value = long.Parse(d.Count.ToString())
        //        });
        //    }

        //    return resData;
        //}
        //public List<ChartTreeAreaModel> ConvertToTreeArea(SummaryModel dataModel)
        //{
        //    var resData = new List<ChartTreeAreaModel>();

        //    foreach (var d in dataModel.Details)
        //    {
        //        resData.Add(new ChartTreeAreaModel
        //        {
        //            Key = d.L3,
        //            Subregion = d.L2,
        //            Region = d.L1,
        //            Value = d.No
        //        });
        //    }

        //    return resData;
        //}
        public List<PieModel> ConvertToPieModel(List<FlatChartDataModel> dataModels)
        {
            var resData = new List<PieModel>();

            foreach (var d in dataModels)
            {
                resData.Add(new PieModel
                {
                    Key = d.Level1,
                    Y = double.Parse(d.Value)
                });
            }

            return resData;
        }

        public List<BarChartModelStringX> ConvertToBarModel(List<FlatChartDataModel> dataModels, string key)
        {
            var lst = new List<BarChartModelStringX>();
            var model = new BarChartModelStringX
            {
                key = key,
                values = new List<BarChartValuesStringX>()
            };
            foreach (var fm in dataModels.OrderBy(r => r.Level1))
            {
                model.values.Add(new BarChartValuesStringX
                {
                    x = fm.Level1,
                    y = decimal.Parse(fm.Value)
                });
            }
            lst.Add(model);
            return lst;
        }
        public List<SpatialDataModel> ConvertToSpatialModel(List<FlatChartDataModel> dataModels)
        {
            var resData = new List<SpatialDataModel>();
            if (dataModels == null || dataModels.Count == 0)
                return resData;
            foreach (var d in dataModels.OrderBy(d => int.Parse(d.Level1 ?? "0")))
            {
                resData.Add(new SpatialDataModel
                {
                    Key = d.Level1,
                    Value = long.Parse(d.Value),
                    LocationName = d.Level3
                });
            }

            return resData;
        }
        public List<int> ConvertToIntList(List<FlatChartDataModel> dataModels)
        {
            return dataModels.OrderBy(d => int.Parse(d.Level1)).Select(d => int.Parse(d.Value)).ToList();
        }
    }

    public class SunBurstL3Model
    {
        public string name { get; set; }

        public List<SunBurstValueModel> children { get; set; }
    }

    public class SunBurstL1Model
    {
        public string name { get; set; }

        public List<SunBurstL2Model> children { get; set; }
    }
    public class SunBurstL2Model
    {
        public string name { get; set; }

        public List<SunBurstL3Model> children { get; set; }
    }

    public class SunBurstValueModel
    {
        public string name { get; set; }

        public decimal size { get; set; }
    }

    public class SunBurstModel
    {
        public string name { get; set; }

        public decimal size { get; set; }

        public List<SunBurstModel> children { get; set; }


        //public List<PieModel> ConvertToPieModel(int level, string l1, string l2, string l3,
        //    List<RecruitmentReport> lst)
        //{
        //    if (!string.IsNullOrEmpty(l3))
        //        level = 4;
        //    else if (!string.IsNullOrEmpty(l2))
        //        level = 3;
        //    //else
        //    //{
        //    //    level = 2;
        //    //}
        //    var resData = new List<PieModel>();
        //    List<RecruitmentReport> lst2;
        //    IEnumerable<IGrouping<string, RecruitmentReport>> detail;
        //    switch (level)
        //    {
        //        case 2:
        //            lst2 = lst.Where(l => l.PositionType.Trim()==l1.Trim()).ToList();
        //            detail = lst2.GroupBy(l => l.Sex);
        //            resData.AddRange(detail.Select(d => new PieModel
        //            {
        //                Key = d.Key.Trim(),
        //                Y = (double)d.Count()
        //            }));
        //            return resData;

        //        case 3:
        //            lst2 = lst.Where(l => l.PositionType.Trim() == l1.Trim() && l.Sex == l2).ToList();
        //            detail = lst2.GroupBy(l => l.AgeGroup);
        //            resData.AddRange(detail.Select(d => new PieModel
        //            {
        //                Key = d.Key.Trim(),
        //                Y = (double)d.Count()
        //            }));
        //            return resData;
        //        default:

        //            lst2 = lst.ToList();
        //            detail = lst2.GroupBy(l => l.PositionType);
        //            resData.AddRange(detail.Select(d => new PieModel
        //            {
        //                Key = d.Key.Trim(),
        //                Y = (double)d.Count()
        //            }));
        //            return resData;

        //    }

        //}

        //public List<PieModel> ConvertToPieModel(int level, string l1, string l2, string l3,
        //    SummaryModel model)
        //{
        //    if (!string.IsNullOrEmpty(l3))
        //        level = 4;
        //    else if (!string.IsNullOrEmpty(l2))
        //        level = 3;
        //    //else
        //    //{
        //    //    level = 2;
        //    //}
        //    var resData = new List<PieModel>();
        //    switch (level)
        //    {
        //        case 2:

        //            resData.AddRange(model.L1Summary.Find(l=> l.L1 == l1).Details.Select(d => new PieModel
        //            {
        //                Key = d.L2,
        //                Y = d.No
        //            }));
        //            return resData;

        //        case 3:
        //            resData.AddRange(model.L1Summary.Find(l => l.L1 == l1)
        //                .Details.Find(l => l.L2 == l2)
        //                .Details.Select(d => new PieModel
        //            {
        //                Key = d.L1,
        //                Y = d.No
        //            }));
        //            return resData;
        //        default:

        //            resData.AddRange(model.L1Summary.Select(d => new PieModel {
        //                Key = d.L1,
        //                Y = d.No
        //            }));
        //            return resData;
        //    }

        //}
        //public List<PieModel> ConvertToPieModel(int level, string l1, string l2, string l3,
        //    List<WorkersSummaryByPostionModel> lst)
        //{
        //    if (!string.IsNullOrEmpty(l3))
        //        level = 4;
        //    else if (!string.IsNullOrEmpty(l2))
        //        level = 3;
        //    //else
        //    //{
        //    //    level = 2;
        //    //}
        //    var resData = new List<PieModel>();
        //    List<WorkersSummaryByPostionModel> lst2;
        //    IEnumerable<IGrouping<string, WorkersSummaryByPostionModel>> detail;
        //    switch (level)
        //    {
        //        case 2:
        //            lst2 = lst.Where(l => l.Position.PositionType.Title.Trim() == l1.Trim()).ToList();
        //            detail = lst2.GroupBy(l => l.Worker.Sex);
        //            resData.AddRange(detail.Select(d => new PieModel
        //            {
        //                Key = d.Key.Trim(),
        //                Y = (double)d.Count()
        //            }));
        //            return resData;

        //        case 3:
        //            lst2 = lst.Where(l => l.Position.PositionType.Title.Trim() == l1.Trim() && l.Worker.Sex == l2).ToList();
        //            detail = lst2.GroupBy(l => l.AgeGroup());
        //            resData.AddRange(detail.Select(d => new PieModel
        //            {
        //                Key = d.Key.Trim(),
        //                Y = (double)d.Count()
        //            }));
        //            return resData;
        //        default:

        //            lst2 = lst.ToList();
        //            detail = lst2.GroupBy(l => l.Position.PositionType.Title);
        //            resData.AddRange(detail.Select(d => new PieModel
        //            {
        //                Key = d.Key.Trim(),
        //                Y = (double)d.Count()
        //            }));
        //            return resData;

        //    }

        //}
        //public List<PieModel> ConvertToPieModel(List<ImportExportProductSummary> lst)
        //{
        //    var resData = new List<PieModel>();

        //    foreach (var d in lst.GroupBy(l => l.Level1))
        //    {
        //        resData.Add(new PieModel
        //        {
        //            Key = d.Key.Trim().Substring(0, 3),
        //            Y = (double)lst.Where(v => v.Level1 == d.Key).Sum(v => v.Value)
        //        });
        //    }

        //    return resData;
        //}
        //public List<SunBurstL1Model> ToSunBurstModel(List<ImportExportProductSummary> lst, string key)
        //{
        //    var l1Ids = lst.Where(l => l.Level1.StartsWith(key)).Select(l => new { l.Level1Id, l.Level1 }).Distinct().ToList();
        //    var l2Ids = lst.Select(l => new { l.Level1Id, l.Level2Id, l.Level2 }).Distinct().ToList();
        //    var l3Ids = lst.Select(l => new { l.Level2Id, l.Level3Id, l.Level3 }).Distinct().ToList();
        //    List<SunBurstL1Model> destLst = new List<SunBurstL1Model>();
        //    foreach (var id1 in l1Ids)
        //    {
        //        var l1 = new SunBurstL1Model
        //        {
        //            name = id1.Level1,
        //            children = new List<SunBurstL2Model>()
        //        };
        //        foreach (var id2 in l2Ids.Where(l => l.Level1Id == id1.Level1Id))
        //        {
        //            var l2 = new SunBurstL2Model
        //            {
        //                name = id2.Level2,
        //                children = new List<SunBurstL3Model>()
        //            };
        //            foreach (var id3 in l3Ids.Where(l => l.Level2Id == id2.Level2Id))
        //            {
        //                var l3 = new SunBurstL3Model
        //                {
        //                    name = id3.Level3,
        //                    children = new List<SunBurstValueModel>()
        //                };

        //                foreach (var val in lst.Where(l => l.Level3Id == id3.Level3Id))
        //                {
        //                    var sb = new SunBurstValueModel
        //                    {
        //                        name = val.Level4,
        //                        size = val.Value
        //                    };
        //                    l3.children.Add(sb);
        //                }
        //                l2.children.Add(l3);
        //            }
        //            l1.children.Add(l2);
        //        }
        //        destLst.Add(l1);
        //    }

        //    return destLst;
        //}
    }

    public class SpatialDataModel
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "locationName")]
        public string LocationName { get; set; }

        [JsonProperty(PropertyName = "value")]
        public long Value { get; set; }
    }
    public class ChartTreeAreaModel
    {

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }


        [JsonProperty(PropertyName = "subregion")]
        public string Subregion { get; set; }



        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }


        [JsonProperty(PropertyName = "value")]
        public long Value { get; set; }
    }
}
