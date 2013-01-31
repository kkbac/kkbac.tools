using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Kkbac.Tools.Extensions.Attributes
{
    /// <summary>
    /// 通过反射访问属性（Attribute）信息的工具类
    /// </summary>
    public static class Attributes
    {
        /// <summary>
        /// 获取某个类型包括指定属性的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<T> GetCustomAttributes<T>(this Type type)
            where T : Attribute
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            var attributes = (T[])(type.GetCustomAttributes(typeof(T), false));
            if (attributes.Length == 0)
            {
                return null;
            }

            var result = new List<T>(attributes);
            return result;
        }

        /// <summary>
        /// 获得某各类型包括指定属性(type.GetMethods())的所有方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<MethodInfo> GetMethodsWithCustomAttribute<T>(this Type type)
            where T : Attribute
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            var methods = type.GetMethods();
            if ((methods == null) || (methods.Length == 0))
            {
                return null;
            }
            IList<MethodInfo> result = new List<MethodInfo>();
            foreach (MethodInfo method in methods)
            {
                if (method.IsDefined(typeof(T), false))
                {
                    result.Add(method);
                }
            }
            if (result.Count == 0)
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// 获取某个方法指定类型属性的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static IList<T> GetMethodCustomAttributes<T>(this MethodInfo method)
            where T : Attribute
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            var attributes = (T[])(method.GetCustomAttributes(typeof(T), false));
            if (attributes.Length == 0)
            {
                return null;
            }

            var result = new List<T>(attributes);
            return result;
        }

        /// <summary>
        /// 获取某个方法指定类型的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static T GetMethodCustomAttribute<T>(this MethodInfo method)
            where T : Attribute
        {
            var attributes = GetMethodCustomAttributes<T>(method);

            if (attributes == null)
            {
                return null;
            }

            return attributes[0];
        }
    }
}
