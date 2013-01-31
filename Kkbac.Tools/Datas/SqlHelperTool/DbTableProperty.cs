using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kkbac.Tools.Datas.SqlHelperTool
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DbTableProperty : Attribute
    {
        public string Name { get; set; }

        public DbTableProperty(string name)
        {
            Name = name;
        }
    }
}
