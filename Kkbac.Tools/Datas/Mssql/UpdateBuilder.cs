using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Kkbac.Tools.Datas.Mssql
{
    public class UpdateBuilder
    {
        public BuilderData Data { get; set; }

        public UpdateBuilder(DbCommand dbCommand, string tableName)
        {
            Data = new BuilderData(dbCommand, tableName);
        }

        public UpdateBuilder Where(
           string columnName,
           object value,
           SqlDbType parameterType,
           int size = 0
        )
        {
            var column = new BuilderColumn(columnName, value, parameterType, size);
            Data.Where.Add(column);

            return this;
        }

        public UpdateBuilder Column(
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

        private DbCommand GetPreparedCommand()
        {
            Data.Command.Clear();

            if (Data.Columns == null || Data.Columns.Count == 0)
            {
                throw new ArgumentNullException("字段列表不能为空.");
            }

            if (Data.Where == null || Data.Where.Count == 0)
            {
                throw new ArgumentNullException("条件列表不能为空.");
            }

            var setSql = string.Join(",",
                Data.Columns.Select(x => string.Format("{0}={1}",
                    x.ColumnNameFormat,
                    x.ParameterName
                ))
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

            var where = string.Join(" and ",
                Data.Where.Select(x => string.Format("{0}={1}",
                    x.ColumnNameFormat,
                    x.ParameterName
                ))
            );
            Data.Where.ForEach(x =>
            {
                Data.Command.Parameter(
                    x.ParameterName,
                    x.Value,
                    x.SqlDbType,
                    x.Size
                );
            });

            var sql = string.Format("update {0} set {1} where {2}",
                Data.TableName,
                setSql,
                where
            );
            Data.Command.Sql(sql);

            return Data.Command;
        }

        public int Execute()
        {
            return GetPreparedCommand().Execute();
        }
    }
}