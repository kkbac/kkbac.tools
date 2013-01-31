using System;
using System.Data;

namespace Kkbac.Tools.Datas.SqlHelperTool
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DbColumnProperty : Attribute
    {
        #region 属性
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }
        public string Dbtype { get; set; }
        public int Length { get; set; }
        #endregion

        public DbColumnProperty()
        {

        }
    }
}
