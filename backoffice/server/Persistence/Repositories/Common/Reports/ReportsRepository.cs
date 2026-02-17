using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Reports;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.Reports
{
    public class SelectListItemReport
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class ReportsRepository
    {
        private readonly EccsaContext _context;
        public ReportsRepository(EccsaContext context)
        {
            _context = context;
        }
        public async Task<List<Report>> Reports()
        {
            return await _context.Reports
                .ToListAsync();
        }
        public async Task<List<Report>> Reports(ECCSAModuleFlag subSystem)
        {
            return await _context.Reports
               .Where(x => x.ReportOwner.HasFlag(subSystem)).ToListAsync();
        }

        public async Task<List<Report>> Reports(ECCSAModuleFlag subSystem, ReportTypeEnum type)
        {
            GetLookupType<SelectListItemReport> _getLookupType = new(_context);
            try
            {
                var ctry = await _context.Countries.ToListAsync();
                var lt = await _context.LookupTypes.FirstOrDefaultAsync(l => l.Id == Country.LookupTypeId);
                if (lt == null)
                {
                    lt = new LookupType()
                    {
                        Id = Country.LookupTypeId,
                        Name = "Countries",
                        IsHierarchical = false,
                        SourceType = LookupSourceTypeEnum.Dynamic,
                        Status = StatusEnum.Active,
                        Owner = (ECCSAModuleFlag)1023
                    };
                    _context.LookupTypes.Add(lt);
                    await _context.SaveChangesAsync();
                }
                var countries = await _context.LookupValues.Where(l=> l.LookupTypeId == lt.Id).ToListAsync();
                if (countries.Count < ctry.Count)
                {
                    if (countries.Any())
                    {
                        _context.LookupValues.RemoveRange(countries);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var c in ctry)
                    {

                        var lv = new LookupValue
                        {
                            Id = Guid.NewGuid(),
                            Name = c.Name,
                            Code = c.Iso3,
                            Value = c.Name,
                            Status = StatusEnum.Active,
                            LookupTypeId = lt.Id,
                        };
                        _context.LookupValues.Add(lv);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            var lookupTypes = await _context.LookupTypes.Include(l=> l.LookupValues).Where(l=> l.Name.EndsWith("_report")).ToListAsync();
            var hasChanged = false;
            foreach (var lt in lookupTypes)
            {
               var exists = await _context.ReportFields.AnyAsync(f => f.DropDownSourceId.Value == lt.Id);
               if (!exists)
               {
                   _context.LookupTypes.Remove(lt);
                   hasChanged = true;
               }
            }
            if(hasChanged)
                 await _context.SaveChangesAsync();
            var rf = _context.ReportFields.Where(r => r.DropDownSource.StartsWith("SELECT") && !r.DropDownSourceId.HasValue).ToList();
            var sourceList = new Dictionary<string, LookupType>();
            hasChanged = false;
            foreach (var f in rf)
            {
                LookupType lookupType = null;
                if (sourceList.Keys.Any(s => s == f.DropDownSource.Trim().ToLowerInvariant().Replace(" ", "")))
                {
                    lookupType = sourceList[f.DropDownSource.Trim().ToLowerInvariant().Replace(" ", "")];
                }
                 
                if (lookupType == null)
                {
                    var l = _getLookupType.GetDataFromSql(f.DropDownSource);
                    lookupType = new LookupType
                    {
                        Id = Guid.NewGuid(),
                        Name = $"{f.FieldName}_report",
                        IsHierarchical = false,
                        Owner = (ECCSAModuleFlag)1023,
                        SourceType = LookupSourceTypeEnum.System,
                        Status = StatusEnum.Active,
                        LookupValues = new List<LookupValue>()
                    };
                    if(lookupType.Name.ToLowerInvariant() == "chamber_association_report".ToLowerInvariant())
                    {
                        lookupType.Id = LookupType.Association;
                    }

                    foreach (var lv in l)
                    {
                        if (lv.Value.Length > 50)
                        {
                            lookupType.Id = lookupType.Id;
                        }

                        lookupType.LookupValues.Add(new LookupValue
                        {
                            Code = "",
                            Id = Guid.NewGuid(),
                            LookupTypeId = lookupType.Id,
                            Name = lv.Text,
                            Value = lv.Value,
                            ParentName = "",
                            Status = StatusEnum.Active,
                        });
                    }

                    _context.LookupTypes.Add(lookupType);
                    sourceList.Add(f.DropDownSource.Trim().ToLowerInvariant().Replace(" ", ""), lookupType);
                }

                f.DropDownSourceId = lookupType.Id;
                f.DropDownSource = lookupType.Name;
                hasChanged = true;
            }

            if (hasChanged)
            {
                await _context.SaveChangesAsync();
                hasChanged = false;
            }

           
          

            //await _context.SaveChangesAsync();
            return await _context.Reports
               .Where(x => x.ReportOwner.HasFlag(subSystem) && x.ReportTypeId == type).ToListAsync();
        }

        public async Task<List<ReportSource>> ReportSources()
        {
            return await _context.ReportSources.Include(s => s.SourceFields).AsNoTracking().ToListAsync();
        }
        public async Task<Report> ReportById(int id)
        {
            var request = await _context.Reports
                    .Include(x => x.ReportFields)
                    .FirstOrDefaultAsync(i => i.Id == id);
            return request;
        }

        public async Task Save(Report data)
        {
            var source = await _context.ReportSources.AsNoTracking().Include(s => s.SourceFields)
                .FirstOrDefaultAsync(s => s.SourceName == data.SourceName);
            var tmp = await _context.Reports.Include(r => r.ReportFields).FirstOrDefaultAsync(r =>
                r.Id == data.Id && r.SourceName.EndsWith(data.SourceName));
            if (tmp != null)
            {
                tmp.GroupName = data.GroupName;
                tmp.SourceName = data.SourceName;
                tmp.ReportFields = data.ReportFields;
                tmp.Title = data.Title;
                tmp.SubTitle = data.SubTitle;
                tmp.SortOrder = data.SortOrder;
                tmp.IsActive = data.IsActive;

            }
            else
            {
                data.ReportFields = new List<ReportField>();
                var i = 1;
                data.ReportTypeId = ReportTypeEnum.List;
                data.SortOrder = int.MaxValue.ToString();
                data.IsActive = false;
                foreach (var field in source.SourceFields)
                {

                    data.ReportFields.Add(new ReportField
                    {
                        Caption = field.ColumnName,
                        DisplayOrder = i++,
                        DropDownSource = string.Empty,
                        FieldName = field.ColumnName,
                        FieldTypeId = FilterFieldType.None,
                        FilterFieldName = string.Empty,
                        IsFilterField = false,
                        IsGroupField = false,
                        IsNumeric = false,
                        IsPercent = false,
                        IsViewable = false,
                        ReportId = data.Id,
                        IsDefault = 0,

                    });
                }

                _context.Reports.Add(data);
            }

            try
            {
                var id = data.Id;
                await _context.SaveChangesAsync();
                var primaryKeys = await _context.ReportPrimaryKeys.FirstOrDefaultAsync(p => p.SchemaName ==
                    source.SchemaName &&
                    p.TableName == source.TableName);
                id = data.Id;
                tmp = await _context.Reports.Include(r => r.ReportFields).FirstOrDefaultAsync(r => r.Id == id);
                if (tmp.PKFieldId == 0)
                {
                    var pkfield = tmp.ReportFields.FirstOrDefault(f => f.FieldName == primaryKeys.ColumnName);
                    if (pkfield != null) tmp.PKFieldId = pkfield.Id;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public async Task ChangeReportSubsystems(int reportId, List<int> subsystems)
        {
            if (!subsystems.Any())
            {
                throw new InvalidOperationException("Must select at least one subsystem!");
            }
            var report = _context.Reports.FirstOrDefault(r => r.Id == reportId);
            //    report.ReportOwner = (ECCSAModuleFlag)subsystems.Sum();

            var reportOwner = 0;
            foreach (var subSystem in subsystems)
            {
                reportOwner += subSystem;
            }
            report.ReportOwner = (ECCSAModuleFlag)reportOwner;

            await _context.SaveChangesAsync();

        }

        public async Task ChangeReportField(ReportField data)
        {
            var report = _context.ReportFields.FirstOrDefault(r => r.Id == data.Id);

            if (report == null)
            {
                return;
            }


            report.Caption = data.Caption;
            report.IsGroupField = data.IsGroupField;
            report.IsViewable = data.IsViewable;
            report.IsPercent = data.IsPercent;
            report.IsNumeric = data.IsNumeric;
            report.FieldTypeId = data.FieldTypeId;
            report.DropDownSource = data.DropDownSource ?? string.Empty;
            report.FilterFieldName = data.FilterFieldName ?? string.Empty;
            report.IsFilterField = data.FieldTypeId != FilterFieldType.None;

            await _context.SaveChangesAsync();

        }

        public async Task ChangeEditReport(Report data)
        {
            var report = _context.Reports.FirstOrDefault(r => r.Id == data.Id);

            if (report == null)
            {
                return;
            }

            report.Title = data.Title;
            report.SubTitle = data.SubTitle;
            report.SortOrder = data.SortOrder;

            await _context.SaveChangesAsync();
        }
    }

    public interface IGetLookupType<T> where T : new()
    {
        List<T> GetData(string lookupName);
    }
    public class GetLookupType<T> : IGetLookupType<T> where T : new()
    {

        private readonly EccsaContext _context;
        private readonly string _connectionString;

        public GetLookupType(EccsaContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        public List<T> GetData(string lookupName)
        {
            var sql = $"SELECT * FROM {lookupName}";
            var retList = new List<T>();
            var connString = _connectionString;
            var table = new DataTable();
            using (var da = new NpgsqlDataAdapter(sql, connString))
            {
                da.Fill(table);
                foreach (DataRow row in table.Rows)
                {
                    var ob = new T();
                    var props = typeof(T).GetProperties();

                    for (var i = 0; i < table.Columns.Count; i++)
                    {
                        var val = row[i];
                        var prop = props.FirstOrDefault(p => p.Name.ToLower() == table.Columns[i].ColumnName.Replace("_", "").ToLower());
                        if (prop == null) continue;
                        if (val != DBNull.Value)
                        {
                            prop.SetValue(ob, val);
                        }
                    }
                    retList.Add(ob);
                }
            }


            return retList;
        }

        public List<T> GetDataFromSql(string sql)
        {
            var retList = new List<T>();
            var connString = _connectionString;
            var table = new DataTable();
            using (var da = new NpgsqlDataAdapter(sql, connString))
            {
                da.Fill(table);
                foreach (DataRow row in table.Rows)
                {
                    var ob = new T();
                    var props = typeof(T).GetProperties();

                    for (var i = 0; i < table.Columns.Count; i++)
                    {
                        var val = row[i];
                        var prop = props.FirstOrDefault(p => p.Name.ToLower() == table.Columns[i].ColumnName.Replace("_", "").ToLower());
                        if (prop == null) continue;
                        if (val != DBNull.Value)
                        {
                            prop.SetValue(ob, val);
                        }
                    }
                    retList.Add(ob);
                }
            }


            return retList;
        }

        public DataTable GetDataTable(string sql)
        {
            DataTable datatable = new DataTable();
            var connString = _connectionString;
            using (var connection = new NpgsqlConnection(connString))
            {
                var cmd = new NpgsqlCommand(sql, connection);
                var dataAdapt = new NpgsqlDataAdapter { SelectCommand = cmd };
                dataAdapt.Fill(datatable);
            }
            return datatable;
        }

        public void ExcuteSql(string sql)
        {
            var connString = _connectionString;
            using (var connection = new NpgsqlConnection(connString))
            {
                connection.Open();
                var cmd = new NpgsqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
