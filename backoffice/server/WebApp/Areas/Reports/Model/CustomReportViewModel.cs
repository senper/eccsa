using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Report.Model
{

    public class CustomReportViewModel : BaseViewModel
    {
        public CustomReportViewModel()
        {
            ChangeActive(MenuItemEnum.DownloadReport);
        }
        protected void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.DownloadReport, "" }
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }

        public Dictionary<int, List<SelectListItem>> DropDownLists { get; set; } = new Dictionary<int, List<SelectListItem>>();
        public Dictionary<int, SelectList> GroupDropDownLists { get; set; } = new Dictionary<int, SelectList>();
        public DataTable ReportResult { get; set; }
        public PivotedData PivotedData { get; set; } = null;

        public int ReportId { get; set; }
        public bool ForAggregate { get; set; } = true;
        public IFormCollection Customs { get; set; }

        public string ResultCriteria => (!string.IsNullOrEmpty(ResultFilter) ? "Filtered by: " + ResultFilter : " ") +
                                        "|" +
                                        (!string.IsNullOrEmpty(ResultGroupBy) ? "Grouped by: " + ResultGroupBy : " ");

        public string ResultFilter { get; set; }
        public string ResultGroupBy { get; set; }

        public Server.Model.Common.Reports.Report Report { get; set; }

        public List<Server.Model.Common.Reports.Report> Reports { get; set; }


    }

    public class DropDownFilterViewModel
    {
        public DropDownFilterViewModel()
        {

        }
        public List<SelectListItem> SelectList { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string AndOrValue { get; set; }
    }
    public class GroupDropDownListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string GroupText { get; set; }

    }
}
