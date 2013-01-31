using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kkbac.Tools.Extensions.DataReader
{
    /// <summary>
    /// IDataReader 扩展方法集合
    /// </summary>
    [DebuggerStepThrough]
    public static class DataReader
    {
        public static List<T> ToList<T>(
            this IDataReader iDataReader,
            int count = 0
        ) where T : new()
        {
            if (iDataReader == null)
            {
                throw new ArgumentNullException("iDataReader");
            }

            if (iDataReader.IsClosed)
            {
                throw new ArgumentException("IDataReader is closed.");
            }

            if (iDataReader.FieldCount == 0)
            {
                return null;
            }

            Type targetType = typeof(T);
            var target = Expression.Parameter(typeof(object), "target");
            Dictionary<string, Tuple<Type, Action<object, object>>> setActions =
                new Dictionary<string, Tuple<Type, Action<object, object>>>();

            var fieldsCount = iDataReader.FieldCount;
            for (var i = 0; i < fieldsCount; i++)
            {
                var propertyName = iDataReader.GetName(i);
                // 如果不用 DeclaredOnly, 
                // 本类和基类中都有相同的属性名称的话, 会报不明确的引用错误.
                // 如果只是简单的从所有属性中查找出第一个属性名相同的属性,
                // 可能找到的不是本类的属性,而是基类的属性.

                // 先不从父类中查找
                PropertyInfo property = targetType.GetProperty(
                    propertyName,
                    BindingFlags.Public
                    | BindingFlags.Instance
                    | BindingFlags.IgnoreCase
                    | BindingFlags.DeclaredOnly
                );
                if (property == null)
                {
                    // 如果自身并没有声明,从基类中查找第一个
                    property = targetType.GetProperties().FirstOrDefault(p =>
                        string.Equals(p.Name, propertyName,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );
                    // property = targetType.GetProperty(
                    //      propertyName , 
                    //      BindingFlags.Public | 
                    //      BindingFlags.Instance | 
                    //      BindingFlags.IgnoreCase
                    //      );
                }
                // 如果还没有,跳过
                if (property == null)
                    continue;

                var setMethod = property.GetSetMethod();
                if (setMethod == null)
                    continue;

                NullableConverter nullableConvert;
                var propertyType = property.PropertyType;
                var toType = propertyType;
                if (propertyType.IsGenericType
                    &&
                    propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))
                )
                {
                    nullableConvert = new NullableConverter(propertyType);
                    toType = nullableConvert.UnderlyingType;
                }

                var propertyValue = Expression.Parameter(typeof(object), "value");
                var castedTarget = setMethod.IsStatic
                    ? null : Expression.Convert(target, targetType);

                var castedPropertyValue = Expression.Convert(
                    propertyValue,
                    propertyType
                );
                Expression propertySet = Expression.Call(
                    castedTarget,
                    setMethod,
                    castedPropertyValue
                );
                var action = Expression.Lambda<Action<object, object>>(
                    propertySet,
                    true,
                    target,
                    propertyValue
                ).Compile();


                propertyName = propertyName.ToUpper();
                if (setActions.ContainsKey(propertyName))
                {
                    setActions[propertyName] =
                        new Tuple<Type, Action<object, object>>(toType, action);
                }
                else
                {
                    setActions.Add(propertyName,
                        new Tuple<Type, Action<object, object>>(toType, action));
                }
            }

            List<T> entitys = new List<T>();

            int idx = 0;
            while (iDataReader.Read())
            {
                if (count > 0)
                    idx++;

                T newObject = new T();
                for (int i = 0; i < iDataReader.FieldCount; i++)
                {
                    var fieldName = iDataReader.GetName(i).ToUpper();
                    if (setActions.ContainsKey(fieldName))
                    {
                        var value = iDataReader.GetValue(i);
                        // var propertyType = setActions[fieldName].Item1;
                        var toType = setActions[fieldName].Item1;
                        var action = setActions[fieldName].Item2;
                        // action.Invoke(newObject, value);
                        if (value != System.DBNull.Value)
                        {
                            if (toType.IsEnum)
                            {
                                object enumValue = null;
                                if (value is string)
                                {
                                    enumValue = Enum.Parse(toType, value as string);
                                }
                                else
                                {
                                    enumValue = Enum.ToObject(toType, value);
                                }

                                action(newObject, enumValue);
                                //  action(newObject , value);
                            }
                            else
                            {
                                // 数据库传回的值，可能不和实体的类型一致
                                // 如果属性类型是 DateTime ，
                                // 数据库传回来的是日期字符串，
                                // 用 Convert.ChangeType 也是可以的，所以，上面条件不成立。
                                try
                                {
                                    action(newObject, Convert.ChangeType(value, toType));
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                entitys.Add(newObject);

                if (idx > count)
                    break;
            }
            return entitys;
        }
    }
}
