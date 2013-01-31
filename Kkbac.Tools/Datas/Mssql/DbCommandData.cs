using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Kkbac.Tools.Datas.Mssql
{
    public class DbCommandData
    {
        public DbContext Context { get; set; }
        public StringBuilder Sql { get; private set; }
        public string SqlString
        {
            get
            {
                return Sql.ToString();
            }
        }
        public CommandType DbCommandType { get; set; }
        public List<SqlParameter> Parameters { get; set; }
        public SqlParameter[] ParametersArray
        {
            get
            {
                if (Parameters == null)
                {
                    return null;
                }
                return Parameters.ToArray();
            }
        }
        public SqlDataReader DbSqlDataReader { get; set; }
        public SqlTransaction DbSqlTransaction { get; set; }
        public bool UseTransaction { get; set; }

        public int CommandTimeout
        {
            get
            {
                return SqlHelper.CommandTimeout;
            }
            set
            {
                SqlHelper.CommandTimeout = value;
            }
        }

        public DbCommandData(DbContext context, CommandType commandType)
        {
            Context = context;
            DbCommandType = commandType;
            Parameters = new List<SqlParameter>();
            Sql = new StringBuilder();
            UseTransaction = false;
        }
    }
}