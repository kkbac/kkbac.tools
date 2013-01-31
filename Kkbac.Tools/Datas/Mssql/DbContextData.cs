using System;
using System.Collections.Generic;
using System.Linq;

namespace Kkbac.Tools.Datas.Mssql
{
    public class DbContextData
    {
        public string ConnectionString { get; set; }
        //public int CommandTimeout { get; set; }
        public bool UseTransaction { get; set; }

        public DbContextData()
        {
            UseTransaction = false;
            //CommandTimeout = 30;
        }
    }
}