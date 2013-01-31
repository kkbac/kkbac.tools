using System;
using System.Data;
using System.Data.SqlClient;

namespace Kkbac.Tools.Datas
{
    public class DatabaseAction
    {
        SqlConnection _connection;
        string _connectionString = "Data Source=127.0.0.1;Initial Catalog=master;User ID=sa;Password=sa;Connect Timeout=500";

        public DatabaseAction(string connectionstring = "")
        {
            if (string.IsNullOrWhiteSpace(connectionstring) == true)
            {
                connectionstring = _connectionString;
            }
            _connection = new SqlConnection(connectionstring);
        }


        /// <summary> 
        /// 数据库备份 
        /// </summary> 
        public void DbBackup(string path, string dbName)
        {
            var command = new SqlCommand("use master;backup database @name to disk=@path;",
                _connection
            );
            command.CommandTimeout = _connection.ConnectionTimeout;
            _connection.Open();
            command.Parameters.AddWithValue("@name", dbName);
            command.Parameters.AddWithValue("@path", path);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        /// <summary> 
        /// 数据库恢复 
        /// </summary> 
        public void DbRestore(string path, string dbName)
        {
            KillSpid(dbName);

            var command = new SqlCommand("use master;restore database @name from disk=@path with replace ",
                _connection
            );
            command.CommandTimeout = _connection.ConnectionTimeout;
            _connection.Open();
            command.Parameters.AddWithValue("@name", dbName);
            command.Parameters.AddWithValue("@path", path);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        private bool KillSpid(string dbName)
        {
            var cmd = new SqlCommand("killspid", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dbname", dbName);
            try
            {
                _connection.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }

    }

}


//USE [master]
//GO

///****** Object:  StoredProcedure [dbo].[killspid]    Script Date: 07/05/2012 08:58:15 ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//create proc [dbo].[killspid] (@dbname varchar(20))
//as
//begin
//declare @sql nvarchar(500)
//declare @spid int
//set @sql='declare getspid cursor for 
//select spid from sysprocesses where dbid=db_id('''+@dbname+''')'
//exec (@sql)
//open getspid
//fetch next from getspid into @spid
//while @@fetch_status<>-1
//begin
//exec('kill '+@spid)
//fetch next from getspid into @spid
//end
//close getspid
//deallocate getspid
//end

//GO
