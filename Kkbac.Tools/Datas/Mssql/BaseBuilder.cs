using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Kkbac.Tools.Datas.Mssql
{
    public abstract class BaseBuilder
    {
        public BuilderData Data { get; set; }

        public BaseBuilder(DbCommand dbCommand, string tableName)
        {
            Data = new BuilderData(dbCommand, tableName);

        }

        public BaseBuilder Where(
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

        #region Column

        public BaseBuilder ColumnChar(string columnName, string value, int size = 0)
        {
            if (size > 8000)
            {
                size = 8000;
            }
            return Column(columnName, value, SqlDbType.Char, size);
        }

        public BaseBuilder ColumnGuid(string columnName, Guid value)
        {
            return Column(columnName, value, SqlDbType.UniqueIdentifier);
        }

        public BaseBuilder ColumnBool(string columnName, bool value)
        {
            return Column(columnName, value, SqlDbType.Bit);
        }

        public BaseBuilder ColumnVarChar(string columnName, string value, int size = 0)
        {
            if (size > 8000)
            {
                size = 8000;
            }
            return Column(columnName, value, SqlDbType.VarChar, size);
        }

        public BaseBuilder ColumnNVarChar(string columnName, string value, int size = 0)
        {
            if (size > 4000)
            {
                size = 4000;
            }
            return Column(columnName, value, SqlDbType.NVarChar, size);
        }

        public BaseBuilder ColumnLong(string columnName, long value)
        {
            return Column(columnName, value, SqlDbType.BigInt);
        }

        public BaseBuilder ColumnInt(string columnName, int value)
        {
            return Column(columnName, value, SqlDbType.Int);
        }

        public BaseBuilder Column(
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

        public int Execute()
        {
            return GetPreparedCommand().Execute();
        }

        public T ExecuteReturnLastId<T>(string identityColumnName = null)
        {
            return GetPreparedCommand().ExecuteReturnLastId<T>();
        }

        public abstract DbCommand GetPreparedCommand();

    }
}