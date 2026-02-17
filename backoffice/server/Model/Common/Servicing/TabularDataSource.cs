using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class TabularDataSource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TabularDataSourceDetail> Details { get; set; }
    }
    public class TabularDataSourceDetail
    {
        public Guid Id { get; set; }
        public Guid TabularDataSourceId { get; set; }
        public virtual TabularDataSource TabularDataSource { get; set; }
        public string No { get; set; }
        public string Caption { get; set; }
        public string Name { get; set; }
        public TabularDataSourceDetailDataType DataType { get; set; }
        [NotMapped]
        public int DataTypeValue => (int)DataType;
        public int MaxLength { get; set; }
        public bool ShowOnReport { get; set; }
        public Guid? LookupId { get; set; }
        public StatusEnum Status { get; set; }
    }
    public class TabularResponse
    {
        public int RowId { get; set; }
        public Guid Text { get; set; }
        public string Value { get; set; }
    }
}
