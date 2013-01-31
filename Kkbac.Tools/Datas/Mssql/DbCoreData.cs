using System;
using System.Collections.Generic;
using System.Linq;

namespace Kkbac.Tools.Datas.Mssql
{
    public class DbCoreData
    {
        public DbCommand Command { get; set; }

        public DbCoreData(DbCommand dbcommand)
        {
            Command = dbcommand;
        }
    }
}