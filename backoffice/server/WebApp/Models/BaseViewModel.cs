using ECCSA.BackOffice.Server.Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Common.Planning;

namespace ECCSA.BackOffice.Server.WebApp.Models
{
    public class BaseViewModel
    {
        public int ActiveTabIndex { get; set; } = 0;
        public string Tab0Active => ActiveTabIndex == 0 ? "active" : "";
        public string Tab1Active => ActiveTabIndex == 1 ? "active" : "";
        public string Tab2Active => ActiveTabIndex == 2 ? "active" : "";
        public string Tab3Active => ActiveTabIndex == 3 ? "active" : "";
        public string Tab4Active => ActiveTabIndex == 4 ? "active" : "";
        public string Tab5Active => ActiveTabIndex == 5 ? "active" : "";

        public string Tab6Active => ActiveTabIndex == 6 ? "active" : "";
        public string Tab7Active => ActiveTabIndex == 7 ? "active" : "";
        public string Tab8Active => ActiveTabIndex == 8 ? "active" : "";
        public string Tab9Active => ActiveTabIndex == 9 ? "active" : "";
        public String FullName { get; set; }

        public String CurrentPage { get; set; }

        public String CurrentOperation { get; set; }

        public RoleLevelEnum RoleLevel { get; set; } = RoleLevelEnum.None;

        public bool IsUserOrAdmin => RoleLevel == RoleLevelEnum.User || RoleLevel == RoleLevelEnum.Admin;

        public bool IsAdmin => RoleLevel == RoleLevelEnum.Admin;

        public ECCSAModuleEnum CurrentSubsystem { get; set; } = ECCSAModuleEnum.None;

        public string PhotoPath { get; set; } = "/assets/img/membership/person.png";

        public Dictionary<MenuItemEnum, string> MenuItem { get; set; } = new Dictionary<MenuItemEnum, string>();
        public const string MenuItemActive = " active ";
        public string GetMenuItemStatus(MenuItemEnum item)
        {
            if (!MenuItem.ContainsKey(item)) return "";
            return MenuItem[item];
        }


        public string Guid2String(Guid id) => id.ToString()[^15..].Replace("-", "");// Convert.ToBase64String(id.ToByteArray()).Replace("==", "").Replace("+","");

        public ECCSAModuleFlag CurrentModule { get; set; }
        public ECCSAModuleEnum SelectedResponsibleUnit { get; set; }
        public string ResponsibleUnitFilter { get; set; }

        public PlanningStatusEnum StatusFilter { get; set; } = PlanningStatusEnum.Submitted;
        public int SupportingUnitId { get; set; }
        public bool TreeView { get; set; } = false;
        public string LayoutPage
        {
            get
            {

                return CurrentModule switch
                {
                    ECCSAModuleFlag.Academy => "~/Views/Shared/_AcademyLayout.cshtml",
                    ECCSAModuleFlag.Arbitration => "~/Views/Shared/_ArbitrationLayout.cshtml",
                    ECCSAModuleFlag.Investment => "~/Views/Shared/_InvestmentLayout.cshtml",
                    ECCSAModuleFlag.Membership => "~/Views/Shared/_MembershipLayout.cshtml",
                    ECCSAModuleFlag.TradePromotion => "~/Views/Shared/_TradeLayout.cshtml",
                    ECCSAModuleFlag.Research => "~/Views/Shared/_AdvocacyLayout.cshtml",
                    ECCSAModuleFlag.Communication => "~/Views/Shared/_CommunicationLayout.cshtml",
                    ECCSAModuleFlag.Administration => "~/Views/Shared/_ITLayout.cshtml",
                    ECCSAModuleFlag.MandE => "~/Views/Shared/_MandELayout.cshtml",
                    ECCSAModuleFlag.Support => "~/Views/Shared/_SupportLayout.cshtml",
                    ECCSAModuleFlag.Bmo => "~/Views/Shared/_BmoLayout.cshtml",
                    _ => "~/Views/Shared/_ServiceLayout.cshtml",
                };
            }
        }

        public string PageTitle
        {
            get
            {

                return CurrentModule switch
                {
                    ECCSAModuleFlag.Academy => "Chamber Academy",
                    ECCSAModuleFlag.Arbitration => "Arbitration",
                    ECCSAModuleFlag.Communication => "Communication",
                    ECCSAModuleFlag.Investment => "Trade Promotion",
                    ECCSAModuleFlag.Membership => "Membership",
                    ECCSAModuleFlag.Reporting => "Reporting",
                    ECCSAModuleFlag.Research => "Research & Advocacy",
                    ECCSAModuleFlag.TradePromotion => "Trade Facilitation",
                    ECCSAModuleFlag.Administration => "User Administration",
                    ECCSAModuleFlag.MandE => "M&E",
                    ECCSAModuleFlag.Bmo => "BMO",
                    _ => "Other Service",
                };
            }
        }
    
        public List<Performance> Performances { get; set; }
        public String CurrentPeriodName { get; private set; }

        internal void ToPerformance(List<Plan> plans, Configuration setting)
        {
            CurrentPeriodName = $"{MonthFullName(setting.CurrentMonth.ToString())} , {setting.CurrentYear}";
            Performances = new List<Performance>();
            foreach (var p in plans)
            {
                var pm = p.PlanMonth(setting.CurrentMonth);
                if (pm == MonthlyPlan.Default) continue;
                var b = pm.Plan;
                var a = pm.Actual;
                if (b == 0) continue;
                var c =  (int)decimal.Round( (a / b) * 100,0);
                Performances.Add(new Performance()
                {
                    Name = p.NameWithCode,
                    Value = c,
                    Percentage = (c * 100).ToString()
                });
            }
        }
        public string MonthName(int monthNo)
        {
            switch (monthNo)
            {
                case 1: return "Ham";
                case 2: return "Neh";
                case 3: return "Mes";
                case 4: return "Tik";
                case 5: return "Hid";
                case 6: return "Tah";
                case 7: return "Tir";
                case 8: return "Yek";
                case 9: return "Meg";
                case 10: return "Mia";
                case 11: return "Gin";
                case 12: return "Sen";
                default: return string.Empty;
            }
        }
        public string MonthFullName(string monthNo)
        {
            int mNo;
            if (string.IsNullOrEmpty(monthNo)) return string.Empty;
            if (monthNo.StartsWith("Q"))
            {
                int.TryParse(monthNo.Replace("Q", ""), out mNo);
                mNo += 12;
            }
            else
            {
                int.TryParse(monthNo, out mNo);
            }
            switch (mNo)
            {
                case 1: return "Hamle";
                case 2: return "Nehase";
                case 3: return "Meskerem";
                case 4: return "Tikimit";
                case 5: return "Hidar";
                case 6: return "Tahisas";
                case 7: return "Tir";
                case 8: return "Yekatit";
                case 9: return "Megabit";
                case 10: return "Miazia";
                case 11: return "Ginbot";
                case 12: return "Sene";
                case 13: return "Quarter I";
                case 14: return "Quarter II";
                case 15: return "Quarter III";
                case 16: return "Quarter IV";
                default: return string.Empty;
            }
        }
    }
    public class Performance
    {
        const string GrayColor = "#e9ecef!important;";
        const string RedColor = "#dc3545!important;";
        const string YelloColor = "#ffc107!important;";
        const string BlueColor = "#007bff!important;";
        const string BlueGreenColor = "#17a2b8!important;";
        const string GreenColor = "#28a745!important;";
        public string Name { get; set; }
        public int Value { get; set; }
        public int BarWidth => Value == 0 ? 100 : Value;
        public string Percentage { get; set; }
        public string Color
        {
            get
            {
                switch (Value)
                {
                    case < 21:
                        return RedColor;
                    case < 50:
                        return YelloColor;
                    case < 65:
                        return BlueColor;
                    case < 80:
                        return BlueGreenColor;
                    default:
                        return GreenColor;

                }

            }
        }
    }
    public static class Constants
    {
        public static  Guid MembershipBoardChairmanId => Persistence.Repositories.Constants.MembershipBoardChairmanId;
        public static Guid MembershipCEOId => Persistence.Repositories.Constants.MembershipCEOId;
        public static Guid EthiopiaId => Persistence.Repositories.Constants.EthiopiaId;
        public static Guid ECCSAId => Persistence.Repositories.Constants.ECCSA;

        public static  Guid SupportingUnitLookupTypeId = Persistence.Repositories.Constants.SupportingUnitLookupTypeId;
    }

    public enum MenuItemEnum
    {
        AcademyDashboard = 0,
        AcademyTopicCategory = 1,
        AcademyTopic = 2,
        AcademyTraining = 3,
        AcademyRegistration = 4,
        AcademyAssessment = 5,
        AcademySponsors = 6,
        AcademySettingVariables = 7,

        ArbitrationDashboard = 10,
      

        ESWRequest = 13,

        Newsletter = 19,
        Resource = 20,
        CommunicationCampaign = 21,
        CommunicationMediaAgency = 22,
        CommunicationMediaCoverage = 23,
        WebsiteMenu = 24,
        WebsiteQuickLink = 25,
        CommunicationLaws = 26,
        CommunicationTextMedia = 27,
        WebsiteContent = 28,
        CommunicationAdvertisment = 29,
        CommunicationMediaType = 30,

        InvestmentBusiness = 31,
        RegionalProfile = 32,
        CountryProfile = 33,
        BusinessSector = 35,
        WebsiteTranslation = 36,

        CommunicationPhotoAlbum = 37,
        CommunicationVideoGallery = 38,

        BusinessInterestSource = 39,
        BusinessInterestProduct = 40,
        News = 41,

        Association = 42,
        Individual = 43,
        Applicant = 44,
        Issue = 45,
        Meeting = 46,
        MembershipTrade = 47,
        IssueReport = 48,
        IssueType = 49,

        AdvocacyDialog = 52,
        AdvocacyResearch = 53,
        AdvocacyPublication = 54,
        AdvocacyProject = 55,
        Bmo = 56,
        BmoType = 57,
        AnnualReport = 60,
        ProgressReport = 85,


        FooterMenu = 86,
        AggregateReport = 87,

        Header = 91,
        Plan = 92,
        Progress = 93,
        ReviewProgress = 94,
        AnnualProgress = 95,
        AnnualPlan = 96,
        FinancialItems = 97,
        FinancialPlan = 98,
        ApprovePlan = 99,

        ReviewPlan = 100,
        SurveySetting = 141,
        Survey = 142,


        Dashboard = 301,
        ServiceType = 302,
        Services = 303,

        EventType = 304,
        Events = 305,
        Appointments = 306,

        MemberUsers = 71,
        Users = 72,
        SMS = 73,
        EmailMessage = 74,
        RecipientSource = 75,
        ExternalData = 76,
        EmailTemplate = 77,
        PublicUser = 78,

        DownloadReport = 79,

        Lookups = 80,
        IssueLocation= 81,
        CommunicationContactUs = 82,
        BankAccountApprovalNeeds = 83,
        FeeApprovalNeeds = 84,
        ReportSetup = 815,
    }


#pragma warning disable IDE1006 // Naming Styles
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

        public List<PieModel> ConvertToPieModel(int level, string l1, string l2, string l3,
            List<ImportExportProductSummary> lst)
        {
            if (!string.IsNullOrEmpty(l3))
                level = 4;
            else if (!string.IsNullOrEmpty(l2))
                level = 3;
            else
            {
                level = 2;
            }
            var resData = new List<PieModel>();
            List<ImportExportProductSummary> lst2;
            IEnumerable<IGrouping<string, ImportExportProductSummary>> detail;
            switch (level)
            {
                case 2:
                    lst2 = lst.Where(l => l.Level1.StartsWith(l1)).ToList();
                    detail = lst2.GroupBy(l => l.Level2);
                    resData.AddRange(detail.Select(d => new PieModel
                    {
                        Key = d.Key.Trim(),
                        Y = (double)lst.Where(v => v.Level2 == d.Key).Sum(v => v.Value)
                    }));
                    return resData;

                case 3:
                    lst2 = lst.Where(l => l.Level1.StartsWith(l1) && l.Level2.StartsWith(l2)).ToList();
                    detail = lst2.GroupBy(l => l.Level3);
                    resData.AddRange(detail.Select(d => new PieModel
                    {
                        Key = d.Key.Trim(),
                        Y = (double)lst.Where(v => v.Level3 == d.Key).Sum(v => v.Value)
                    }));
                    return resData;
                default:

                    lst2 = lst.Where(l => l.Level1.StartsWith(l1) && l.Level2.StartsWith(l2) && l.Level3.StartsWith(l3)).ToList();
                    detail = lst2.GroupBy(l => l.Level4);
                    resData.AddRange(detail.Select(d => new PieModel
                    {
                        Key = d.Key.Trim(),
                        Y = (double)lst.Where(v => v.Level4 == d.Key).Sum(v => v.Value)
                    }));
                    return resData;

            }

        }
        public List<PieModel> ConvertToPieModel(List<ImportExportProductSummary> lst)
        {
            var resData = new List<PieModel>();

            foreach (var d in lst.GroupBy(l => l.Level1))
            {
                resData.Add(new PieModel
                {
                    Key = d.Key.Trim()[..3],
                    Y = (double)lst.Where(v => v.Level1 == d.Key).Sum(v => v.Value)
                });
            }

            return resData;
        }
        public List<SunBurstL1Model> ToSunBurstModel(List<ImportExportProductSummary> lst, string key)
        {
            var l1Ids = lst.Where(l => l.Level1.StartsWith(key)).Select(l => new { l.Level1Id, l.Level1 }).Distinct().ToList();
            var l2Ids = lst.Select(l => new { l.Level1Id, l.Level2Id, l.Level2 }).Distinct().ToList();
            var l3Ids = lst.Select(l => new { l.Level2Id, l.Level3Id, l.Level3 }).Distinct().ToList();
            List<SunBurstL1Model> destLst = new ();
            foreach (var id1 in l1Ids)
            {
                var l1 = new SunBurstL1Model
                {
                    name = id1.Level1,
                    children = new List<SunBurstL2Model>()
                };
                foreach (var id2 in l2Ids.Where(l => l.Level1Id == id1.Level1Id))
                {
                    var l2 = new SunBurstL2Model
                    {
                        name = id2.Level2,
                        children = new List<SunBurstL3Model>()
                    };
                    foreach (var id3 in l3Ids.Where(l => l.Level2Id == id2.Level2Id))
                    {
                        var l3 = new SunBurstL3Model
                        {
                            name = id3.Level3,
                            children = new List<SunBurstValueModel>()
                        };

                        foreach (var val in lst.Where(l => l.Level3Id == id3.Level3Id))
                        {
                            var sb = new SunBurstValueModel
                            {
                                name = val.Level4,
                                size = val.Value
                            };
                            l3.children.Add(sb);
                        }
                        l2.children.Add(l3);
                    }
                    l1.children.Add(l2);
                }
                destLst.Add(l1);
            }

            return destLst;
        }
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
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }
        [JsonProperty(PropertyName = "subregion")]
        public string Subregion { get; set; }
        [JsonProperty(PropertyName = "value")]
        public long Value { get; set; }
    }

    public class ImportExportProductSummary
    {
        [NotMapped]
        public string name { get; set; }

        [NotMapped]
        public decimal size { get; set; }

        public List<ImportExportProductSummary> children { get; set; }
        public int Id { get; set; }

        public decimal Value { get; set; }

        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Level4 { get; set; }

        public int Level1Id { get; set; }
        public int Level2Id { get; set; }
        public int Level3Id { get; set; }

    }
#pragma warning restore IDE1006 // Naming Styles
}
