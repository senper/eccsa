using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Reports
{
    public class ReportPrimaryKey
    {
        public string SourceName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }
    public class ReportSource
    {
        public string SourceName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public List<SourceField> SourceFields { get; set; }
    }

    public class SourceField
    {
        public string SourceName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public ReportSource Source { get; set; }
    }
    public class Report
    {
        public Report()
        {
            ReportFields = new HashSet<ReportField>();
        }

        public int Id { get; set; }
 
        public string Title { get; set; }

        public ReportTypeEnum ReportTypeId { get; set; }

        public string SourceName { get; set; }

        public string SubTitle { get; set; }

        public string FileName { get; set; }

        public string GroupName { get; set; }

        public string SortOrder { get; set; }

        public bool? IsActive { get; set; }
        public string DefaultValueKey { get; set; }
        [NotMapped]
        public bool HasSpatial =>
            SpatialComponent != DrillDownComponent.None &&
            !string.IsNullOrEmpty(SpatialSource) &&
            SpatialSource.Length > 3;
        [NotMapped]
        public bool HasTrend
            => TrendComponent != DrillDownComponent.None &&
            !string.IsNullOrEmpty(TrendSource) &&
            TrendSource.Length > 3;

        [NotMapped]
        public string LiClass
        {
            get
            {
                if (HasTrend && HasSpatial)
                    return "fa fa-bar-chart fa-map-marker";
                if (HasTrend)
                    return "fa fa-bar-chart";
                if (HasSpatial)
                    return "fa fa-map-marker";
                return string.Empty;
            }
        }

        public DrillDownComponent SpatialComponent { get; set; }

        public DrillDownComponent TrendComponent { get; set; }

        public string SpatialSource { get; set; }

        public string TrendSource { get; set; }

        public int PKFieldId { get; set; }

        public ECCSAModuleFlag ReportOwner { get; set; }
        public string ServiceOwnerFieldName { get; set; }

        public virtual ICollection<ReportField> ReportFields { get; set; }
    }

}
