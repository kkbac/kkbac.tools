using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Kkbac.Tools.Datas.Mssql
{
    using Extensions.DataReader;

    public class DbCommand : IDisposable
    {
        #region 属性

        public DbCommandData Data { get; set; }

        #endregion

        #region 构造函数

        public DbCommand(DbContext dbContext, CommandType commandType = CommandType.Text)
        {
            Data = new DbCommandData(dbContext, commandType);
        }

        #endregion

        #region SqlParameter

        #region byte
        /// <summary>
        /// 存储过程参数[byte]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbCommand ParameterByte(string parameterName, byte value)
        {
            Parameter(parameterName, value, SqlDbType.TinyInt);

            return this;
        }
        #endregion

        #region int

        /// <summary>
        /// 存储过程参数[int]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbCommand ParameterInt(string parameterName, int value)
        {
            Parameter(parameterName, value, SqlDbType.Int);

            return this;
        }

        #endregion

        #region long

        /// <summary>
        /// 存储过程参数[Long]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbCommand ParameterLong(string parameterName, long value)
        {
            Parameter(parameterName, value, SqlDbType.BigInt);

            return this;
        }

        #endregion

        #region datetime

        /// <summary>
        /// 存储过程参数[DateTime]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbCommand ParameterDateTime(string parameterName, DateTime value)
        {
            Parameter(parameterName, value, SqlDbType.DateTime);

            return this;
        }

        #endregion

        #region guid

        /// <summary>
        /// 存储过程参数[Guid]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbCommand ParameterGuid(string parameterName, Guid value)
        {
            Parameter(parameterName, value, SqlDbType.UniqueIdentifier);

            return this;
        }

        #endregion

        #region bool

        /// <summary>
        /// 存储过程参数[bool]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbCommand ParameterBool(string parameterName, bool value)
        {
            Parameter(parameterName, value, SqlDbType.Bit);

            return this;
        }

        #endregion

        #region nvarchar

        /// <summary>
        /// 存储过程参数[NVarChar]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbCommand ParameterNVarChar(string parameterName, string value, int size = 0)
        {
            Parameter(parameterName, value, SqlDbType.NVarChar, size);

            return this;
        }

        #endregion

        #region char

        /// <summary>
        /// 存储过程参数[Char]
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbCommand ParameterChar(string parameterName, string value, int size = 0)
        {
            Parameter(parameterName, value, SqlDbType.Char, size);

            return this;
        }

        #endregion

        #region out

        public DbCommand ParameterOut(string parameterName, SqlDbType sqlDbType, int size = 0)
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

        #endregion

        #region parameter(s)

        public DbCommand Parameters(params SqlParameter[] parameters)
        {
            if (parameters == null)
            {
                return this;
            }
            if (Data.Parameters == null)
            {
                Data.Parameters = new List<SqlParameter>();
            }
            Data.Parameters.AddRange(parameters);
            return this;
        }

        public DbCommand Parameter(
            string parameterName,
            object value,
            SqlDbType sqlDbType,
            int size = 0,
            ParameterDirection direction = ParameterDirection.Input
        )
        {
            var par = MakeParameter(parameterName, value, sqlDbType, size, direction);

            if (Data.Parameters == null)
            {
                Data.Parameters = new List<SqlParameter>();
            }
            Data.Parameters.Add(par);

            return this;
        }

        #endregion

        #region makeparameter or out

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

        #endregion

        #endregion

        #region Sql

        /// <summary>
        /// append
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbCommand Sql(string sql)
        {
            Data.Sql.Append(sql);

            return this;
        }

        #endregion

        #region Execute

        public int Execute()
        {
            var i = 0;
            if (Data.UseTransaction == true)
            {
                i = SqlHelper.ExecuteNonQuery(
                    Data.DbSqlTransaction,
                    Data.DbCommandType,
                    Data.SqlString,
                    Data.ParametersArray
                );
            }
            else
            {
                i = SqlHelper.ExecuteNonQuery(
                  Data.Context.Data.ConnectionString,
                  Data.DbCommandType,
                  Data.SqlString,
                  Data.ParametersArray
              );
            }
            return i;
        }

        public object ExecuteScalar()
        {
            object o;
            if (Data.UseTransaction == true)
            {
                o = SqlHelper.ExecuteScalar(
                    Data.DbSqlTransaction,
                    Data.DbCommandType,
                    Data.SqlString,
                    Data.ParametersArray
                );
            }
            else
            {
                o = SqlHelper.ExecuteScalar(
                    Data.Context.Data.ConnectionString,
                    Data.DbCommandType,
                    Data.SqlString,
                    Data.ParametersArray
                );
            }
            return o;
        }

        public T ExecuteScalar<T>()
        {
            var value = ExecuteScalar();

            if (value == null || value == DBNull.Value)
            {
                return default(T);
            }

            T scalarId;

            if (value.GetType() == typeof(T))
            {
                scalarId = (T)value;
            }
            else
            {
                scalarId = (T)Convert.ChangeType(value, typeof(T));
            }

            return scalarId;
        }

        /// <summary>
        /// select SCOPE_IDENTITY();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ExecuteReturnLastId<T>()
        {
            if (Data.Sql[Data.Sql.Length - 1] != ';')
            {
                Data.Sql.Append(";");
            }

            Data.Sql.Append("select SCOPE_IDENTITY();");

            var t = ExecuteScalar<T>();

            return t;
        }

        #endregion

        #region Query

        public List<TEntity> Query<TEntity>() where TEntity : new()
        {
            var t = QueryDataReader.ToList<TEntity>();

            CloseDataReader();

            return t;
        }

        public Tuple<List<TEntity>, int> QueryPaging<TEntity>(
            string select,
            string from,
            string where,
            string orderBy,
            int pageIndex,
            int pageSize,
            bool isGetTotalRows,
            params SqlParameter[] parameters
        ) where TEntity : new()
        {
            Clear();

            if (string.IsNullOrWhiteSpace(where) == false)
            {
                where = " where " + where;
            }
            var pageStart = (pageIndex - 1) * pageSize + 1;
            var pageEnd = (pageIndex * pageSize);
            ParameterInt("@pagestart", pageStart);
            ParameterInt("@pageend", pageEnd);
            Parameters(parameters);

            Data.Sql.AppendFormat(@"with PagedPersons as
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

            var totalrows = 0;
            var listtentity = new List<TEntity>();
            if (isGetTotalRows == true)
            {
                var parsout = MakeParameterOut("@totalrows", SqlDbType.Int);
                Parameters(parsout);
                Data.Sql.AppendFormat(@";select @totalrows = count(0) from {0} {1}"
                    , from, where);
                listtentity = Query<TEntity>();
                totalrows = (int)parsout.Value;
            }
            else
            {
                listtentity = Query<TEntity>();
            }
            var tuple = new Tuple<List<TEntity>, int>(listtentity, totalrows);

            return tuple;
        }

        #endregion

        #region QuerySingle

        public TEntity QuerySingle<TEntity>() where TEntity : new()
        {
            var t = QueryDataReader.ToList<TEntity>(1);
            CloseDataReader();

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

        #region DataReader

        private SqlDataReader QueryDataReader
        {
            get
            {
                if (Data.UseTransaction == true)
                {
                    Data.DbSqlDataReader = SqlHelper.ExecuteReader(
                        Data.DbSqlTransaction,
                        Data.DbCommandType,
                        Data.SqlString,
                        Data.ParametersArray
                    );
                }
                else
                {
                    Data.DbSqlDataReader = SqlHelper.ExecuteReader(
                        Data.Context.Data.ConnectionString,
                        Data.DbCommandType,
                        Data.SqlString,
                        Data.ParametersArray
                    );
                }
                return Data.DbSqlDataReader;
            }
        }

        private void CloseDataReader()
        {
            if (Data.DbSqlDataReader != null
                &&
                Data.DbSqlDataReader.IsClosed == false)
            {
                Data.DbSqlDataReader.Close();
            }
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

        #region DataSet

        public DataSet QueryDataSet()
        {
            DataSet dataSet;
            if (Data.UseTransaction == true)
            {
                dataSet = SqlHelper.ExecuteDataset(
                    Data.DbSqlTransaction,
                    Data.DbCommandType,
                    Data.SqlString,
                    Data.ParametersArray
                );
            }
            else
            {
                dataSet = SqlHelper.ExecuteDataset(
                    Data.Context.Data.ConnectionString,
                    Data.DbCommandType,
                    Data.SqlString,
                    Data.ParametersArray
                );
            }
            return dataSet;
        }

        #endregion

        #region StoredProcedure

        public DbCommand StoredProcedure(string sql)
        {
            Data.DbCommandType = CommandType.StoredProcedure;

            Data.Sql.Append(sql);

            return this;
        }

        #endregion

        #region Set

        public DbCommand SetCommandType(CommandType commandType)
        {
            Data.DbCommandType = commandType;

            return this;
        }

        public DbCommand SetCommandTimeout(int timeout)
        {
            Data.CommandTimeout = timeout;

            return this;
        }

        #endregion

        #region Clear

        public DbCommand ClearSql()
        {
            Data.Sql.Clear();

            return this;
        }

        public DbCommand ClearParameter()
        {
            Data.Parameters.Clear();

            return this;
        }

        /// <summary>
        /// sql, pars, commandtype
        /// </summary>
        /// <returns></returns>
        public DbCommand Clear()
        {
            ClearSql();
            ClearParameter();
            Data.DbCommandType = CommandType.Text;

            return this;
        }
        #endregion

        #region Transaction

        public DbCommand UseTransaction
        {
            get
            {
                Data.UseTransaction = true;

                var connection = new SqlConnection(Data.Context.Data.ConnectionString);

                Data.DbSqlTransaction = connection.BeginTransaction();

                return this;
            }
        }

        public void Commit()
        {
            if (Data.DbSqlTransaction == null)
            {
                return;
            }
            try
            {
                Data.DbSqlTransaction.Commit();
                Data.UseTransaction = false;
            }
            catch (Exception ex1)
            {
                try
                {
                    Data.DbSqlTransaction.Rollback();
                }
                catch (Exception ex2)
                {
                }
            }
            finally
            {
                CloseTransaction();
            }
        }

        private void CloseTransaction()
        {
            if (Data.DbSqlTransaction == null)
            {
                return;
            }

            Data.DbSqlTransaction.Dispose();

            Data.DbSqlTransaction.Connection.Close();

        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            CloseDataReader();

            CloseTransaction();
        }

        #endregion

    }
}