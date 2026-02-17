using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.ExternalData
{
    public class ExternalDataType : Entity
    {
        public String SourceName { get; set; }
        public String SchemaName { get; set; } = "public";
        public string Caption { get; set; }
       
        public DataTable GetDataTable()
        {
            DataTable table = new DataTable($"{SourceName}{DateTime.Today.ToString("yyyy-MM-dd")}");
            foreach(var md in MetaData)
            {
                DataColumn dc = new DataColumn(md.ColumnName);
                
                dc.Caption = md.Caption;
                switch (md.DataType)
                {
                    case DataTypeEnum.Integer:
                        dc.DataType = typeof(int);
                        break;
                    case DataTypeEnum.Boolean:
                        dc.DataType = typeof(bool);
                        break;
                    case DataTypeEnum.Double:
                        dc.DataType = typeof(double);
                        break;
                    case DataTypeEnum.Date:
                        dc.DataType = typeof(DateTime);
                        break;
                    case DataTypeEnum.Decimal:
                        dc.DataType = typeof(decimal);
                        break;
                    case DataTypeEnum.Guid:
                        dc.DataType = typeof(Guid);
                        break;
                    default:
                        dc.DataType = typeof(string);
                        break;
                }
                table.Columns.Add(dc);
            }
            return table;
        }
        public List<ExternalMetaData> MetaData { get; set; }

        public List<Period> DataPeriods { get; set; }

        public bool PeriodExists(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;
            return DataPeriods != null &&
                     DataPeriods.Count > 0 &&
                     DataPeriods.Any(p => p.StartDate < endDate && p.EndDate >= startDate);
        }
    }
    public class Period
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class ExternalMetaData
    {
        public string ColumnName { get; set; }

        public string ExcelHeaderName { get; set; }
        public string Caption { get; set; }
        public bool ShowInGrid { get; set; }

        public DataTypeEnum DataType { get; set; }
        public bool NotNull { get; set; }
        public int MaxSize { get; set; }
        public string DefaultValue { get; set; }

        [NotMapped]
        public DataColumn DataColumn { get; set; }
         
    }
    public class PingReply
    {
        public DateTime AttemptDate { get; set; }
        public bool Successful { get; set; }
    }
    public enum DataTypeEnum
    {
        String = 0,
        Date = 1,
        Integer = 2,
        Decimal = 3,
        Double = 4,
        Boolean = 5,
        Guid = 6
    }
}
