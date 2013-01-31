using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kkbac.Tools.Web.MVC.Exceptions
{
    public class ExceptionModelKeyValue
    {
        public string k { get; set; }
        public string v { get; set; }
    }
    public class ExceptionModel
    {
        public int Pkid { get; set; }
        /// <summary>
        /// nvarchar(2000)
        /// </summary>
        public string Url { get; set; }
        public int StatusCode { get; set; }
        /// <summary>
        /// nvarchar(100)
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// nvarchar(100)
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// nvarchar(500)
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// nvarchar(max)
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// nvarchar(max)
        /// </summary>
        public string ServerVariables { get; set; }
        /// <summary>
        /// ServerVariables
        /// </summary>
        public List<ExceptionModelKeyValue> ServerVariablesList { get; set; }
        /// <summary>
        /// nvarchar(max)
        /// </summary>
        public string QueryString { get; set; }
        /// <summary>
        /// QueryString
        /// </summary>
        public List<ExceptionModelKeyValue> QueryStringList { get; set; }
        /// <summary>
        /// nvarchar(max)
        /// </summary>
        public string Form { get; set; }
        /// <summary>
        /// Form
        /// </summary>
        public List<ExceptionModelKeyValue> FormList { get; set; }
        /// <summary>
        /// nvarchar(max)
        /// </summary>
        public string Cookies { get; set; }
        /// <summary>
        /// Cookies
        /// </summary>
        public List<ExceptionModelKeyValue> CookiesList { get; set; }
        /// <summary>
        /// datetime
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
