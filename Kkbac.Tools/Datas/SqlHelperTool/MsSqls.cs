using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace Kkbac.Tools.Datas.SqlHelperTool
{
    using Extensions.DataReader;
    using Extensions.Formats;

    public class MsSqls : IDisposable
    {
        #region 字段

        private string _connectionString;
        private CommandType _commandType;
        private string _commandText;
        private List<SqlParameter> _commandParameters;

        private SqlParameter[] _commandParametersArray
        {
            get
            {
                if (_commandParameters == null)
                {
                    return null;
                }
                else
                {
                    return _commandParameters.ToArray();
                }
            }
        }

        #endregion

        #region 属性

        // public bool UseTransaction { get; set; }

        #endregion

        #region 构造方法

        public MsSqls(string connection = null)
        {
            if (string.IsNullOrWhiteSpace(connection) == true)
            {
                _connectionString = ConfigurationManager
                    .ConnectionStrings["connection"].ConnectionString;
            }
            else
            {
                _connectionString = connection;
            }
            _commandType = CommandType.Text;
            _commandText = string.Empty;
            _commandParameters = null;
        }

        #endregion

        #region SqlParameter

        /// <summary>
        /// 存储过程参数[byte]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MsSqls ParameterByte(string parameterName, byte value)
        {
            Parameter(parameterName, value, SqlDbType.TinyInt);

            return this;
        }

        /// <summary>
        /// 存储过程参数[int]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MsSqls ParameterInt(string parameterName, int value)
        {
            Parameter(parameterName, value, SqlDbType.Int);

            return this;
        }

        /// <summary>
        /// 存储过程参数[Long]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MsSqls ParameterLong(string parameterName, long value)
        {
            Parameter(parameterName, value, SqlDbType.BigInt);

            return this;
        }

        /// <summary>
        /// 存储过程参数[DateTime]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MsSqls ParameterDateTime(string parameterName, DateTime value)
        {
            Parameter(parameterName, value, SqlDbType.DateTime);

            return this;
        }

        /// <summary>
        /// 存储过程参数[Guid]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MsSqls ParameterGuid(string parameterName, Guid value)
        {
            Parameter(parameterName, value, SqlDbType.UniqueIdentifier);

            return this;
        }

        /// <summary>
        /// 存储过程参数[bool]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MsSqls ParameterBool(string parameterName, bool value)
        {
            Parameter(parameterName, value, SqlDbType.Bit);

            return this;
        }

        /// <summary>
        /// 存储过程参数[NVarChar]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public MsSqls ParameterNVarChar(string parameterName, string value, int size = 0)
        {
            Parameter(parameterName, value, SqlDbType.NVarChar, size);

            return this;
        }

        /// <summary>
        /// 存储过程参数[Char]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public MsSqls ParameterChar(string parameterName, string value, int size = 0)
        {
            Parameter(parameterName, value, SqlDbType.Char, size);

            return this;
        }

        public MsSqls ParameterOut(string parameterName, SqlDbType sqlDbType, int size = 0)
        {
            Parameter(
                parameterName,
                null,
                sqlDbType,
                size,
                ParameterDirection.Output
            );

            return this;
        }

        public MsSqls Parameter(
            string parameterName,
            object value,
            SqlDbType sqlDbType,
            int size = 0,
            ParameterDirection direction = ParameterDirection.Input
        )
        {
            var par = new SqlParameter();
            if (value == null)
            {
                value = DBNull.Value;
            }
            else if (value.GetType().IsEnum)
            {
                value = (int)value;
            }

            par.ParameterName = parameterName;
            par.Value = value;
            par.SqlDbType = sqlDbType;
            par.Direction = direction;
            if (size > 0)
            {
                par.Size = size;
            }
            if (_commandParameters == null)
            {
                _commandParameters = new List<SqlParameter>();
            }
            _commandParameters.Add(par);

            return this;
        }

        public SqlParameter MakeParameterOut(string parameterName, SqlDbType sqlDbType, int size = 0)
        {
            var pars = MakeParameter(
                parameterName,
                null,
                sqlDbType,
                size,
                ParameterDirection.Output
            );

            return pars;
        }

        public SqlParameter MakeParameter(
            string parameterName,
            object value,
            SqlDbType sqlDbType,
            int size = 0,
            ParameterDirection direction = ParameterDirection.Input
        )
        {
            var par = new SqlParameter();
            if (value == null)
            {
                value = DBNull.Value;
            }
            else if (value.GetType().IsEnum)
            {
                value = (int)value;
            }

            par.ParameterName = parameterName;
            par.Value = value;
            par.SqlDbType = sqlDbType;
            par.Direction = direction;
            if (size > 0)
            {
                par.Size = size;
            }

            return par;
        }

        public MsSqls Parameters(params SqlParameter[] parameters)
        {
            if (parameters == null)
            {
                return this;
            }
            if (_commandParameters == null)
            {
                _commandParameters = new List<SqlParameter>();
            }
            _commandParameters.AddRange(parameters);
            return this;
        }

        #endregion

        #region Sql

        public MsSqls Sql(
            string sql,
            params SqlParameter[] parameters
        )
        {
            ClearSql();
            ClearParameter();

            _commandText = sql;
            Parameters(parameters);

            return this;
        }

        #endregion

        #region Config

        public MsSqls SetCommandTimeout(int commandTimeout)
        {
            SqlHelper.CommandTimeout = commandTimeout;

            return this;
        }

        public MsSqls SetCommandType(CommandType commandType)
        {
            _commandType = commandType;

            return this;
        }

        public MsSqls ClearSql()
        {
            _commandText = string.Empty;
            return this;
        }

        public MsSqls ClearParameter()
        {
            _commandParameters = null;
            return this;
        }

        #endregion

        #region Execute

        public int Execute()
        {
            var i = SqlHelper.ExecuteNonQuery(
                _connectionString,
                _commandType,
                _commandText,
                _commandParametersArray
            );
            return i;
        }

        public object ExecuteScalar()
        {
            var o = SqlHelper.ExecuteScalar(
                _connectionString,
                _commandType,
                _commandText,
                _commandParametersArray
            );
            return o;
        }

        public T ExecuteScalar<T>()
        {
            var o = ExecuteScalar();

            if (o == null || o == DBNull.Value)
            {
                return default(T);
            }

            var t = o.TryChangeType<T>();

            return t;
        }

        /// <summary>
        /// select SCOPE_IDENTITY();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ExecuteReturnLastId<T>()
        {
            if (_commandText.EndsWith(";") == false)
            {
                _commandText += ";";
            }

            _commandText += "select SCOPE_IDENTITY();";

            var t = ExecuteScalar<T>();

            return t;
        }

        #endregion

        #region DataSet

        public DataSet QueryDataSet()
        {
            var ds = SqlHelper.ExecuteDataset(
                _connectionString,
                _commandType,
                _commandText,
                _commandParametersArray
            );
            return ds;
        }

        #endregion

        #region DataTable

        public DataTable QueryDataTable()
        {
            var ds = QueryDataSet();
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        #endregion

        #region SqlDataReader

        public SqlDataReader QueryDataReader()
        {
            var dr = SqlHelper.ExecuteReader(
                _connectionString,
                _commandType,
                _commandText,
                _commandParametersArray
            );
            return dr;
        }

        #endregion

        #region QuerySingle

        public TEntity QuerySingle<TEntity>() where TEntity : new()
        {
            var dr = QueryDataReader();
            if (dr == null)
            {
                return default(TEntity);
            }
            var t = dr.ToList<TEntity>(1);
            if (dr != null && dr.IsClosed == false)
            {
                dr.Close();
            }
            if (t == null || t.Count == 0)
            {
                return default(TEntity);
            }
            else
            {
                return t.FirstOrDefault();
            }
        }
        #endregion

        #region Query

        public List<TEntity> Query<TEntity>() where TEntity : new()
        {
            var dr = QueryDataReader();
            if (dr == null)
            {
                return null;
            }
            var t = dr.ToList<TEntity>();
            if (dr != null && dr.IsClosed == false)
            {
                dr.Close();
            }
            return t;
        }

        public List<TEntity> QueryPaging<TEntity>(
            string select,
            string from,
            string where,
            string orderBy,
            int pageIndex,
            int pageSize,
            params SqlParameter[] parameters
        ) where TEntity : new()
        {
            if (string.IsNullOrWhiteSpace(where) == false)
            {
                where = " where " + where;
            }
            var pageStart = (pageIndex - 1) * pageSize + 1;
            var pageEnd = (pageIndex * pageSize);
            ParameterInt("@pagestart", pageStart);
            ParameterInt("@pageend", pageEnd);
            Parameters(parameters);

            _commandText = string.Format(@"with PagedPersons as
(
    select 
        top 100 percent {0}, 
        row_number() over (order by {1}) as mssql_rownumber
    from
        {2}
    {3}
)
select 
    *
from 
    PagedPersons
where 
    mssql_rownumber between @pagestart and @pageend ",
                select,
                orderBy,
                from,
                where
            );

            var listtentity = Query<TEntity>();

            return listtentity;
        }

        public List<TEntity> QueryPaging<TEntity>(
            string select,
            string from,
            string where,
            string orderBy,
            int pageIndex,
            int pageSize,
            out int totalrows,
            params SqlParameter[] parameters
        ) where TEntity : new()
        {
            if (string.IsNullOrWhiteSpace(where) == false)
            {
                where = " where " + where;
            }
            var pageStart = (pageIndex - 1) * pageSize + 1;
            var pageEnd = (pageIndex * pageSize);
            ParameterInt("@pagestart", pageStart);
            ParameterInt("@pageend", pageEnd);
            Parameters(parameters);
            var pars = MakeParameterOut("@totalrows", SqlDbType.Int);
            Parameters(pars);

            _commandText = string.Format(@"with PagedPersons as
(
    select 
        top 100 percent {0}, 
        row_number() over (order by {1}) as mssql_rownumber
    from
        {2}
    {3}
)
select 
    *
from 
    PagedPersons
where 
    mssql_rownumber between @pagestart and @pageend ;
select @totalrows = count(0) from {2} {3}",
                select,
                orderBy,
                from,
                where
            );

            var listtentity = Query<TEntity>();
            totalrows = (int)pars.Value;
            return listtentity;
        }
        #endregion

        #region Tools

        /// <summary>
        /// 数据库是否存在.
        /// </summary>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public bool HasDataBase(string dbname)
        {
            var sql = " select top 1 1 from master.dbo.sysdatabases where name = @name ";
            var i = Sql(sql)
                .ParameterNVarChar("@name", dbname)
                .ExecuteScalar<int>();

            var b = i > 0;
            return b;
        }

        /// <summary>
        /// 大批量插入.
        /// 表名: dt.TableName
        /// </summary>
        /// <param name="dataTable"></param>
        public void BulkCopy(DataTable dataTable)
        {
            var sqlConnection = new SqlConnection(_connectionString);
            var bulkCopy = new SqlBulkCopy(sqlConnection);
            bulkCopy.DestinationTableName = dataTable.TableName;
            bulkCopy.BatchSize = dataTable.Rows.Count;

            try
            {
                sqlConnection.Open();
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    bulkCopy.WriteToServer(dataTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConnection.Close();
                if (bulkCopy != null)
                {
                    bulkCopy.Close();
                }
            }
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
