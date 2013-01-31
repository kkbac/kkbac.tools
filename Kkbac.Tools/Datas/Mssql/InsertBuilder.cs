using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Kkbac.Tools.Datas.Mssql
{
    public class InsertBuilder
    {
        public BuilderData Data { get; set; }

        public InsertBuilder(DbCommand dbCommand, string tableName)
        {
            Data = new BuilderData(dbCommand, tableName);
        }

        #region Column

        public InsertBuilder ColumnNVarChar(string columnName, string value, int size)
        {
            return Column(columnName, value, SqlDbType.NVarChar, size);
        }

        public InsertBuilder ColumnLong(string columnName, long value)
        {
            return Column(columnName, value, SqlDbType.BigInt);
        }

        public InsertBuilder ColumnInt(string columnName, int value)
        {
            return Column(columnName, value, SqlDbType.Int);
        }

        public InsertBuilder Column(
            string columnName,
            object value,
            SqlDbType parameterType,
            int size = 0
        )
        {
            var column = new BuilderColumn(columnName, value, parameterType, size);
            Data.Columns.Add(column);

            return this;
        }

        #endregion

        private DbCommand GetPreparedCommand()
        {
            Data.Command.Clear();

            if (Data.Columns == null || Data.Columns.Count == 0)
            {
                throw new ArgumentNullException("字段列表不能为空.");
            }

            var insertSql = string.Join(",",
                Data.Columns.Select(x => x.ColumnNameFormat)
            );

            var valuesSql = string.Join(",",
                Data.Columns.Select(x => x.ParameterName)
            );

            Data.Columns.ForEach(x =>
            {
                Data.Command.Parameter(
                    x.ParameterName,
                    x.Value,
                    x.SqlDbType,
                    x.Size
                );
            });

            var sql = string.Format("insert into {0} ({1}) values ({2})",
                Data.TableName,
                insertSql,
                valuesSql
            );
            Data.Command.Sql(sql);

            return Data.Command;
        }

        public int Execute()
        {
            return GetPreparedCommand().Execute();
        }

        public T ExecuteReturnLastId<T>(string identityColumnName = null)
        {
            return GetPreparedCommand().ExecuteReturnLastId<T>();
        }
    }
}