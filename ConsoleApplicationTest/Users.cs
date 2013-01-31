using System;
using System.Data;

namespace ConsoleApplicationTest
{
    [Kkbac.Tools.Datas.SqlHelperTool.DbTableProperty("chinalovematch.net.dbo.users")]
    public class Users
    {
        [Kkbac.Tools.Datas.SqlHelperTool.DbColumnProperty(IsIdentity = true)]
        public int id { get; set; }
        [Kkbac.Tools.Datas.SqlHelperTool.DbColumnProperty(Length = 50)]
        public string username { get; set; }
        public string password { get; set; }

    }
}
