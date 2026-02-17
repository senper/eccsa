using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class IssueReport
    {
        public string IssueType { get; set; }
        public int Resolved { get; set; }
        public int UnresolvedCurrent { get; set; }
        public int Unresolved30Days { get; set; }
        public int Unresolved60Days { get; set; }
        public int TotalCases => Resolved + Unresolved60Days + Unresolved30Days + UnresolvedCurrent;
        public static List<IssueReport> FakeData(List<IssueType> issueTypes)
        {
            var list = new List<IssueReport>();
            foreach(var issueType in issueTypes)
            {
                var r = new Random();
                list.Add(new IssueReport
                {
                    IssueType = issueType.Name,
                    Resolved = r.Next(1, 20),
                    UnresolvedCurrent = r.Next(1, 6),
                    Unresolved30Days = r.Next(1, 4),
                    Unresolved60Days = r.Next(1, 3)
                });
            }
            return list;
        }
    }
    public class MultiHorizontalBarChartModel
    {
        public string key { get; set; }
        public string color { get; set; }
        public List<MultiHorizontalBarChartDetailModel> values { get; set; }
        public static List<MultiHorizontalBarChartModel> FromIssueReportList(List<IssueReport> data)
        {
            var resolved = new MultiHorizontalBarChartModel
            {
                key = "Resolved",
                color = "#00FF00",
                values = new List<MultiHorizontalBarChartDetailModel>()
            };
            var unresolvedCurrent = new MultiHorizontalBarChartModel
            {
                key = "Unresolved Current",
                color = "#4f99b4",
                values = new List<MultiHorizontalBarChartDetailModel>()
            };
            var unresolved30Days = new MultiHorizontalBarChartModel
            {
                key = "Unresolved 30 Days",
                color = "#FFA500",
                values = new List<MultiHorizontalBarChartDetailModel>()
            };
            var unresolved60Days = new MultiHorizontalBarChartModel
            {
                key = "Unresolved 60 Days",
                color = "#FF0000",
                values = new List<MultiHorizontalBarChartDetailModel>()
            };
            foreach (var issue in data)
            {
                resolved.values.Add(new MultiHorizontalBarChartDetailModel
                {
                    label = issue.IssueType,
                    value = issue.Resolved
                });
                unresolvedCurrent.values.Add(new MultiHorizontalBarChartDetailModel
                {
                    label = issue.IssueType,
                    value = issue.UnresolvedCurrent
                });
                unresolved30Days.values.Add(new MultiHorizontalBarChartDetailModel
                {
                    label = issue.IssueType,
                    value = issue.Unresolved30Days
                });
                unresolved60Days.values.Add(new MultiHorizontalBarChartDetailModel
                {
                    label = issue.IssueType,
                    value = issue.Unresolved60Days
                });
            }
            return new List<MultiHorizontalBarChartModel> {
                resolved, unresolvedCurrent, unresolved30Days, unresolved60Days };
        }
    }
    public class MultiHorizontalBarChartDetailModel
    {
        public string label { get; set; }
        public decimal value { get; set; }
    }
    public class Issue
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public virtual IssueType IssueType { get; set; }
        public String CompanyName { get; set; }
        public String DetailsOfOccurrence { get; set; }
        public DateTime IssueDate { get; set; }
        public String MeasureCode { get; set; }
        public String PlaceOfOccurrence { get; set; }
        public DateTime DateAssigned { get; set; }
        public string Remark { get; set; }
        public String ActionTaken { get; set; }
        public IssueStatus Status { get; set; }
        public ECCSAModuleFlag ActionOwner { get; set; }
        public DateTime DateActionTaken { get; set; }
        public DateTime DateOfOccurrence { get; set; }
        public string CountryRegion { get; set; }
        public Guid? TypeOfLocationId { get; set; }
        public Guid? LocationId { get; set; }
        public string OtherLocation { get; set; }
        public virtual IssueLocation Location { get; set; }
        public String Description { get; set; }
        public String AffectedProducts { get; set; }
        public String ProductTariffCode { get; set; }
        public String DescriptionOfProducts { get; set; }
        public String ProductValueCurrency { get; set; }
        public Decimal ProductValueAmount { get; set; }
        public Guid? PreferredHandlerId { get; set; }
        public Guid UserId { get; set; }


        [NotMapped]
        public string PreferredHandler { get; set; }

        [NotMapped]
        public string LocationType { get; set; }

        [NotMapped]
        public string LocationName { get; set; }

        [NotMapped]
        public string AssignedTo
        {
            get
            {
                switch (ActionOwner)
                {
                    case ECCSAModuleFlag.Academy:
                        return "Chamber Academy";
                    case ECCSAModuleFlag.Administration:
                        return "IT";
                    case ECCSAModuleFlag.Arbitration:
                        return "Arbitration";
                    case ECCSAModuleFlag.Communication:
                        return "PR & Communication";
                    case ECCSAModuleFlag.Investment:
                    case ECCSAModuleFlag.TradePromotion:
                        return "Investment & Trade Promotion";
                    case ECCSAModuleFlag.Membership:
                        return "Membership";
                    case ECCSAModuleFlag.Research:
                        return "Research & Adocacy";
                    default:
                        return "None";
                }
            }
        }

        public virtual ICollection<IssueAttachment> Attachments { get; set; }
    }
    public class IssueAttachment
    {
        public Guid Id { get; set; }
        public Guid IssueId { get; set; }
        public virtual Issue Issue { get; set; }
        public String Title { get; set; }
        public String Extension { get; set; }
    }
    public class IssueLocation
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public string CountryRegion { get; set; }
        public Guid? TypeOfLocationId { get; set; }
        [NotMapped]
        public string LocationType { get; set; } = string.Empty;
        public virtual ICollection<Issue> Issues { get; set; }
    }
    public class IssueSummary
    {
        public string Type { get; set; }
        public IssueStatus Status { get; set; }
        public int Count { get; set; }
        public string FormattedCount => $"{Count:G}";
    }
}
