using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.ExternalData;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.Bulk;

namespace ECCSA.BackOffice.Server.Persistence.Repositories
{
    public static class DbContextExtensions
    {
        static readonly SemaphoreSlim semaphoreSlim = new(1, 1);
        /*
         * need
            Microsoft.EntityFrameworkCore
            Microsoft.EntityFrameworkCore.Relational
         */
        public static DataTable DataTable(this DbContext context,
            string sqlQuery, params DbParameter[] parameters)
        {
            DataTable dataTable = new();
            DbConnection connection = context.Database.GetDbConnection();
            var connString = connection.ConnectionString;
            using (var da = new NpgsqlDataAdapter(sqlQuery, connString))
            {
                da.Fill(dataTable);
            }
            if(dataTable.Rows.Count > 0) return dataTable;
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using DbDataAdapter adapter = dbFactory.CreateDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable);
            }
            return dataTable;
        }


        public async static Task BulkInsert(this DbContext context
            , ExternalDataType externalDataType

            , DataTable data)
        {


            var columnDictionary = new Dictionary<string, ExternalMetaData>();

            string schema = externalDataType.SchemaName;
            string tableName = externalDataType.SourceName;

            foreach (DataColumn dc in data.Columns)
            {
                var mdata = externalDataType.MetaData.FirstOrDefault(m => 
                    m.ColumnName.ToLowerInvariant()
                        .Replace("_", "")
                        .Replace(" ", "")
                        .Trim() == dc.ColumnName.ToLowerInvariant()
                        .Replace("_", "")
                        .Replace(" ", "")
                        .Trim());
                if (mdata == null) continue;
                mdata.DataColumn = dc;
                columnDictionary.Add(dc.ColumnName, mdata);
            }
            var count = 0;
            using DbConnection connection = context.Database.GetDbConnection();
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            connection.Open();
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Empty;
                foreach (DataRow dr in data.Rows)
                {
                    string values = string.Empty;
                    foreach (string key in columnDictionary.Keys)
                    {
                        if (!string.IsNullOrEmpty(values))
                            values += " , ";
                        values += GetValue(columnDictionary[key], dr);
                    }

                    cmd.CommandText += $"INSERT INTO {schema}.{tableName} VALUES({values});";

                    await semaphoreSlim.WaitAsync();
                    try
                    {
                        if (count > 100)
                        {
                            count = 0;
                            await cmd.ExecuteNonQueryAsync();
                            cmd.CommandText = string.Empty;
                        }
                        count++;
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                }
                if (!string.IsNullOrEmpty(cmd.CommandText))
                    await cmd.ExecuteNonQueryAsync();

            }
            connection.Close();
            await connection.DisposeAsync();

        }
        private static string GetValue(ExternalMetaData metadata, DataRow dr)
        {
            var dc = metadata.DataColumn;
            var val = $"{dr[dc]}";
            if (dr.IsNull(dc))
            {
                if (dc.ColumnName.ToLowerInvariant() == "id")
                {
                    return $"'{Guid.NewGuid()}'";
                }
                return metadata.DefaultValue;
            }
            switch (metadata.DataType)
            {
                case DataTypeEnum.Date:
                    return $"'{DateTime.Parse(val):yyyy-MM-dd}'";
                case DataTypeEnum.Integer:
                case DataTypeEnum.Decimal:
                case DataTypeEnum.Double:
                    return val;
                case DataTypeEnum.Boolean:
                    return bool.Parse(val) ? "t" : "f";
                case DataTypeEnum.Guid:
                    return $"'{Guid.Parse(val)}'";
                default:
                    if (metadata.MaxSize < val.Length)
                    {
                        val = val[..(metadata.MaxSize - 1)];
                    }
                    return $"'{val}'";

            }


        }
        public async static Task BulkInsert<T>(this DbContext context, List<T> list)
        {
            var uploader = new NpgsqlBulkUploader(context);
            await uploader.InsertAsync(list);
            await context.SaveChangesAsync();
        }

        public async static Task ExecuteSql(this DbContext context, DbConnection connection, string sql)
        {
            
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();
            }
            
            
        }
    }
}