using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Kkbac.Tools.Datas.Mssql
{
    public class DbContext
    {
        public DbContextData Data { get; private set; }

        /// <summary>
        /// new DbCommand();
        /// </summary>
        private DbCommand CreateCommand
        {
            get
            {
                return new DbCommand(this);
            }
        }


        public DbContext(string connection = null)
        {
            Data = new DbContextData();
            if (string.IsNullOrWhiteSpace(connection) == true)
            {
                Data.ConnectionString = ConfigurationManager
                    .ConnectionStrings["connection"].ConnectionString;
            }
            else
            {
                Data.ConnectionString = connection;
            }
        }

        public DbCommand StoredProcedure(string sql, params SqlParameter[] parameters)
        {
            var command = CreateCommand.StoredProcedure(sql).Parameters(parameters);

            return command;
        }

        public DbCommand Sql(string sql, params SqlParameter[] parameters)
        {
            var command = CreateCommand.Sql(sql).Parameters(parameters);
            return command;
        }

        public InsertBuilder Insert(string tableName)
        {
            return new InsertBuilder(CreateCommand, tableName);
        }

        public DeleteBuilder Delete(string tableName)
        {
            return new DeleteBuilder(CreateCommand, tableName);
        }

        public UpdateBuilder Update(string tableName)
        {
            return new UpdateBuilder(CreateCommand, tableName);
        }

        public DbCore DbCore
        {
            get
            {
                return new DbCore(CreateCommand);
            }
        }
    }
}