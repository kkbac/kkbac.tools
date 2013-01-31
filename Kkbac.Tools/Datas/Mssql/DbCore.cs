using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Kkbac.Tools.Datas.Mssql
{
    public class DbCore
    {
        public DbCoreData Data { get; set; }

        public DbCore(DbCommand dbcommand)
        {
            Data = new DbCoreData(dbcommand);
        }

        /// <summary>
        /// 数据库是否存在.
        /// </summary>
        /// <param name="dbname"></param>
        /// <returns></returns>
        public bool HasDataBase(string dbname)
        {
            var sql = " select top 1 1 from master.dbo.sysdatabases where name = @name ";
            var b = Data.Command.Clear()
                 .Sql(sql).ParameterNVarChar("@name", dbname, 100)
                 .ExecuteScalar<int>() > 0;

            return b;
        }

        /// <summary>
        /// 插入.不写Log ( 大批量效率高 )
        /// 表名: dt.TableName
        /// </summary>
        /// <param name="dataTable"></param>
        public void BulkCopy(DataTable dataTable)
        {
            using (var sqlConnection = new SqlConnection(Data.Command.Data.Context.Data.ConnectionString))
            {
                using (var bulkCopy = new SqlBulkCopy(sqlConnection))
                {
                    bulkCopy.DestinationTableName = dataTable.TableName;
                    bulkCopy.BatchSize = dataTable.Rows.Count;
                    sqlConnection.Open();
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }
            //;


            //try
            //{

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    sqlConnection.Close();
            //    if (bulkCopy != null)
            //    {
            //        bulkCopy.Close();
            //    }
            //}
        }
    }
}