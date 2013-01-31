using System;
using System.Collections.Generic;
using System.Linq;

namespace Kkbac.Tools.Datas.Mssql
{
    public class BuilderData
    {
        public DbCommand Command { get; set; }
        public string TableName { get; set; }
        public List<BuilderColumn> Columns { get; set; }
        public List<BuilderColumn> Where { get; set; }
        public List<string> WhereString { get; set; }

        public BuilderData(DbCommand command, string tableName)
        {
            TableName = tableName;
            Command = command;
            Columns = new List<BuilderColumn>();
            Where = new List<BuilderColumn>();
            WhereString = new List<string>();
        }
    }
}