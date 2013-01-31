using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kkbac.Tools.Extensions.Formats
{
    public static class Formats
    {
        #region TryChangeType 更改对象类型

        /// <summary>
        /// Enum.Parse or Convert.ChangeType
        /// </summary>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T TryChangeType<T>(this object value)
        {
            if (value == null)
            {
                return default(T);
            }
            var type = typeof(T);
            var o = value.TryChangeType(type);
            if (o is T)
            {
                return (T)o;
            }
            return default(T);
        }

        /// <summary>
        /// Enum.Parse or Convert.ChangeType
        /// </summary>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <returns>null or error: Activator.CreateInstance(type);</returns>
        public static object TryChangeType(
            this object value,
            Type type
        )
        {
            if (value == null && type.IsGenericType)
            {
                return Activator.CreateInstance(type);
            }
            if (value == null)
            {
                //return null;
                return Activator.CreateInstance(type);
            }
            if (type == value.GetType())
            {
                return value;
            }
            try
            {
                if (type.IsEnum)
                {
                    if (value is string)
                    {
                        return Enum.Parse(type, value as string);
                    }
                    else
                    {
                        return Enum.ToObject(type, value);
                    }
                }
                //if (!type.IsInterface && type.IsGenericType)
                //{
                //    Type innerType = type.GetGenericArguments()[0];
                //    object innerValue = QueryHelper.ChangeType(value, innerType);
                //    return Activator.CreateInstance(type, new object[] { innerValue });
                //}
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    System.ComponentModel.NullableConverter nullableConverter
                        = new System.ComponentModel.NullableConverter(type);

                    type = nullableConverter.UnderlyingType;
                    return Convert.ChangeType(value, type);
                }

                if (value is string && type == typeof(Guid))
                {
                    return new Guid(value as string);
                }
                if (value is string && type == typeof(Version))
                {
                    return new Version(value as string);
                }
                if (!(value is IConvertible))
                {
                    //return value;
                    return Activator.CreateInstance(type);
                }
                return Convert.ChangeType(value, type);
            }
            catch
            {
            }
            return Activator.CreateInstance(type);
        }

        #endregion

        #region 类型转换

        public static long ToLong(this object o,
           long defaultValue = 0)
        {
            long r = defaultValue;
            var b = long.TryParse(o.ToStrings(), out r);
            return b ? r : defaultValue;
        }

        public static int ToInt(this object o,
            int defaultValue = 0)
        {
            int r = defaultValue;
            var b = int.TryParse(o.ToStrings(), out r);
            return b ? r : defaultValue;
        }

        public static DateTime ToDateTime(this object o)
        {
            if (o == null)
            {
                return DateTime.MinValue;
            }
            DateTime d = DateTime.MinValue;
            DateTime.TryParse(o.ToString(), out d);
            return d;
        }

        /// <summary>
        /// 如果 value 为非零值，则为 true；否则为 false。 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool IntToBool(this int i)
        {
            return i != 0;
        }

        /// <summary>
        /// return Convert.ToString(o)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ToStrings(this object o)
        {
            return Convert.ToString(o);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="hashMethod">Clear or MD5 or SHA1</param>
        /// <returns>空返回空</returns>
        public static string ToPasswordFormat(this string s,
             System.Web.Configuration.FormsAuthPasswordFormat formsAuthPasswordFormat
                = System.Web.Configuration.FormsAuthPasswordFormat.MD5)
        {
            var hash = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(
                        s,
                        formsAuthPasswordFormat.ToString());

            return hash;
        }


        #region data to list

        /// <summary>
        /// dt=null: 返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this System.Data.DataTable dt)
            where T : class,new()
        {
            if (dt == null)
            {
                return null;
            }

            List<T> list = new List<T>();

            T model = new T();
            //model = default(T);
            //model = Activator.CreateInstance<T>();
            Type type = model.GetType();
            System.Reflection.PropertyInfo[] pi = type.GetProperties();

            var columnName = "";

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                //model = Activator.CreateInstance<T>();
                model = new T();
                foreach (var p in pi)
                {
                    if (!p.CanWrite)
                    {
                        continue;
                    }
                    columnName = p.Name;
                    if (!dr.Table.Columns.Contains(columnName))
                    {
                        continue;
                    }
                    if (dr[columnName] == DBNull.Value)
                    {
                        continue;
                    }
                    p.SetValue(model, dr[p.Name], null);
                }
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// dr=null: 返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ToModel<T>(this System.Data.DataRow dr)
            where T : class,new()
        {
            if (dr == null)
            {
                return null;
            }
            T model = new T();
            Type type = model.GetType();
            System.Reflection.PropertyInfo[] pi = type.GetProperties();

            var columnName = "";

            foreach (var p in pi)
            {
                if (!p.CanWrite)
                {
                    continue;
                }
                columnName = p.Name;
                if (!dr.Table.Columns.Contains(columnName))
                {
                    continue;
                }
                if (dr[columnName] == DBNull.Value)
                {
                    continue;
                }

                p.SetValue(model, dr[p.Name], null);
            }

            return model;
        }

        #endregion

        #region object to json.
        public static string Obj2Json<T>(this T data)
        {
            try
            {
                var js = new System.Web.Script.Serialization.JavaScriptSerializer();
                var r = js.Serialize(data);
                return r;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 含try
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Json2Obj<T>(this String json)
            where T : class, new()
        {
            T t = new T();
            try
            {
                var js = new System.Web.Script.Serialization.JavaScriptSerializer();
                t = js.Deserialize<T>(json);
            }
            catch
            {
                t = null;
            }
            return t;
        }

        #endregion

        public static string ToMd5(this string s)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(
                s,
                "MD5" //Clear or MD5 or SHA1
            );
        }

        /// <summary>  
        /// 格式化文件大小 
        /// </summary>
        /// <param name="size">大小</param>
        /// <returns>式化大小</returns>
        public static string FormatSize(long size, string formatString = "0.##")
        {
            try
            {
                string strReturn = "";
                double tempSize = Math.Abs(size);
                if (tempSize < 1024)
                {
                    strReturn += tempSize.ToString() + "B";
                }
                else if (tempSize < 1024 * 1024)
                {
                    tempSize = tempSize / 1024;
                    strReturn += tempSize.ToString(formatString) + "K";
                }
                else if (tempSize < 1024 * 1024 * 1024)
                {
                    tempSize = tempSize / 1024 / 1024;
                    strReturn += tempSize.ToString(formatString) + "M";
                }
                else
                {
                    tempSize = tempSize / 1024 / 1024 / 1024;
                    strReturn += tempSize.ToString(formatString) + "G";
                }
                if (size < 0)
                {
                    strReturn = "-" + strReturn;
                }
                return strReturn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

}
