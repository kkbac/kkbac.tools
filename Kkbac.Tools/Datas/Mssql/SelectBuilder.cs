using System;
using System.Collections.Generic;
using System.Linq;

namespace Kkbac.Tools.Datas.Mssql
{
    public class SelectBuilder : BaseBuilder
    {
        public SelectBuilder(DbCommand dbCommand, string tableName)
            : base(dbCommand, tableName)
        {

        }

        public override DbCommand GetPreparedCommand()
        {
            return Data.Command;
        }


    }
}