using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;

namespace CCSA.BackOffice.Server.WebApp.Areas.MandE.Model
{
    public class AggregateReportExportColumnMap
    {
        public static List<String> Cols => new()
        {
            "ተ.ቁ", "በዕቅድ የተያዙ ዋና ዋና ግቦች፣ ዓላማዎችና ተግባራት",  "መለክያ", "የአመቱ ዕቅድ",    "ዕቅድ",  "ክንውን", "ንጽጽር በ%", "የተከናወኑ ተግባራት",
            "የተከናወኑ ተግባራት", "ዕቅድ", "ክንውን", "ንጽጽር በ%","አስተያየት", "የሚያከናውነው ክፍል/አካል"
        };
        public static void BuildRow(ISheet sheet, int rowNo, ProgressViewModel Model, Plan ind)
        {
            var hasChildren = Model.HasChildren(ind.Id);
            var labelSubPlan = Model.AreAllLabelChildren(ind.Id);
            var isPlanning = ind.Status == PlanningStatusEnum.Planning || ind.Status == PlanningStatusEnum.Rejected;

            var ownedIndicator = ind.ResponsibleUnit == Model.CurrentSubsystem && ind.SupportingUnitId == Model.SupportingUnitId;
            var unitName = @ind.ResponsibleUnit.GetDisplayName();

            IRow row = sheet.CreateRow(rowNo);
            row.CreateCell(0)?.SetCellValue(rowNo - 1);
            row.CreateCell(1)?.SetCellValue(ind.NameWithCode ?? "");
            row.CreateCell(2)?.SetCellValue(ind.UnitName ?? "");
            row.CreateCell(3)?.SetCellValue(ind.TotalPlanString ?? "");
            row.CreateCell(4)?.SetCellValue(ind.ReportedPlan ?? "");
            row.CreateCell(5)?.SetCellValue(ind.ReportedActual ?? "");
            if (double.TryParse(ind.ReportedActual, out var n1) && double.TryParse(ind.ReportedPlan, out var n2) && n2 > 0)
            {
                row.CreateCell(6)?.SetCellValue(n1 * 100/ n2);
            }
            else
            {
                row.CreateCell(6)?.SetCellValue(ind.ReportedActual ?? "" + " / " + ind.ReportedPlan ?? "");
            }
            row.CreateCell(7)?.SetCellValue(ind.ReportedOutput ?? "");
            row.CreateCell(8)?.SetCellValue(ind.ReportedYtDOutput ?? "");
            row.CreateCell(9)?.SetCellValue(ind.ReportedYtDPlan ?? "");
            row.CreateCell(10)?.SetCellValue(ind.ReportedYtDActual ?? "");

            if (double.TryParse(ind.ReportedYtDActual, out var n3) && double.TryParse(ind.ReportedYtDPlan, out var n4) && n4 > 0)
            {
                row.CreateCell(11)?.SetCellValue(n3 * 100 / n4);
            }
            else
            {
                row.CreateCell(6)?.SetCellValue(ind.ReportedYtDActual ?? "" + " / " + ind.ReportedYtDPlan ?? "");
            }
            row.CreateCell(12)?.SetCellValue(ind.Remark ?? "");
            var sUnit = Model.SupportingUnits.FirstOrDefault(s => s.Value == ind.SupportingUnitId.ToString())?.Name;
            var runit = ind.ResponsibleUnitCode + (ind.SupportingUnitId != 0 && !string.IsNullOrEmpty(sUnit) ? $"_{sUnit}" : "");
            row.CreateCell(13)?.SetCellValue(runit ?? "");

        }

        public static void Template(ISheet sheet, ref int rowNo, ProgressViewModel Model, Plan parent, List<Plan> subPlans)
        {

            BuildRow(sheet, rowNo++, Model, parent);

            foreach (var ind in subPlans)
            {
                var plans = Model.TheList.Where(p => p.ParentId == ind.Id).OrderBy(p => p.Code).ToList();
                Template(sheet, ref rowNo, Model, ind, plans);
            }
        }
        public static void MergeRegionAndSetValue(ISheet sheet, string mergeVal, string val)
        {
            var mr1 = sheet.AddMergedRegion(NPOI.SS.Util.CellRangeAddress.ValueOf(mergeVal));
            var region = sheet.GetMergedRegion(mr1);
            IRow row = sheet.GetRow(region.FirstRow);
            var headercell1 = row?.CreateCell(region.FirstColumn);
            headercell1.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headercell1.CellStyle.VerticalAlignment = VerticalAlignment.Center;
            headercell1.CellStyle.WrapText = true;
            headercell1.CellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            headercell1.CellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            headercell1.CellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            headercell1.CellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            headercell1.SetCellValue(val);
        }

        public static HSSFWorkbook ExportExcel(ProgressViewModel Model)
        {
            List<Plan> lst = new();

            HSSFWorkbook hssfwb = new HSSFWorkbook(); //This will read the Excel 97-2000 formats    
            ISheet sheet = hssfwb.CreateSheet("Aggregate Report");
            IRow headerRow1 = sheet.CreateRow(0); //Get Header Row
            IRow headerRow = sheet.CreateRow(1); //Get Header Row
            Dictionary<int, string> indivisualModel = new();
            Dictionary<int, string> staffModel = new();
            Dictionary<int, string> customModel = new();
            MergeRegionAndSetValue(sheet, "E1:H1", "የ" + Model.CurrentYear + " በጀት አመት " + Model.SelectedPeriod + " ወር ዕቅድ ክንውን");
            MergeRegionAndSetValue(sheet, "I1:L1", "እስከ እዚህ ወር መጨረሻ የተከናወነ");

            MergeRegionAndSetValue(sheet, "A1:A2", Cols[0]);
            MergeRegionAndSetValue(sheet, "B1:B2", Cols[1]);
            MergeRegionAndSetValue(sheet, "C1:C2", Cols[2]);
            MergeRegionAndSetValue(sheet, "D1:D2", Cols[3]);
            MergeRegionAndSetValue(sheet, "M1:M2", Cols[12]);
            MergeRegionAndSetValue(sheet, "N1:N2", Cols[13]);
            for (int j = 4; j < 12; j++)
            {
                var cell = headerRow?.CreateCell(j);
                cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                cell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
                cell.CellStyle.WrapText = true;
                cell.SetCellValue(Cols[j]);
            }

            sheet.SetColumnWidth(1, 9600);
            sheet.SetColumnWidth(7, 9600);
            sheet.SetColumnWidth(8, 9600);
            int rowNo = 2;
            if (Model.TreeView)
            {
                foreach (var root in Model.RootPlans)
                {
                    var subPlans = Model.TheList.Where(p => p.ParentId == root.Id).OrderBy(p => p.Code).ToList();
                    if (subPlans.Count == 0) continue;
                    Template(sheet,ref rowNo, Model, root, subPlans);
                    rowNo++;
                }
            }
            else
            {
                foreach (var plan in Model.TheList.OrderBy(c => c.Code))
                {
                    BuildRow(sheet, rowNo++, Model, plan);
                }
            }
            IRow row = sheet.CreateRow(rowNo);
            var lastcell = row.CreateCell(0);
            return hssfwb;
        }
    }
}
