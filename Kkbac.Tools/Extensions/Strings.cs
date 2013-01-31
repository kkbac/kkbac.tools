using System;
using System.Text.RegularExpressions;

namespace Kkbac.Tools.Extensions.Strings
{
    public static class Strings
    {
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        ///  string today = "今天是：{0:yyyy年MM月dd日 星期ddd}".FormatWith(DateTime.Today);
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        { 
            return string.Format(format, args);
        }

        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern);
        }

        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }

        public static string ToCamel(this string s)
        {

            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToLower() + s.Substring(1);

        }

        public static string ToPascal(this string s)
        {
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToUpper() + s.Substring(1);
        }
    }
}
