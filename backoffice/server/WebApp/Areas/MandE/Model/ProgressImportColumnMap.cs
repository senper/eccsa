using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Common.Planning;

namespace ECCSA.BackOffice.Server.WebApp.Areas.MandE.Model
{
    public class ProgressTemplate
    {
        public Guid Id { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public String Unit { get; set; }

        public int AnnualPlan { get; set; }
        public decimal Plan { get; set; }
        public decimal Actual { get; set; }

        public String Outcome { get; set; }
        public String CumulativeWork { get; set; }
        public String Remark { get; set; }
    }
    public class ProgressImportColumnMap
    {
        public static Dictionary<int, string> ProgressExcelColumnMap =>
            new Dictionary<int, string>()
            {
                { 0, "Code" },
                { 1, "Name" },
                {2, "Unit" },
                { 3, "AnnualPlan" },
                {4, "Plan" },
                { 5, "Actual" },
                {6, "Outcome" },
                { 7, "CumulativeWork" },
                { 8, "Remark" },
            };

        public static List<ProgressTemplate> ImportExcel(IFormFile file, List<ProgressTemplate> templatePlan, string TempAbsolutePath)
        {
            string webRootPath = TempAbsolutePath;
            string newPath = TempAbsolutePath;// Path.Combine(webRootPath, folderName);

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file == null || file.Length == 0)
            {
                throw new InvalidDataException("Nothing to import");
            }
            string sFileExtension = Path.GetExtension(file.FileName).ToLower();
            if (sFileExtension != ".xls" && sFileExtension != ".xlsx")
            {
                throw new InvalidDataException("Only Excel file is supported");
            }


            ISheet sheet;
            string fullPath = Path.Combine(newPath, file.FileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);
            stream.Position = 0;
            if (sFileExtension == ".xls")
            {
                HSSFWorkbook hssfwb = new(stream); //This will read the Excel 97-2000 formats  
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
            }
            else
            {
                XSSFWorkbook hssfwb = new(stream); //This will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
            }
            IRow headerRow = sheet.GetRow(0); //Get Header Row

            int cellCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;
            if (rowCount != templatePlan.Count || cellCount != 8)
            {
                throw new InvalidDataException($"Excel file with {templatePlan.Count} rows and 8 columns expected");
            }

            var type = typeof(ProgressTemplate);
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
            {
                var t = templatePlan[i - 1];
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                for (int j = 5; j < cellCount; j++)
                {
                    var prop = type.GetProperty(ProgressExcelColumnMap[j]);
                    var sVal = row.GetCell(j)?.ToString();
                    if (j == 5)
                    {
                        decimal.TryParse(sVal, out var dVal);
                        prop.SetValue(t, dVal);
                    }
                    else
                        prop.SetValue(t, sVal);
                }
                //  lst.Add(t);
            }


            return templatePlan;
        }
    }
}