using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Reports;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Reports;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Report.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Report.Controllers
{
    [Area("Reports")]
    public class ReportController : BaseController
    {
        private readonly ILogger<ReportController> _logger;

        private readonly GetLookupType<Server.Model.Common.Reports.Report> _qry;
        private readonly ReportsRepository _reportsRepository;
        private readonly GetLookupType<SelectListItem> _getLookupType;
        private readonly YesNoRepository _lookupRepository; 
        public ReportController(ILogger<ReportController> logger,
            UserManager<User> userManager,
            ReportsRepository reportsRepository,
            YesNoRepository lookupRepository,
            GetLookupType<SelectListItem> getLookupType,
            GetLookupType<Server.Model.Common.Reports.Report> qry,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _reportsRepository = reportsRepository;
            _getLookupType = getLookupType;
            _lookupRepository = lookupRepository;
            _qry = qry;
        }
        private string GetCurrentPage(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Academy => "Chamber Academy ",
                ECCSAModuleFlag.Arbitration => "Arbitration ",
                ECCSAModuleFlag.Communication => "Communication ",
                ECCSAModuleFlag.Investment => "Trade Promotion ",
                ECCSAModuleFlag.Membership => "Membership ",
                ECCSAModuleFlag.Reporting => "Reporting ",
                ECCSAModuleFlag.Research => "Research & Advocacy ",
                ECCSAModuleFlag.TradePromotion => "Trade Facilitation ",
                _ => string.Empty,
            };
        }

        private static StringBuilder ToCsv(DataTable dt)
        {
            var sb = new StringBuilder();

            var columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }
            return sb;

        }

        public async Task<IActionResult> Index(ECCSAModuleFlag module)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var reports = await _reportsRepository.Reports(module, ReportTypeEnum.List);

            return RedirectToAction("LoadReport", new { ReportId = reports.FirstOrDefault()?.Id ?? 0, module });

        }
        [HttpGet]
        public async Task<IActionResult> LoadReport(CustomReportViewModel viewModel, ECCSAModuleFlag module)
        {
            var userName = TheUser.FullName;
            try
            {
                viewModel = await GetRport(viewModel, module);
                viewModel.FullName = userName;
                viewModel.CurrentOperation = "Report";
                viewModel.CurrentPage = GetCurrentPage(module);
                viewModel.CurrentModule = module;
                TempData["title"] = viewModel.CurrentPage;
                var roleLevel = await GetRoleLevel(module);
                viewModel.RoleLevel = roleLevel;
            }
            catch 
            {
                return RedirectToAction("Index");
            }
            if (viewModel.Report != null)
            {
                var sql = viewModel.Report.SourceName;
                if (!string.IsNullOrEmpty(sql))
                {
                    var flist = viewModel.Report.ReportFields.Where(f => f.IsViewable).OrderBy(f => f.DisplayOrder);
                    string selectstr = "";
                    foreach (var rf in flist)
                    {
                        if (rf.IsViewable)
                        {
                            selectstr = selectstr + " " + rf.FieldName + " As '" + rf.DisplayCaption + "' , ";
                        }
                    }
                    if (selectstr != string.Empty)
                    {
                        selectstr = selectstr.Substring(0, selectstr.Length - 2);
                        sql = sql.Replace("*", selectstr);
                    }
                }
            }
            return View("Index", viewModel);
        }

        private const string AND = " AND";
        private const string OR = " OR";
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadReportCustom(CustomReportViewModel viewModel, IFormCollection coll, ECCSAModuleFlag module)
        {
            try
            {
                viewModel = await GetRport(viewModel, module);
                viewModel.FullName = TheUser.FullName;
                viewModel.CurrentOperation = "Report";
                viewModel.CurrentPage = GetCurrentPage(module);
                viewModel.CurrentModule = module;
                TempData["title"] = viewModel.CurrentPage;
                var roleLevel = await GetRoleLevel(module);
                viewModel.RoleLevel = roleLevel;
            }
            catch 
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(viewModel.Report?.SourceName))
            {
                TempData["CommandStatus"] = "Error viewing this report";
                return View("Index", viewModel);
            }

            var pivotColumnList = new string[2];
            var viewableColumnsCount = 0;
            var pivotValColumn = "";
            var pivotValColumnsCount = 0;
            var sql = viewModel.Report.SourceName;


            var flist = viewModel.Report.ReportFields.Where(f => f.IsFilterField || f.IsViewable).OrderBy(f => f.DisplayOrder);
            var filterstr = "";
            var selectstr = "";
            var groupbystr = "";
            var resultCriteriaGroup = "";


            viewModel.ResultFilter = string.Empty;
            viewModel.ResultGroupBy = string.Empty;
            foreach (var rf in flist)
            {
                if((rf.FieldName.ToLowerInvariant() == "id" || rf.Caption == "*") && !viewModel.ForAggregate) continue;
                if (rf.IsFilterField)
                {
                    var filterValue = coll[rf.HtmlFilterFieldName];
                    var filterValue1 = coll.TryGetValue(rf.HtmlFilterFieldName1, out var tempfiltervalue1) ? tempfiltervalue1.ToString() : string.Empty;
                    var andOr = coll[rf.HtmlFilterFieldAndOr];
                    filterstr += PrepareFilterString(viewModel, filterValue, filterValue1, andOr, rf);
                }
                StringValues tempFieldName = new StringValues();
                if (rf.IsViewable && !coll.TryGetValue(rf.FieldName, out tempFieldName) || string.IsNullOrEmpty(tempFieldName.ToString()))
                    continue;

                var aggregateField = coll.TryGetValue(rf.HtmlFieldAggregate, out var tempHtmlFieldAggregate)
                    ? tempHtmlFieldAggregate.ToString()
                    : string.Empty;
                var pivotField = coll.TryGetValue(rf.HtmlFieldPivot, out var tempHtmlFieldPivot)
                    ? tempHtmlFieldPivot.ToString()
                    : string.Empty;
                selectstr += GetSelectFileds(viewModel, aggregateField, pivotField, rf, pivotColumnList, ref pivotValColumn, ref pivotValColumnsCount, ref groupbystr, ref resultCriteriaGroup, ref viewableColumnsCount);

            }

            var ownerFilter = "";
            if(viewModel.Report.ServiceOwnerFieldName != null &&
                viewModel.Report.ServiceOwnerFieldName.Trim() != String.Empty)
            {
                ownerFilter += $" \"{viewModel.Report.ServiceOwnerFieldName}\" & {(int)viewModel.CurrentModule} = {(int)viewModel.CurrentModule} ";// + barckter + filterstr + barckter2;
            }
            if (filterstr != string.Empty || ownerFilter != String.Empty)
            {
                if (filterstr.EndsWith(AND))
                    filterstr = filterstr.Substring(0, filterstr.Length - AND.Length);
                if (filterstr.EndsWith(OR))
                    filterstr = filterstr.Substring(0, filterstr.Length - OR.Length);

                var barckter = ownerFilter != String.Empty && filterstr != String.Empty ? " AND (" : "";
                var barckter2 = ownerFilter != String.Empty && filterstr != String.Empty ? ")" : "";
                sql = sql + " where " + ownerFilter + barckter + filterstr + barckter2;
            }
            if (selectstr != string.Empty)
            {
                selectstr = selectstr.Substring(0, selectstr.Length - 2);
                sql = sql.Replace("*", selectstr);
            }
            if (viewModel.ForAggregate && groupbystr != string.Empty)
            {
                groupbystr = groupbystr.Substring(0, groupbystr.Length - 2);
                sql = sql + " group by " + groupbystr;
            }

            viewModel.Customs = coll;
            var datatable = _qry.GetDataTable(sql);
            viewModel.ReportResult = datatable;
            viewModel.PivotedData = null;
            if (datatable.Rows.Count >= 500)
            {
                return DownloadResult(datatable, viewModel.Report);
            }

            if (viewableColumnsCount <= 2 && pivotColumnList[0] != null)
            {
                TempData["CommandStatus"] = "Pivot not allowed for less than 3 columns !!";
                return View("Index", viewModel);
            }

            if (viewableColumnsCount <= 3 && pivotColumnList[0] != null && pivotColumnList[1] != null)
            {
                TempData["CommandStatus"] = "2 level Pivot not allowed for less than 4 columns !!";
                return View("Index", viewModel);
            }

            if (!IsPivotable(viewableColumnsCount, pivotValColumnsCount, pivotColumnList, pivotValColumn))
            {
                return View("Index", viewModel);
            }

            var dataPivoter = new PivotedData { PivotColumns = new List<string>() };
            foreach (
                var pc in
                    pivotColumnList.Where(
                        pc => !string.IsNullOrEmpty(pc) && !string.IsNullOrWhiteSpace(pc)))
            {
                dataPivoter.PivotColumns.Add(pc);
            }
            dataPivoter.ValueColumn = pivotValColumn;
            dataPivoter.Data = datatable;
            viewModel.ReportResult = dataPivoter.PivotTable;
            viewModel.PivotedData = dataPivoter;


            return View("Index", viewModel);
        }

        private static bool IsPivotable(int viewableColumnsCount, int pivotValColumnsCount, string[] pivotColumnList, string pivotValColumn)
        {
            return viewableColumnsCount <= 5 && pivotValColumnsCount == 1 &&
                   pivotColumnList.Any(pc => !string.IsNullOrEmpty(pc) && !string.IsNullOrWhiteSpace(pc)) &&
                   !string.IsNullOrEmpty(pivotValColumn) && !string.IsNullOrWhiteSpace(pivotValColumn);
        }

        private static string GetSelectFileds(CustomReportViewModel viewModel, string aggregateField, string pivotField, ReportField rf, string[] pivotColumnList, ref string pivotValColumn, ref int pivotValColumnsCount,
            ref string groupbystr, ref string resultCriteriaGroup, ref int viewableColumnsCount)
        {

            string selectString;
            if (viewModel.ForAggregate)
            {
                if (!string.IsNullOrEmpty(aggregateField) && aggregateField != "GROUPBY")
                {
                    selectString = " " + aggregateField + "(" + rf.FieldName +
                                ") As \"" + aggregateField + "(" + rf.DisplayCaption + ")\" , ";

                    //(rf.IsNumeric || rf.IsPercent) &&
                    if (!rf.IsGroupField)
                    {
                        pivotValColumn = aggregateField + "(" + rf.DisplayCaption + ")";

                        pivotValColumnsCount++;
                    }
                }
                else
                {
                    selectString = " \"" + rf.FieldName + "\" As \"" + rf.DisplayCaption + "\" , ";
                    groupbystr = groupbystr + " \"" + rf.FieldName + "\" , ";

                    resultCriteriaGroup += "\"" + rf.DisplayCaption + "\", ";
                }
            }
            else
            {
                selectString = " \"" + rf.FieldName + "\" As \"" + rf.DisplayCaption + "\" , ";
                //(rf.IsNumeric || rf.IsPercent) &&
                if (!rf.IsGroupField)
                {
                    pivotValColumn = rf.DisplayCaption;

                    pivotValColumnsCount++;
                }
            }

            if (!string.IsNullOrEmpty(pivotField) && pivotField != "0")
            {
                if (pivotField != "1")
                {
                    pivotColumnList[1] = rf.DisplayCaption;
                }
                else
                {
                    pivotColumnList[0] = rf.DisplayCaption;
                }
            }

            viewableColumnsCount++;
            return selectString;
        }

        private string PrepareFilterString(CustomReportViewModel viewModel, string filterValue, string filterValue1, string andOr, ReportField rf)
        {

            string filterstr;

            if (viewModel.DropDownLists.ContainsKey(rf.Id) &&
               viewModel.DropDownLists[rf.Id].Any(f => f.Value == filterValue))
            {
                viewModel.DropDownLists[rf.Id].Find(f => f.Value == filterValue)
                    .Selected = true;
            }

            if (filterValue == null || string.IsNullOrEmpty(filterValue) || filterValue == Guid.Empty.ToString()
                //|| filterValue.Trim() == "0"
                )
            {
                return string.Empty;
            }
            string fieldName = rf.FilterFieldName;

            var appostroph = "'";
            if (rf.FieldTypeId == FilterFieldType.DropDownListNumeric || rf.FieldTypeId == FilterFieldType.DropDownListNumericFromTo)
                appostroph = "";
            var friendlyValue = GetFriendlyValue(viewModel, rf.FieldTypeId == FilterFieldType.DropDownListNumericFromTo, rf, filterValue, filterValue1);
            switch (rf.FieldTypeId)
            {
                case FilterFieldType.DropDownListNumericFromTo:
                case FilterFieldType.TextNumericFromTo:
                    if (string.IsNullOrEmpty(filterValue) || string.IsNullOrEmpty(filterValue1))
                    {
                        if (string.IsNullOrEmpty(filterValue) && string.IsNullOrEmpty(filterValue1))
                        {
                            return string.Empty;
                        }

                        if(  string.IsNullOrEmpty(filterValue))
                            filterstr = " \"" + fieldName.Trim() + "\" <= " + filterValue1 +  " " + andOr;
                        else
                            filterstr = " \"" + fieldName.Trim() + "\" >= " + filterValue + " " + andOr;
                    }
                    else
                    {
                        filterstr = " \"" + fieldName.Trim() + "\" BETWEEN " + filterValue + " AND " + filterValue1 +
                                    " " + andOr;
                    }

                    break;
                case FilterFieldType.TextStringLike:
                    friendlyValue = " starts with '" + filterValue + "', ";
                    filterstr = " \"" + fieldName.Trim() + "\" like '" + filterValue + "%' " + andOr;
                    break;
                default:
                    filterstr = " \"" + fieldName.Trim() + "\"=" + appostroph + filterValue + appostroph + " " + andOr;
                    break;

            }

            viewModel.ResultFilter += rf.DisplayCaption + " = '" + friendlyValue + "', ";





            return filterstr;
        }

        private string GetFriendlyValue(CustomReportViewModel viewModel, bool group, ReportField rf, string filterValue, string filterValue1)
        {
            switch (rf.FieldTypeId)
            {
                case FilterFieldType.TextNumeric:
                case FilterFieldType.TextString:
                    return filterValue;
            }
            var list = group ?
               (viewModel.GroupDropDownLists.ContainsKey(rf.Id)
                ? viewModel.GroupDropDownLists[rf.Id].Select(l => new SelectListItem { Value = l.Value, Text = l.Text })
                    .ToList()
                : new List<SelectListItem>()) :
                (viewModel.DropDownLists.ContainsKey(rf.Id) ?
                                        viewModel.DropDownLists[rf.Id] : new List<SelectListItem>());

            var friendlyValue = rf.FieldTypeId == FilterFieldType.DropDownListNumericFromTo ||
                                rf.FieldTypeId == FilterFieldType.DropDownListNumeric ||
                                rf.FieldTypeId == FilterFieldType.DropDownListText
                ? list.First(
                    l => l.Value == filterValue).Text
                : filterValue;
            if (rf.FieldTypeId == FilterFieldType.TextNumericFromTo || rf.FieldTypeId == FilterFieldType.DropDownListNumericFromTo)
            {
                var friendlyValue1 = rf.FieldTypeId == FilterFieldType.TextNumericFromTo
                ? filterValue1
                : list.First(
                    l => l.Value == filterValue1).Text;
                friendlyValue = " from " + friendlyValue + " to " + friendlyValue1;
            }

            return friendlyValue;
        }

        private async Task<CustomReportViewModel> GetRport(CustomReportViewModel viewModel, ECCSAModuleFlag module)
        {
            viewModel.DropDownLists = new Dictionary<int, List<SelectListItem>>();

            var reports = await _reportsRepository.Reports(module, ReportTypeEnum.List);
            var report = await _reportsRepository.ReportById(viewModel.ReportId);
            viewModel.Report = report ?? new Server.Model.Common.Reports.Report { ReportFields = new List<ReportField>() };
            viewModel.Reports = reports;
            viewModel.ReportResult = new DataTable();
            if (report != null)
            {
                foreach (var rfield in report.ReportFields.Where(f => f.FieldTypeId == FilterFieldType.DropDownListNumeric ||
                     f.FieldTypeId == FilterFieldType.DropDownListText ||
                     f.FieldTypeId == FilterFieldType.DropDownListNumericFromTo))
                {
                   var idString = rfield.DropDownSourceId;
                   if (!rfield.DropDownSourceId.HasValue) continue;
                   var lookupType = await _lookupRepository.LookupTypeById(rfield.DropDownSourceId.Value);
                   var lst = new List<SelectListItem>
                   {
                       Capacity = lookupType.LookupValues.Count
                   };
                   foreach (var lv in lookupType.LookupValues.OrderBy( v=> v.Name))
                   {
                       lst.Add(new SelectListItem
                       {
                           Text = lv.Text,
                           Value = lv.Value
                       });
                   }
                   viewModel.DropDownLists.Add(rfield.Id, lst);
                }
                //foreach (var rfield in report.ReportFields.Where(f => f.FieldTypeId == FilterFieldType.DropDownListNumericFromTo))
                //{
                //    var sql = rfield.DropDownSource;
                //    if (string.IsNullOrEmpty(sql.Trim())) continue;
                //    var lst = _getLookupType.GetDataFromSql(sql);
                //    viewModel.DropDownLists.Add(rfield.Id, lst);
                //    //viewModel.GroupDropDownLists.Add(rfield.Id,
                //      //  new SelectList(lst, "Value", "Text", 0, "GroupText"));
                //}
            }
            return viewModel;
        }
        public FileStreamResult DownloadResult(DataTable tbl, Server.Model.Common.Reports.Report rep)
        {
            var fileName = "Export.csv";
            if (rep != null)
            {
                fileName = rep.Title + "_" + DateTime.Today.ToString("yyyyMMdd") + "_" + DateTime.UtcNow.ToString("hhmmss") + ".csv";
            }
            Response.Headers.Add("content-disposition", "attachment; filename=" + fileName);

            var sw = new StreamWriter(new MemoryStream());
            var sb = ToCsv(tbl);
            sw.Write(sb.ToString());
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(sw.BaseStream, "text/csv");


        }
    }
}
