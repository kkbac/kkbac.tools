using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Kkbac.Tools.Datas.Mssql
{
    public class BuilderColumn
    {
        public string ColumnName { get; set; }
        public object Value { get; set; }
        public SqlDbType SqlDbType { get; set; }
        public int Size { get; set; }

        /// <summary>
        /// 加括号 [ColumnName]
        /// </summary>
        public string ColumnNameFormat
        {
            get
            {
                return string.Format("[{0}]", ColumnName);
            }
        }

        /// <summary>
        /// 加@ @ColumnName
        /// </summary>
        public string ParameterName
        {
            get
            {
                return string.Format("@{0}", ColumnName);
            }
        }

        public BuilderColumn(
            string columnName,
            object value,
            SqlDbType sqlDbType,
            int size
        )
        {
            ColumnName = columnName;
            Value = value;
            SqlDbType = sqlDbType;
            Size = size;
        }
    }
}