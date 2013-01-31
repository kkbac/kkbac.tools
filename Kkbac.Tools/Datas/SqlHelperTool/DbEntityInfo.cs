using System.Collections.Generic;
using System.Data.SqlClient;

namespace Kkbac.Tools.Datas.SqlHelperTool
{
    public class DbEntityInfo
    {
        public string TableName { get; set; }
        public List<SqlParameter> Parameters { get; set; }
        public List<string> Columns { get; set; }
        public List<string> ColumnKeys { get; set; }
        public List<string> ColumnNormals { get; set; }
    }
}
