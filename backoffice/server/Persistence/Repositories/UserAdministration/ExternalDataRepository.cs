using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.ExternalData;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration
{
    public class ExternalDataRepository
    {
        private readonly EccsaContext _context;
        private readonly ExternalDataContext _dataContext;
        public ExternalDataRepository(EccsaContext context, ExternalDataContext dataContext)
        {
            _context = context;
            _dataContext = dataContext;
        }
        public async Task<List<CertificateOfOrigin>>CertificateOfOrigins(DateTime importDate){
            return await _context.CertificateOfOrigins.Include(c=> c.CommodityList).Where(c => c.ImportDate == importDate.Date).ToListAsync();
        }
        public async Task<List<ExternalDataType>> ExternalDataTypes()
        {
            var lst= await _context.ExternalDataTypes.ToListAsync();
            if (lst.Count == 0)
            {
                await GenerateInitialData();
                lst = await _context.ExternalDataTypes.ToListAsync();
            }
            
            return lst;
        }
        public async Task<ExternalDataType> ExternalDataTypeById(Guid id)
        {
            return await _context.ExternalDataTypes.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task Save(ExternalDataType data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            var tmp = await _context.ExternalDataTypes.FirstOrDefaultAsync(i => i.Id == data.Id);
            if (tmp == null)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.ExternalDataTypes.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
        }

        public async Task<List<ESWRequest>> EswRequests()
        {
            return await _context.EswRequests.OrderByDescending(r=> r.RequestDate).Take(20).ToListAsync();
        }

        private async Task GenerateInitialData()
        {
            var data = new List<ExternalDataType>
            {
                 new ExternalDataType
                {
                    Id = Guid.NewGuid(),
                    Caption = "Ministry of Trade",
                    SchemaName = "public",
                    SourceName = "mot",
                    MetaData = new List<ExternalMetaData>
                {
                    GetId(),
                    GetStartDate(),
                    GetEndDate(),
                    GetTIN(),

                    GetMetaData("License No", "licenseno", 100),
                    GetMetaData("Company Name", "company_name"),
                    GetMetaData("Owner Manager Name", "owner_manager_name"),
                    GetMetaData("Sex", "sex"),
                    GetMetaData("Legal Status", "legal_status"),
                    GetMetaData("Sector", "sector"),
                    GetMetaData("Sub Sector", "subsector"),

                    GetMetaData("Capital", "capital"),

                }
                },
                 new ExternalDataType
                {
                    Id = Guid.NewGuid(),
                    Caption = "Ministry of Revenue Importer Exporters",
                    SchemaName = "public",
                    SourceName = "importer_exporters",
                    MetaData = new List<ExternalMetaData>
                {
                    GetId(),
                    GetStartDate(),
                    GetEndDate(),
                    new ExternalMetaData()
                    {
                        Caption = "Import Export Direction",
                        ColumnName = "import_export_direction",
                        NotNull = true,
                        ShowInGrid = true,
                        DataType = DataTypeEnum.String,
                        DefaultValue = "E",
                        MaxSize = 6
                    },

                    GetMetaData("Company Name", "company_name"),
                    GetMetaData("Product Type", "product_type"),
                    GetMetaData("Country", "country",10),
                    new ExternalMetaData()
                    {
                        Caption = "Value",
                        ColumnName = "value",
                        NotNull = true,
                        ShowInGrid = true,
                        DataType = DataTypeEnum.Double,
                    },


                }
                },
                 new ExternalDataType
                {
                    Id = Guid.NewGuid(),
                    Caption = "Ministry of Revenue TIN",
                    SchemaName = "public",
                    SourceName = "tin",
                    MetaData = new List<ExternalMetaData>
                {
                    GetId(),
                    GetStartDate(),
                    GetEndDate(),

                    GetTIN(),
                    GetMetaData("License No", "licenseno", 100),
                    GetMetaData("Company Name", "company_name"),
                    GetMetaData("Owner Manager Name", "owner_manager_name"),
                    GetMetaData("Sex", "sex"),
                    GetMetaData("Legal Status", "legal_status"),
                    GetMetaData("Sector", "sector"),
                    GetMetaData("Sub Sector", "subsector"),


                }
                }
                 ,
                 new ExternalDataType
                {
                    Id = Guid.NewGuid(),
                    Caption = "Ethiopian Investment Authority",
                    SchemaName = "public",
                    SourceName = "investments",
                    MetaData = new List<ExternalMetaData>
                {
                    GetId(),
                    GetStartDate(),
                    GetEndDate(),
                    GetTIN(),

                    GetMetaData("License No", "licenseno", 100),
                    GetMetaData("Company Name", "company_name"),
                    GetMetaData("Owner Manager Name", "owner_manager_name"),
                    GetMetaData("Sex", "sex",10),
                    GetMetaData("Project Status", "project_status"),
                    GetMetaData("Investment Type", "investment_type"),
                    GetMetaData("Location", "location"),
                    GetMetaData("Sector", "sector"),
                    GetMetaData("Capital", "capital"),
                    GetMetaData("Permanent Females", "permanent_female"),
                    GetMetaData("Permanent Males", "permanent_male"),
                    GetMetaData("Temporary Females", "temporary_female"),
                    GetMetaData("Temporary Males", "temporary_male"),
                }
                },
                 new ExternalDataType
                {
                    Id = Guid.NewGuid(),
                    Caption = "Ministry of Industry",
                    SchemaName = "public",
                    SourceName = "manufacturers",
                    MetaData = new List<ExternalMetaData>
                {
                    GetId(),
                    GetStartDate(),
                    GetEndDate(),
                    GetTIN(),

                    GetMetaData("License No", "licenseno", 100),
                    GetMetaData("Company Name", "company_name"),
                    GetMetaData("Owner Manager Name", "owner_manager_name"),
                    GetMetaData("Sex", "sex"),
                    GetMetaData("Legal Status", "legal_status"),

                    GetMetaData("Production Volume", "production_volume"),
                    GetMetaData("Production Value", "production_value"),
                    GetMetaData("Production Efficiency", "production_efficiency"),
                    GetMetaData("Sector", "sector"),
                    GetMetaData("Sub Sector", "subsector"),
                    GetMetaData("Permanent Females", "permanent_female"),
                    GetMetaData("Permanent Males", "permanent_male"),
                    GetMetaData("Temporary Females", "temporary_female"),
                    GetMetaData("Temporary Males", "temporary_male"),

                }
                },
                 new ExternalDataType
                {
                    Id = Guid.NewGuid(),
                    Caption = "Members",
                    SchemaName = "public",
                    SourceName = "manufacturers",
                    MetaData = new List<ExternalMetaData>
                {
                    GetId(),
                    GetStartDate(),
                    GetEndDate(),
                    GetTIN(),

                    GetMetaData("Member Name", "member_name"),
                    GetMetaData("Owner Manager Name", "owner_manager_name"),
                    GetMetaData("Sex", "sex"),
                    GetMetaData("Legal Status", "legal_status"),
                    GetMetaData("Sector", "sector"),
                    GetMetaData("Sub Sector", "subsector"),

                    GetMetaData("Capital", "capital"),

                }
                }
            };
            foreach (var d in data)
            {
                d.MetaData.AddRange(GetAddress());
            }
            data.Add(
                new ExternalDataType
                {
                    Id = Guid.NewGuid(),
                    Caption = "Import Export Summary",
                    SchemaName = "public",
                    SourceName = "importexports",
                    MetaData = new List<ExternalMetaData>
                {
                   GetId(),
                   GetStartDate(),
                   GetEndDate(),
                    GetMetaData("Import Export", "import_export", 6),
                    GetMetaData("HS Code", "hscode"),
                    GetMetaData("Country", "country"),
                   new ExternalMetaData()
            {
                Caption = "Value",
                ColumnName = "value",
                NotNull = true,
                ShowInGrid = true,
                DataType = DataTypeEnum.Double,
            },
                   new ExternalMetaData()
            {
                Caption = "Volume",
                ColumnName = "volume",
                NotNull = true,
                ShowInGrid = true,
                DataType = DataTypeEnum.Double,
            },
                   new ExternalMetaData()
            {
                Caption = "Count",
                ColumnName = "count",
                NotNull = true,
                ShowInGrid = true,
                DataType = DataTypeEnum.Integer,
            }

                }
                }
                );

            foreach (var d in data)
            {
                foreach(var md in d.MetaData)
                {
                    md.ExcelHeaderName = md.Caption.Replace(" ", "").Trim();
                    var cname = md.ColumnName.ToLowerInvariant().Replace("_", "");
                    if (cname == "id" || cname == "startdate" || cname == "enddate")
                        md.ExcelHeaderName = string.Empty;
                }
                if (d.DataPeriods == null)
                {
                    d.DataPeriods = new List<Period>
                    {
                        new Period
                        {
                            StartDate = new DateTime(1900,1,1),
                            EndDate = new DateTime(1900,1,1)
                        }
                    };
                }
            }
            _context.ExternalDataTypes.AddRange(data);
            await _context.SaveChangesAsync();
        }
        private List<ExternalMetaData> GetAddress()
        {
            return new List<ExternalMetaData>
                {
                    GetMetaData("Region", "region"),
                    GetMetaData("Phone", "phone"),
                    GetMetaData("Mobile", "mobile"),
                    GetMetaData("Email", "email"),
                    GetMetaData("Website", "website"),
                    GetMetaData("Fax", "fax"),
                    GetMetaData("POBox", "pobox"),
                };
        }
        private ExternalMetaData GetMetaData(string caption, string columnName, int maxSize = 250)
        {
            return new ExternalMetaData()
            {
                Caption = caption,
                ColumnName = columnName,
                NotNull = false,
                ShowInGrid = true,
                DataType = DataTypeEnum.String,
                DefaultValue = "NULL",
                MaxSize = maxSize
            };
        }


        private ExternalMetaData GetStartDate()
        {
            return new ExternalMetaData()
            {
                Caption = "Start Date",
                ColumnName = "start_date",
                NotNull = true,
                ShowInGrid = true,
                DataType = DataTypeEnum.Date,

            };
        }
        private ExternalMetaData GetEndDate()
        {
            return new ExternalMetaData()
            {
                Caption = "End Date",
                ColumnName = "end_date",
                NotNull = true,
                ShowInGrid = true,
                DataType = DataTypeEnum.Date,

            };
        }
        private ExternalMetaData GetTIN()
        {
            return new ExternalMetaData()
            {
                Caption = "TIN",
                ColumnName = "tin",
                NotNull = true,
                ShowInGrid = true,
                DataType = DataTypeEnum.String,
                DefaultValue = "000000000000",
                MaxSize = 12
            };
        }
        private ExternalMetaData GetId()
        {
            return new ExternalMetaData()
            {
                Caption = "Id",
                ColumnName = "id",
                NotNull = true,
                ShowInGrid = false,
                DataType = DataTypeEnum.Guid
            };
        }
        public DataTable GetData(string schemaName, string sourceName, DateTime startDate, DateTime endDate, int skip = 0, int take = 1000)
        {
            var sql = $"select * from {schemaName}.{sourceName} where start_date >= '{startDate.ToString("yyyy-MM-dd")}' AND end_date <= '{endDate.ToString("yyyy-MM-dd")}'";
            return _dataContext.DataTable(sql);
        }
        public async Task BulkInsert(ExternalDataType dataType, DataTable data)
        {
            var tmp = await _context.ExternalDataTypes.FirstOrDefaultAsync(i => i.Id == dataType.Id);
            tmp.DataPeriods = dataType.DataPeriods;
            _context.ExternalDataTypes.Update(tmp);
            await _context.SaveChangesAsync();
            await _dataContext.BulkInsert(dataType, data);

        }
        public async Task BulkInsert<T>(List<T> lst)
        {
            await _dataContext.BulkInsert<T>(lst);
        }
    
        public async Task GenerateDirectory(List<LookupValue> lst)
        {
           
            using var connection = _context.Database.GetDbConnection();
            try
            {
                connection.Open();
                foreach (var lv in lst)
                {
                    string id = "\"id\"";
                    string idVal = $"'{lv.Id}'";
                    string tyeId = "'00000000-0000-0000-0000-000000000002'";
                    string sql;
                    if (!lv.ParentId.HasValue)
                        sql = $"INSERT INTO lookup_values({id}, name, lookup_type_id, status) VALUES({idVal}, '{lv.Name}', {tyeId}, 1);";
                    else
                        sql = $"INSERT INTO lookup_values({id}, name, lookup_type_id, status, parent_id) VALUES({idVal}, '{lv.Name}', {tyeId}, 1, '{lv.ParentId.Value}');";
                    try
                    {
                        await _context.ExecuteSql(connection, sql);
                    }
                    catch(Exception ex) {
                        var msg = ex.Message;
                    }
                }
            }
            finally
            {
                connection.Close();
                await connection.DisposeAsync();
            }
            
        }
        public void GenerateDirectory(System.IO.DirectoryInfo parent, Guid? parentId, ref int i, ref List<LookupValue> lst)
        {
            Guid lookupTemplate = Guid.Parse("00000000-0000-0000-1000-000000000000");
            Guid tyeId = Guid.Parse( "00000000-0000-0000-0000-000000000002");
            foreach (var dInfo in parent.GetDirectories())
            {
                i++;
                var id = FromInt(i, lookupTemplate);
                lst.Add(new LookupValue() {
                    Id = id,
                    Name = dInfo.Name,
                    LookupTypeId = tyeId,
                    ParentId = parentId
                });
                var children = dInfo.GetDirectories();
                if (children.Length > 0)
                {
                    GenerateDirectory(dInfo, id, ref i, ref lst);
                }
               
            }
        }
        private Guid FromInt(int val, Guid template)
        {
            var s = template.ToString();
            var hex = val.ToString("X");
            var lenHex = hex.Length;
            s = s[..^lenHex] + hex;
            return Guid.Parse(s);
        }
    }
   
}
