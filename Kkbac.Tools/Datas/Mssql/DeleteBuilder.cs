using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Kkbac.Tools.Datas.Mssql
{
    public class DeleteBuilder
    {
        public BuilderData Data { get; set; }

        public DeleteBuilder(DbCommand dbCommand, string tableName)
        {
            Data = new BuilderData(dbCommand, tableName);
        }

        public DeleteBuilder Where(
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

        private DbCommand GetPreparedCommand()
        {
            Data.Command.Clear();

            if (Data.Where == null || Data.Where.Count == 0)
            {
                throw new ArgumentNullException("条件列表不能为空.");
            }

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

            var sql = string.Format("delete from {0} where {1} ",
                Data.TableName,
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