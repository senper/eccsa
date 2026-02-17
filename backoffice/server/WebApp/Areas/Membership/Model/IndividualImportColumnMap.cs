using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class IndividualImportColumnMap
    {
        public static Dictionary<string, string> IndivisualExcelColumnMap =>
            new Dictionary<string, string>()
        {
            { "No", "No" },
            { "Company Name Amharic", "NameLocal" },
            { "Company Name English", "NameEng" },
            { "TIN_NO", "TaxIdNumber" },
            { "Sector", "Sector" },
            { "Subsector", "Subsector" },
            { "Capital", "Capital" },
            { "Number of Employees", "NumberofEmployees" },
        };
        public static Dictionary<string, string> ContactExcelColumnMap =>
            new Dictionary<string, string>()
        {
            { "Tel No", "Mobile" },
            { "Mob No", "Mobile" },
            { "Email", "Email" },
            { "Website", "Website" },
            { "H_No", "Physical" }
        };
        public static Dictionary<string, string> StaffExcelColumnMap =>
            new Dictionary<string, string>()
        {
            { "Representative Name", "NameEng" },
            { "Representative Position", "Title" },
            { "Representative Mobile", "Mobile" },
            { "Representative Email", "Email" },
        };
        public static Dictionary<string, string> BusinessExcelColumnMap =>
            new Dictionary<string, string>()
        {
            { "Business Type", "BusinessType" },
            { "License No", "LicenseNo" },
            { "Business License Issue Date", "IssueDate" },
            { "Business License Renewal Date", "RenewalDate" },
            { "Capital", "Capital" },
        };
        //public static Dictionary<string, string> RepExcelColumnMap =>
        //    new Dictionary<string, string>()
        //{
        //    { "Representative Name", "NameEng" },
        //    { "Representative Position", "Position" },
        //};
        //public static Dictionary<string, string> RepContactExcelColumnMap =>
        //    new Dictionary<string, string>()
        //{
        //    { "Representative Mobile", "Mobile" },
        //    { "Representative Email", "Email" },
        //};

        public static List<Individual> ImportExcel(IFormFile file, string TempAbsolutePath)
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
            List<Individual> lst = new();
            if (file.Length > 0)
            {
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
                Dictionary<int, string> indivisualModel = new ();
                Dictionary<int, ContactTypeEnum> contactModel = new();
                Dictionary<int, string> staffModel = new ();
                Dictionary<int, string> businessModel = new();
                Dictionary<int, string> customModel = new ();

                var type = typeof(Individual);
                var typeStaff = typeof(IndividualStaff);
                var typeBusiness = typeof(IndividualBusiness);
                string propName = "";
                for (int j = headerRow.FirstCellNum; j < cellCount; j++)
                {
                    if (String.IsNullOrEmpty(headerRow.GetCell(j)?.StringCellValue)) continue;
                    if (IndivisualExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                            && type.GetProperty(propName) != null)
                    {
                        indivisualModel.Add(j, propName);
                    }
                    else if (ContactExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                        && Enum.TryParse<ContactTypeEnum>(propName, out var enumVal))
                    {

                        contactModel.Add(j, enumVal);
                    }
                    else if (StaffExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                        && typeStaff.GetProperty(propName) != null)
                    {

                        staffModel.Add(j, propName);
                    }
                    else if (BusinessExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                        && typeBusiness.GetProperty(propName) != null)
                    {

                        businessModel.Add(j, propName);
                    }
                    else
                    {
                        customModel.Add(j, headerRow.GetCell(j).StringCellValue);
                    }
                }
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    Individual t = new()
                    {
                        Id = Guid.NewGuid(),
                        Stakeholder = new Stakeholder
                        {
                            Id = Guid.NewGuid(),
                            Contacts = new List<Contact>(),
                            Representatives = new List<Representative>()
                        },
                        CustomFields = new List<CustomField>(),
                        Employees = new List<IndividualStaff>(),
                        BusinessLicenses = new List<IndividualBusiness>()
                    };
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (String.IsNullOrEmpty(headerRow.GetCell(j)?.StringCellValue)) continue;
                        var sVal = row.GetCell(j)?.ToString();
                        //string propName = "";
                        if (indivisualModel.ContainsKey(j))
                            //IndivisualExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                            //&& type.GetProperty(propName) != null)
                        {
                            type.GetProperty(indivisualModel[j]).SetValue(t, sVal);
                        }
                        else if (contactModel.ContainsKey(j))
                            //ContactExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                            //&& Enum.TryParse<ContactTypeEnum>(propName, out var enumVal))
                        {
                            t.Stakeholder.Contacts.Add(new Contact
                            {
                                Id = Guid.NewGuid(),
                                Priority = 0,
                                ContactType = contactModel[j],
                                Address = sVal
                            });
                        }
                        else if (staffModel.ContainsKey(j))
                            //StaffExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                            //&& typeStaff.GetProperty(propName) != null)
                        {
                            if (t.Employees.Count <= 0)
                            {
                                t.Employees.Add(new IndividualStaff
                                {
                                    Id = Guid.NewGuid(),
                                    Status = StatusEnum.Active,
                                    StartDate = DateTime.Today.AddDays(-1),
                                    EndDate = DateTime.Today,
                                    Title = "",
                                    NameLocal = ""
                                });
                            }
                            var staff = t.Employees.FirstOrDefault();
                            typeStaff.GetProperty(staffModel[j]).SetValue(staff, sVal);
                        }
                        else if (businessModel.ContainsKey(j))
                        {
                            if (t.BusinessLicenses.Count <= 0)
                            {
                                t.BusinessLicenses.Add(new IndividualBusiness
                                {
                                    Id = Guid.NewGuid(),
                                    SerNo = string.Empty,
                                });
                            }
                            var business = t.BusinessLicenses.FirstOrDefault();
                            var prop = typeBusiness.GetProperty(businessModel[j]);
                            if (prop.PropertyType == typeof(DateTime))
                            {
                                if (DateTime.TryParse(sVal, out var dateVal))
                                {
                                    typeBusiness.GetProperty(businessModel[j]).SetValue(business, dateVal);
                                }
                                else
                                {
                                    typeBusiness.GetProperty(businessModel[j]).SetValue(business, DateTime.Today);
                                }
                            }
                            else
                            {
                                typeBusiness.GetProperty(businessModel[j]).SetValue(business, sVal);
                            }
                        }
                        else if (customModel.ContainsKey(j))
                        {
                            t.CustomFields.Add(new CustomField
                            {
                                Text = customModel[j],
                                Value = sVal
                            });
                        }
                        //else if (RepExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                        //    && typeRep.GetProperty(propName) != null)
                        //{
                        //    if (t.Stakeholder.Representatives.Count <= 0)
                        //    {
                        //        t.Stakeholder.Representatives.Add(new Representative
                        //        {
                        //            Id = Guid.NewGuid(),
                        //            Status = StatusEnum.Active,
                        //            StartDate = DateTime.Today.AddDays(-1),
                        //            EndDate = DateTime.Today,
                        //            Title = "",
                        //            OrderNo = "",
                        //            NameLocal = "",
                        //            RepresentativeContacts = new List<RepresentativeContact>()
                        //        });
                        //    }
                        //    var rep = t.Stakeholder.Representatives.FirstOrDefault();
                        //    typeRep.GetProperty(propName).SetValue(rep, sVal);
                        //}

                        //else if (RepContactExcelColumnMap.TryGetValue(headerRow.GetCell(j).StringCellValue, out propName)
                        //    && Enum.TryParse<ContactTypeEnum>(propName, out var enumVal2))
                        //{
                        //    if (t.Stakeholder.Representatives.Count <= 0)
                        //    {
                        //        t.Stakeholder.Representatives.Add(new Representative
                        //        {
                        //            Id = Guid.NewGuid(),
                        //            Status = StatusEnum.Active,
                        //            StartDate = DateTime.Today.AddDays(-1),
                        //            EndDate = DateTime.Today,
                        //            Title = "",
                        //            OrderNo = "",
                        //            NameLocal = "",
                        //            RepresentativeContacts = new List<RepresentativeContact>()
                        //        });
                        //    }
                        //    var rep = t.Stakeholder.Representatives.FirstOrDefault();
                        //    rep.RepresentativeContacts.Add(new RepresentativeContact
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        Priority = 0,
                        //        ContactType = (int)enumVal2,
                        //        Address = sVal
                        //    });
                        //}
                    }
                    lst.Add(t);
                }
            }

            return lst;
        }
    }
}
