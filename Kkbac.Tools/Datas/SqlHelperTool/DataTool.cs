using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Kkbac.Tools.Datas.SqlHelperTool
{
    using Extensions.DataReader;
    using Extensions.Formats;

    public class DataTool
    {
        #region 属性
        private string connectionString;
        /// <summary>
        /// @
        /// </summary>
        private string parameterLeft = "@";
        /// <summary>
        /// [
        /// </summary>
        private string columnLeft = "[";
        /// <summary>
        /// ]
        /// </summary>
        private string columnRight = "]";
        #endregion

        #region 构造方法
        public DataTool(string connection = null)
        {
            if (string.IsNullOrWhiteSpace(connection) == true)
            {
                connectionString =
                    ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
            }
            else
            {
                connectionString = connection;
            }

        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行指定连接字符串, 返回受影响的行数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(
            string commandText,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            int i = SqlHelper.ExecuteNonQuery(
                connectionString,
                commandType,
                commandText,
                commandParameters);
            return i;
        }

        /// <summary>
        /// 使用事务执行sql.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(
            SqlTransaction sqlTransaction,
            string commandText,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        { 
            int i = SqlHelper.ExecuteNonQuery(
                sqlTransaction,
                commandType,
                commandText,
                commandParameters);
            return i;
        }

        #endregion

        #region ExecuteScalar: 返回结果集中的第一行第一列

        /// <summary>
        /// 返回结果集中的第一行第一列(Long类型)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public long ExecuteScalarLong(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            var l = ExecuteScalar(sql, commandParameters, commandType).ToLong();
            return l;
        }

        /// <summary>
        /// 返回结果集中的第一行第一列(Int类型)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteScalarInt(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            return ExecuteScalar(sql, commandParameters, commandType).ToInt();
        }

        /// <summary>
        /// 返回结果集中的第一行第一列(Object类型)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            object o = SqlHelper.ExecuteScalar(
                connectionString,
                commandType,
                sql,
                commandParameters);
            return o;
        }

        /// <summary>
        /// 返回结果集中的第一行第一列(泛型)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            object o = ExecuteScalar(
                sql,
                commandParameters,
                commandType
            );
            var result = o.TryChangeType<T>();
            return result;
        }

        #endregion

        #region  ExecuteDataset: Select 多行或单行.

        /// <summary>
        /// 通过DataSet转换; Table[0]记录数为0:null,
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public T GetSingleByRow<T>(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        ) where T : class ,new()
        {
            var t = GetRow(sql, commandParameters, commandType).ToModel<T>();
            return t;
        }

        /// <summary>
        /// 通过DataSet转换; Table[0]记录数为0:null,
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataRow GetRow(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            DataTable dt = GetTable(sql, commandParameters, commandType);
            DataRow dr = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                dr = dt.Rows[0];
            }
            return dr;
        }

        /// <summary>
        /// 通过DataSet转换; 非select语句:null,
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public List<T> GetListByTable<T>(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        ) where T : class, new()
        {
            var t = GetTable(sql, commandParameters, commandType).ToList<T>();
            return t;
        }

        /// <summary>
        /// 通过DataSet转换; 非select语句:null,
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable GetTable(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            DataSet ds = GetDataSet(sql, commandParameters, commandType);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public DataSet GetDataSet(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            DataSet ds = SqlHelper.ExecuteDataset(
                connectionString,
                commandType,
                sql,
                commandParameters
            );
            return ds;
        }

        #endregion

        #region ExecuteReader: Select 单行或多行.

        /// <summary>
        /// 通过DataReader转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public T GetSingleByDataReader<T>(
            string sql,
            SqlParameter[] commandParameters = null,
            CommandType commandType = CommandType.Text
        ) where T : class, new()
        {
            var dr = GetDataReader(sql, commandParameters, commandType);
            if (dr == null)
            {
                return null;
            }
            var t = dr.ToList<T>(1);
            if (dr != null && dr.IsClosed == false)
            {
                dr.Close();
            }
            if (t == null || t.Count == 0)
            {
                return null;
            }
            else
            {
                return t.FirstOrDefault();
            }
        }

        public List<T> GetListByDataReader<T>(
                  string sql,
                  SqlParameter[] commandParameters = null,
                  CommandType commandType = CommandType.Text
              ) where T : class, new()
        {
            var dr = GetDataReader(sql, commandParameters, commandType);
            if (dr == null)
            {
                return null;
            }
            var t = dr.ToList<T>();
            if (dr != null && dr.IsClosed == false)
            {
                dr.Close();
            }
            return t;
        }

        public IDataReader GetDataReader(
                  string sql,
                  SqlParameter[] commandParameters = null,
                  CommandType commandType = CommandType.Text
              )
        {
            var dr = SqlHelper.ExecuteReader(
                connectionString,
                commandType,
                sql,
                commandParameters
            );
            return dr;
        }

        #endregion

        #region Insert 方法

        /// <summary>
        /// 返回-1: 失败;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int Insert<T>(T entity, string tableName = "")
            where T : class,new()
        {
            DbEntityInfo dbEntityInfo = GetEntityInfo<T>(entity, tableName);

            if (dbEntityInfo == null
                ||
                dbEntityInfo.ColumnNormals.Count == 0
            )
            {
                return -1;
            }

            var sql = string.Format(@"INSERT INTO {0} ({1}) values ({2})",
                dbEntityInfo.TableName,
                string.Join(",", dbEntityInfo.ColumnNormals.Select(x => string.Format("{0}{1}{2}", columnLeft, x, columnRight))),
                string.Join(",", dbEntityInfo.ColumnNormals.Select(x => string.Format("{0}{1}", parameterLeft, x)))
            );
            Console.WriteLine(sql);

            var i = ExecuteNonQuery(sql, dbEntityInfo.Parameters.ToArray());

            return i;
        }

        #endregion

        #region Update 方法

        public int Update<T>(T entity, string tableName = "")
            where T : class,new()
        {
            DbEntityInfo dbEntityInfo = GetEntityInfo<T>(entity, tableName);
            if (dbEntityInfo == null
                ||
                dbEntityInfo.ColumnKeys.Count == 0
                ||
                dbEntityInfo.ColumnNormals.Count == 0
            )
            {
                return -1;
            }

            var sql = string.Format(@"UPDATE {0} SET {1} where {2}",
                dbEntityInfo.TableName,
                string.Join(",", dbEntityInfo.ColumnNormals.Select(x => string.Format("{0}{1}{2}={3}{1}", columnLeft, x, columnRight, parameterLeft))),
                string.Join(" and ", dbEntityInfo.ColumnKeys.Select(x => string.Format("{0}{1}{2}={3}{1}", columnLeft, x, columnRight, parameterLeft)))
            );
            Console.WriteLine(sql);
            var i = ExecuteNonQuery(sql, dbEntityInfo.Parameters.ToArray());

            return i;
        }

        #endregion

        #region Delete 方法

        public int Delete<T>(T entity, string tableName = "")
           where T : class,new()
        {
            DbEntityInfo dbEntityInfo = GetEntityInfo<T>(entity, tableName);
            if (dbEntityInfo == null
                ||
                dbEntityInfo.ColumnKeys.Count == 0
            )
            {
                return -1;
            }
            var sql = string.Format(@"DELETE {0} where {1}",
                dbEntityInfo.TableName,
                string.Join(" and ", dbEntityInfo.ColumnKeys.Select(x => string.Format("{0}{1}{2}={3}{1}", columnLeft, x, columnRight, parameterLeft)))
            );
            Console.WriteLine(sql);
            var i = ExecuteNonQuery(sql, dbEntityInfo.Parameters.ToArray());

            return i;
        }
        #endregion

        #region 存储过程参数生成

        #region     存储过程参数[long]
        /// <summary>
        /// Make input param.存储过程参数[long]
        /// </summary>
        /// <param name="ParamName">Name of param.</param> 
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParamLong(string ParamName, long Value)
        {
            var sqlParameter = MakeInParam(
                ParamName,
                SqlDbType.BigInt,
                Value
            );
            return sqlParameter;
        }
        #endregion

        #region 存储过程参数[int]
        /// <summary>
        /// Make input param.存储过程参数[int]
        /// </summary>
        /// <param name="ParamName">Name of param.</param> 
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParamInt(string ParamName, int Value)
        {
            var sqlParameter = MakeInParam(
                ParamName,
                SqlDbType.Int,
                Value
            );
            return sqlParameter;
        }
        #endregion

        #region 存储过程参数[byte]
        /// <summary>
        /// Make input param.存储过程参数[byte]
        /// </summary>
        /// <param name="ParamName">Name of param.</param> 
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParamByte(string ParamName, byte Value)
        {
            var sqlParameter = MakeInParam(
                ParamName,
                SqlDbType.TinyInt,
                Value
            );
            return sqlParameter;
        }
        #endregion

        #region 存储过程参数[bool]
        /// <summary>
        /// Make input param.存储过程参数[bool]
        /// </summary>
        /// <param name="ParamName">Name of param.</param> 
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParamBool(string ParamName, bool Value)
        {
            var sqlParameter = MakeInParam(
                ParamName,
                SqlDbType.Bit,
                Value
            );
            return sqlParameter;
        }
        #endregion

        #region 存储过程参数[Guid]
        /// <summary>
        /// Make input param.存储过程参数[Guid]
        /// </summary>
        /// <param name="ParamName">Name of param.</param> 
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParamGuid(string ParamName, Guid Value)
        {
            var sqlParameter = MakeInParam(
                ParamName,
                SqlDbType.UniqueIdentifier,
                Value
            );
            return sqlParameter;
        }
        #endregion

        #region 存储过程参数[DateTime]
        /// <summary>
        /// Make input param.存储过程参数[DateTime]
        /// </summary>
        /// <param name="ParamName">Name of param.</param> 
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParamDateTime(string ParamName, DateTime Value)
        {
            var sqlParameter = MakeInParam(
                ParamName,
                SqlDbType.DateTime,
                Value
            );
            return sqlParameter;
        }
        #endregion

        #region 存储过程参数[NVarChar]
        /// <summary>
        /// Make input param.存储过程参数[NVarChar]
        /// </summary>
        /// <param name="ParamName">Name of param.</param> 
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParamNVarChar(string ParamName, int Size, string Value)
        {
            if (Value == null)
            {
                Value = "";
            }
            var sqlParameter = MakeInParam(
                ParamName,
                SqlDbType.NVarChar,
                Size,
                Value
            );
            return sqlParameter;
        }
        #endregion

        #region 存储过程参数[Char]
        /// <summary>
        /// Make input param.存储过程参数[Char]
        /// </summary>
        /// <param name="ParamName">Name of param.</param> 
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParamChar(string ParamName, int Size, string Value)
        {
            if (Value == null)
            {
                Value = "";
            }
            var sqlParameter = MakeInParam(
                ParamName,
                SqlDbType.Char,
                Size,
                Value
            );
            return sqlParameter;
        }
        #endregion

        #region 存储过程参数 In
        /// <summary>
        /// Make input param.存储过程参数
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParam(
            string ParamName,
            SqlDbType DbType,
            object Value
        )
        {
            ParameterDirection Direction = ParameterDirection.Input;
            int Size = 0;
            var sqlParameter = MakeParam(ParamName, DbType, Size, Direction, Value);

            return sqlParameter;
        }

        /// <summary>
        /// Make input param.存储过程
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeInParam(
            string ParamName,
            SqlDbType DbType,
            int Size,
            object Value
        )
        {
            ParameterDirection Direction = ParameterDirection.Input;
            var sqlParameter = MakeParam(ParamName, DbType, Size, Direction, Value);

            return sqlParameter;
        }
        #endregion

        #region 存储过程参数 Out
        /// <summary>
        /// Make input param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <returns>New parameter.</returns>
        public SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            ParameterDirection Direction = ParameterDirection.Output;
            object Value = null;
            var sqlParameter = MakeParam(ParamName, DbType, Size, Direction, Value);

            return sqlParameter;
        }
        #endregion

        #region 存储过程参数Base方法
        /// <summary>
        /// Make stored procedure param.
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Direction">Parm direction.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        private SqlParameter MakeParam(
            string ParamName,
            SqlDbType DbType,
            Int32 Size,
            ParameterDirection Direction,
            object Value
        )
        {
            SqlParameter param;

            if (Size > 0)
            {
                param = new SqlParameter(ParamName, DbType, Size);
            }
            else
            {
                param = new SqlParameter(ParamName, DbType);
            }

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
            {
                param.Value = Value;
            }

            return param;
        }
        #endregion

        #endregion

        #region 事务
        /// <summary>
        /// 运行事务.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool ExecTransaction(Action<SqlTransaction> action)
        {
            // 您好，tran参数就是一个事务参数，
            // 在调用SqlHelper.ExecuteNonQuery时
            // 需要将你之前创建的transaction传入进去。例如

            // SqlConnection con = new SqlConnection("...");
            // con.Open();
            // SqlTransaction tran = con.BeginTransaction();
            // int i = SqlHelper.ExecuteNonQuery(tran,commandType.Text,sql,parm);

            bool resule = false;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        action(sqlTransaction);
                        sqlTransaction.Commit();
                        resule = true;
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        throw ex;
                    } 
                }
            }

            return resule;
        }

        #endregion

        #region 一些常用方法

        /// <summary>
        /// 数据库是否存在.
        /// </summary>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public bool HasDataBase(string dbname)
        {
            var sql = " select COUNT(0) from master.dbo.sysdatabases where name = @name ";
            SqlParameter[] pars = new[]{
                MakeInParamNVarChar("@name", 50, dbname)
            };

            var b = ExecuteScalarInt(sql, pars) > 0;
            return b;
        }

        /// <summary>
        /// 大批量插入.
        /// 表名: dt.TableName
        /// </summary>
        /// <param name="dataTable"></param>
        public void BulkCopy(DataTable dataTable)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection);
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


        /// <summary>
        /// 分页的sql语句 .sql 2005 +
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="tablename">表名</param>
        /// <param name="fields">需要返回的列</param>
        /// <param name="order">排序,需要desc, asd,不能为空.</param>
        /// <param name="where">查询条件(不需要where)</param>
        /// <param name="varchar"></param>
        /// <returns></returns>
        string GetPaginationSql(
          int pageindex,
          int pagesize,
          string tablename,
          string fields,
          string order,
          string where
      )
        {
            string sql = "";
            if (!string.IsNullOrWhiteSpace(where))
            {
                where = " where " + where;
            }
            if (pageindex <= 1)
            {
                sql = string.Format(@"
select 
    top {0} 
    {1}
from
    {2}
    {3}
order by
    {4}",
                pagesize,
                fields,
                tablename,
                where,
                order
                );
            }
            else
            {
                sql = string.Format(@"
select 
    * 
from 
    (
        select 
            row_number() over (order by {0}) as [rn]
            ,{1}
        from
            {2}
            {3}
    ) as [k]
where 
    [k].[rn] between {4} and {5}
",
                order,
                fields,
                tablename,
                where,
                (pageindex - 1) * pagesize + 1,
                pageindex * pagesize
                );

            }
            return sql;
        }

        #endregion

        #region 工具类

        /// <summary>
        /// 取得实体类的sql信息.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DbEntityInfo GetEntityInfo<T>(T entity, string tableName = "")
            where T : class,new()
        {
            var type = entity.GetType();
            if (string.IsNullOrWhiteSpace(tableName) == true)
            {
                if (type.IsDefined(typeof(DbTableProperty), false) == false)
                {
                    return null;
                }
                var dbTableProperty = (DbTableProperty)type.GetCustomAttributes
                    (typeof(DbTableProperty), false)[0];
                tableName = dbTableProperty.Name;
            }
            DbEntityInfo dbEntityInfo = new DbEntityInfo()
            {
                TableName = tableName,
                Parameters = new List<SqlParameter>(),
                Columns = new List<string>(),
                ColumnKeys = new List<string>(),
                ColumnNormals = new List<string>()
            };

            var propertyinfos = type.GetProperties();
            foreach (var propertyinfo in propertyinfos)
            {
                if (propertyinfo.CanRead == false)
                {
                    continue;
                }
                if (propertyinfo.IsDefined(typeof(DbColumnProperty), false) == true)
                {
                    var property = (DbColumnProperty)propertyinfo.GetCustomAttributes
                        (typeof(DbColumnProperty), false)[0];
                    if (string.IsNullOrWhiteSpace(property.Name) == true)
                    {
                        property.Name = propertyinfo.Name;
                    }
                    SqlDbType sqlDbType;

                    if (string.IsNullOrWhiteSpace(property.Dbtype) == true)
                    {
                        sqlDbType = FormatSqlDbType(propertyinfo.PropertyType);
                    }
                    else
                    {
                        var b = Enum.TryParse<SqlDbType>(property.Dbtype, true, out sqlDbType);
                        if (b == false)
                        {
                            sqlDbType = FormatSqlDbType(propertyinfo.PropertyType);
                        }
                    }
                    Console.WriteLine(
                        property.Name + "="
                        + property.Dbtype + "="
                        + sqlDbType + "="
                        + property.Length + "="
                        + propertyinfo.PropertyType
                    );
                    var value = propertyinfo.GetValue(entity, null);

                    dbEntityInfo.Parameters.Add(
                            MakeInParam(
                            string.Format("{0}{1}", parameterLeft, property.Name),
                            sqlDbType,
                            property.Length,
                            value
                        )
                    );

                    if (property.IsIdentity == true)
                    {
                        dbEntityInfo.ColumnKeys.Add(property.Name);
                    }
                    else
                    {
                        dbEntityInfo.ColumnNormals.Add(property.Name);
                    }
                    dbEntityInfo.Columns.Add(property.Name);
                }
            }

            if (dbEntityInfo.Columns.Count == 0)
            {
                return null;
            }

            return dbEntityInfo;
        }

        /// <summary>
        /// 格式化普通类型到sql类型;
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SqlDbType FormatSqlDbType(Type type)
        {
            if (type == typeof(bool)) return SqlDbType.Bit;
            else if (type == typeof(byte)) return SqlDbType.TinyInt;
            else if (type == typeof(byte[])) return SqlDbType.Image;
            // else if (type == typeof(byte[])) return SqlDbType.VarBinary;
            // else if (type == typeof(byte[])) return SqlDbType.Timestamp;
            else if (type == typeof(DateTime)) return SqlDbType.DateTime;
            // else if (type == typeof(DateTime)) return SqlDbType.SmallDateTime;
            else if (type == typeof(decimal)) return SqlDbType.Decimal;
            // else if (type == typeof(decimal)) return SqlDbType.Money;
            // else if (type == typeof(decimal)) return SqlDbType.SmallMoney;
            else if (type == typeof(double)) return SqlDbType.Float;
            else if (type == typeof(float)) return SqlDbType.Real;
            else if (type == typeof(Guid)) return SqlDbType.UniqueIdentifier;
            else if (type == typeof(int)) return SqlDbType.Int;
            else if (type == typeof(long)) return SqlDbType.BigInt;
            else if (type == typeof(object)) return SqlDbType.Binary;
            else if (type == typeof(short)) return SqlDbType.SmallInt;
            else if (type == typeof(string)) return SqlDbType.NVarChar;
            // else if (type == typeof(string)) return SqlDbType.Char;
            // else if (type == typeof(string)) return SqlDbType.NChar;
            // else if (type == typeof(string)) return SqlDbType.NText;
            // else if (type == typeof(string)) return SqlDbType.Text;
            // else if (type == typeof(string)) return SqlDbType.VarChar;
            // else if (type == typeof(string)) return SqlDbType.Xml;

            else if (type == typeof(bool?)) return SqlDbType.Bit;
            else if (type == typeof(byte?)) return SqlDbType.TinyInt;
            else if (type == typeof(byte[])) return SqlDbType.Image;
            else if (type == typeof(DateTime?)) return SqlDbType.DateTime;
            else if (type == typeof(decimal?)) return SqlDbType.Decimal;
            else if (type == typeof(double?)) return SqlDbType.Float;
            else if (type == typeof(float?)) return SqlDbType.Real;
            else if (type == typeof(Guid?)) return SqlDbType.UniqueIdentifier;
            else if (type == typeof(int?)) return SqlDbType.Int;
            else if (type == typeof(long?)) return SqlDbType.BigInt;
            else if (type == typeof(object)) return SqlDbType.Binary;
            else if (type == typeof(short?)) return SqlDbType.SmallInt;

            else return SqlDbType.Int;
        }

        #endregion
    }
}
#region Get方法测试结果
// Get方法结果
// select * from users where id < 1000, datatable == null : False
// select * from users where id < 1000, listtable count: 867
// select * from users where id < 1000, singletable == null : False
// select * from users where id < 1000, listreader count: 867
// select * from users where id < 1000, singlereader == null : False

// select * from users where id < 0, datatable == null : False
// select * from users where id < 0, listtable count: 0
// select * from users where id < 0, singletable == null : True
// select * from users where id < 0, listreader count: 0
// select * from users where id < 0, singlereader == null : True

// update users set username = '' where id < 0, datatable == null : True
// update users set username = '' where id < 0, listtable count: null
// update users set username = '' where id < 0, singletable == null : True
// update users set username = '' where id < 0, listreader count: null
// update users set username = '' where id < 0, singlereader == null : True

#endregion

#region SqlServer 与 c#类型对应
//<Type From="bigint" To="long" />
//<Type From="binary" To="object" />
//<Type From="bit" To="bool" />
//<Type From="char" To="string" />
//<Type From="datetime" To="DateTime" />
//<Type From="decimal" To="decimal" />
//<Type From="float" To="double" />
//<Type From="image" To="byte[]" />
//<Type From="int" To="int" />
//<Type From="money" To="decimal" />
//<Type From="nchar" To="string" />
//<Type From="ntext" To="string" />
//<Type From="numeric" To="decimal" />
//<Type From="nvarchar" To="string" />
//<Type From="real" To="float" />
//<Type From="smalldatetime" To="DateTime" />
//<Type From="smallint" To="short" />
//<Type From="smallmoney" To="decimal" />
//<Type From="text" To="string" />
//<Type From="timestamp" To="byte[]" />
//<Type From="tinyint" To="byte" />
//<Type From="uniqueidentifier" To="Guid" />
//<Type From="varbinary" To="byte[]" />
//<Type From="varchar" To="string" />
//<Type From="xml" To="string" />
//<Type From="sql_variant" To="object" />

#endregion
