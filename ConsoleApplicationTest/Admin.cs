using System;
using System.Data;

namespace ConsoleApplicationTest
{
    [Kkbac.Tools.Datas.SqlHelperTool.DbTableProperty("[chinalovematch.net].dbo.[admin]")]
    public class Admin
    {
        [Kkbac.Tools.Datas.SqlHelperTool.DbColumnProperty(IsIdentity = true)]
        public int id { get; set; }
        [Kkbac.Tools.Datas.SqlHelperTool.DbColumnProperty(Length = 50)]
        public string userid { get; set; }
        [Kkbac.Tools.Datas.SqlHelperTool.DbColumnProperty(Length = 50)]
        public string password { get; set; }
        //[Kkbac.Tools.Datas.DbColumnProperty(Dbtype = "int")]
        public long kkbac { get; set; }
        [Kkbac.Tools.Datas.SqlHelperTool.DbColumnProperty(IsIdentity = true)]
        public int tc { get; set; }
    }
}
