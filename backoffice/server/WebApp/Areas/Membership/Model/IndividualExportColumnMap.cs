using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class IndividualExportColumnMap
    {
        public static List<String> Cols => new()
        {
"Code", "TIN_NO",  "Member Name Amharic", "Member Name English",    "Tel No",  "Mob_No", "Email",   "Website", "Address", "Sector", "Location", "Member Since",
            "Representative Name", "Representative Position", "Representative Mobile", "Representative Email"

    };

        public static HSSFWorkbook ExportExcel(List<MembershipEnrollment> list, List<Server.Model.Membership.Location> locations, List<LookupValue> sectors)
        {
            List<Individual> lst = new();

            HSSFWorkbook hssfwb = new HSSFWorkbook(); //This will read the Excel 97-2000 formats    
            ISheet sheet = hssfwb.CreateSheet("Members");
            IRow headerRow = sheet.CreateRow(0); //Get Header Row

            int cellCount = list.Count;
            Dictionary<int, string> indivisualModel = new();
            Dictionary<int, ContactTypeEnum> contactModel = new();
            Dictionary<int, string> staffModel = new();
            Dictionary<int, string> customModel = new();

            for (int j = 0; j < Cols.Count; j++)
            {
                headerRow.CreateCell(j)?.SetCellValue(Cols[j]);
            }
            int count = 1;
            foreach (var mer in list)
            {
                var ind = mer.Stakeholder.Individual;
                IRow row = sheet.CreateRow(count);
                row.CreateCell(0)?.SetCellValue(mer.Code ?? "");
                row.CreateCell(1)?.SetCellValue(ind.TaxIdNumber);
                row.CreateCell(2)?.SetCellValue(ind.NameLocal);
                row.CreateCell(3)?.SetCellValue(ind.NameEng);
                if (mer.Stakeholder.Contacts.Count > 0)
                {
                    row.CreateCell(5)?.SetCellValue(mer.Stakeholder.Contacts.FirstOrDefault(c => c.ContactType == ContactTypeEnum.Mobile)?.Address ?? "");
                    if (mer.Stakeholder.Contacts.Count(c => c.ContactType == ContactTypeEnum.Mobile) > 1)
                    {
                        row.CreateCell(4)?.SetCellValue(mer.Stakeholder.Contacts.LastOrDefault(c => c.ContactType == ContactTypeEnum.Mobile)?.Address ?? "");
                    }
                    row.CreateCell(6)?.SetCellValue(mer.Stakeholder.Contacts.FirstOrDefault(c => c.ContactType == ContactTypeEnum.Email)?.Address ?? "");
                    row.CreateCell(7)?.SetCellValue(mer.Stakeholder.Contacts.FirstOrDefault(c => c.ContactType == ContactTypeEnum.Website)?.Address ?? "");
                    row.CreateCell(8)?.SetCellValue(mer.Stakeholder.Contacts.FirstOrDefault(c => c.ContactType == ContactTypeEnum.Physical)?.Address ?? "");
                }
                row.CreateCell(9)?.SetCellValue(sectors.FirstOrDefault(s => s.Id == ind.SectorId)?.Name);
                var loc = locations.FirstOrDefault(c => c.Id == ind.LocationId);
                if (loc != null)
                {
                    row.CreateCell(10)?.SetCellValue($"{loc.Level1NameEng} {loc.Level2NameEng}");
                }
                row.CreateCell(11)?.SetCellValue(ind.MemberSince.ToString("yyyy-MM-dd"));
                var rep = ind.Employees.FirstOrDefault();
                if (rep != null)
                {
                    row.CreateCell(12)?.SetCellValue(rep.NameEng + " " + rep.NameLocal);
                    row.CreateCell(13)?.SetCellValue(rep.Title);
                    row.CreateCell(14)?.SetCellValue(rep.Mobile);
                    row.CreateCell(15)?.SetCellValue(rep.Email);
                }
                count++;
            }
            return hssfwb;            
        }
    }
}
