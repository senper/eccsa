using System;
using System.Collections.Generic;
using System.Data;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Report.Model
{
    public class PivotedData
    {
        class RetValue
        {
            public RetValue(DataColumn dc, PivotColumn pc)
            {
                DataColumn = dc;
                PivotColumn = pc;
            }

            internal DataColumn DataColumn { get; }
            internal PivotColumn PivotColumn { get; }
        }
        public List<string> PivotColumns { get; set; } = new List<string>();

        public List<PivotColumn> YColumns { get; private set; } = new List<PivotColumn>();

        public List<string> XColumns { get; private set; } = new List<string>();
        public string ValueColumn { get; set; }

        public DataTable Data { get; set; } = new DataTable();

        private DataTable _pivotTable;
        public DataTable PivotTable => _pivotTable ?? (_pivotTable = Transpose());




        private RetValue GetPivotColumn(DataRow dr, int j, int i, PivotColumn parent)
        {
            var c = new PivotColumn
            {
                Name = PivotColumns[i] + j,
                Caption = dr[i].ToString()

            };
            var filter = GetFilter(PivotColumns[i], dr[i]);
            if (!string.IsNullOrEmpty(parent.Filter) && !filter.EndsWith(And))
                filter += And;
            c.Filter = filter + parent.Filter;
            var dc = new DataColumn(c.Name)
            {
                Caption = c.Caption,
                DataType = typeof(string)
            };


            if (parent.Children.ContainsKey(c.Caption))
            {
                return new RetValue(dc, c);
            }
            parent.ChildrenColumns.Add(c);
            parent.Children.Add(c.Caption, c);
            return new RetValue(dc, c);
        }

        private const string And = " AND ";

        private object Compute(DataTable dt, PivotColumn pc, int c, string filter)
        {
            if (!string.IsNullOrEmpty(filter) && !filter.EndsWith(And) && !string.IsNullOrEmpty(pc.ChildrenColumns[c].Filter))
                filter += And;
            filter += pc.ChildrenColumns[c].Filter;
            var rws = dt.Select(filter);
            if (rws.Length == 0)
                return string.Empty;
            return rws[0][ValueColumn];
        }



        private static string GetFilter(string fieldName, object value)
        {
            var sValue = value?.ToString().ToLower();
            if (string.IsNullOrEmpty(sValue)) return string.Empty;
            double x;
            if (sValue == "true" || sValue == "false")
            {
                sValue = (sValue == "true" ? "1" : "0");
            }
            else if (!double.TryParse(sValue, out x))
            {
                sValue = "'" + sValue.Replace("'", "''") + "'";
            }
            return "[" + fieldName + "] = " + sValue;
        }

        private DataTable Transpose()
        {
            var dtSource = Data;
            var zColumn = ValueColumn;
            if (string.IsNullOrEmpty(zColumn) || PivotColumns.Count < 1)
                return dtSource;
            if (dtSource?.Columns == null || dtSource.Columns.Count < 2) //One for Pivot column another for value
                return dtSource;


            DataTable dtX;
            PrepareXColumns(dtSource, zColumn, out dtX);
            var dtY = dtSource.DefaultView.ToTable(true, PivotColumns.ToArray());
            var returnTable = new DataTable("Pivot");
            foreach (var s in XColumns)
                returnTable.Columns.Add(s);
            Dictionary<string, PivotColumn> dic = new Dictionary<string, PivotColumn>();
            var j = 1;

            foreach (DataRow dr in dtY.Rows)
            {
                var pc = new PivotColumn
                {
                    Name = PivotColumns[0] + j,
                    Caption = dr[0].ToString(),
                    Filter = GetFilter(PivotColumns[0], dr[0])
                };
                PivotColumn parentColumn;
                if (dic.ContainsKey(pc.Caption))
                    parentColumn = dic[pc.Caption];
                else
                {
                    parentColumn = pc;
                    dic.Add(pc.Caption, pc);
                    YColumns.Add(pc);
                }
                var ret = new RetValue(null, parentColumn);
                if (dtY.Columns.Count == 1)
                {
                    ret = GetPivotColumn(dr, j, 0, parentColumn);
                    returnTable.Columns.Add(ret.DataColumn);
                    j++;
                }
                for (var i = 1; i < dtY.Columns.Count; i++)
                {
                    ret = GetPivotColumn(dr, j, i, ret.PivotColumn);
                    returnTable.Columns.Add(ret.DataColumn);
                    j++;
                }


            }


            var xColumnCount = dtX.Columns.Count;
            foreach (DataRow drX in dtX.Rows)
            {
                var drReturn = returnTable.NewRow();
                var xFilter = string.Empty;
                for (var xi = 0; xi < xColumnCount; xi++)
                {
                    drReturn[xi] = drX[xi];
                    if (!string.IsNullOrEmpty(xFilter) && !xFilter.EndsWith(And))
                        xFilter += And;
                    xFilter += GetFilter(drReturn.Table.Columns[xi].ColumnName, drX[xi]);
                }
                var colIndex = xColumnCount;
                foreach (var pc in YColumns)
                {
                    for (var c = 0; c < pc.ChildrenColumns.Count; c++)
                    {
                        drReturn[colIndex] = Compute(dtSource, pc, c, xFilter);
                        colIndex++;
                    }
                }

                returnTable.Rows.Add(drReturn);
            }
            return returnTable;

        }



        private void PrepareXColumns(DataTable dtSource, string zColumn, out DataTable dtX)
        {
            XColumns = new List<string>(dtSource.Columns.Count - PivotColumns.Count - 1);
            foreach (DataColumn dc in dtSource.Columns)
            {
                if (string.Equals(zColumn, dc.ColumnName, StringComparison.CurrentCultureIgnoreCase) ||
                    PivotColumns.Exists(c => string.Equals(c, dc.ColumnName, StringComparison.CurrentCultureIgnoreCase)))
                {
                    continue;
                }
                XColumns.Add(dc.ColumnName);
            }


            dtX = dtSource.DefaultView.ToTable(true, XColumns.ToArray());
        }
    }
    public class PivotColumn
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public string Filter { get; set; }
        public List<PivotColumn> ChildrenColumns { get; set; } = new List<PivotColumn>();
        public Dictionary<string, PivotColumn> Children { get; set; } = new Dictionary<string, PivotColumn>();
    }
}
