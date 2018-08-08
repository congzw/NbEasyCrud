using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZQNB.Common
{
    public static class SimpleMapToExtensions
    {

        /// <summary>
        /// COPY所有简单类型的属性的值
        /// </summary>
        /// <typeparam name="TCopyTo"></typeparam>
        /// <param name="copyFrom"></param>
        /// <param name="copyTo"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        public static void TryCopyTo<TCopyTo>(this object copyFrom, TCopyTo copyTo, params string[] excludeProperties)
        {
            if (copyFrom == null)
            {
                return;
            }
            if (copyTo == null)
            {
                throw new ArgumentNullException("copyTo");
            }
            //better relace impl from reflection with expression tree, not find good impl yet. todo
            MyModelHelper.TryCopyProperties(copyTo, copyFrom, excludeProperties);
        }
    }

    public class MyModelHelper
    {
        public static void TryCopyProperties(Object updatingObj, Object collectedObj, string[] excludeProperties = null)
        {
            if (collectedObj != null && updatingObj != null)
            {
                //获取类型信息
                Type updatingObjType = updatingObj.GetType();
                PropertyInfo[] updatingObjPropertyInfos = updatingObjType.GetProperties();

                Type collectedObjType = collectedObj.GetType();
                PropertyInfo[] collectedObjPropertyInfos = collectedObjType.GetProperties();

                string[] fixedExPropertites = excludeProperties ?? new string[] { };

                foreach (PropertyInfo updatingObjPropertyInfo in updatingObjPropertyInfos)
                {
                    foreach (PropertyInfo collectedObjPropertyInfo in collectedObjPropertyInfos)
                    {
                        if (updatingObjPropertyInfo.Name == collectedObjPropertyInfo.Name)
                        {
                            if (fixedExPropertites.Contains(updatingObjPropertyInfo.Name, StringComparer.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            //do not process complex property
                            var isSimpleType = IsSimpleType(collectedObjPropertyInfo.PropertyType);
                            if (!isSimpleType)
                            {
                                continue;
                            }

                            //fix dynamic problems: System.Reflection.TargetParameterCountException
                            var declaringType = collectedObjPropertyInfo.DeclaringType;
                            if (declaringType != null && declaringType != collectedObjType)
                            {
                                //do not process base class dynamic property
                                if (NotProcessPerpertyBaseTypes.Contains(declaringType))
                                {
                                    continue;
                                }
                            }

                            object value = collectedObjPropertyInfo.GetValue(collectedObj, null);
                            if (updatingObjPropertyInfo.CanWrite)
                            {
                                //do not process read only property
                                updatingObjPropertyInfo.SetValue(updatingObj, value, null);
                            }
                            break;
                        }
                    }
                }
            }
        }
        
        public static Dictionary<string, object> GetKeyValueDictionary<T>(T obj)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (obj != null)
            {
                //获取类型信息
                Type t = typeof(T);
                PropertyInfo[] propertyInfos = t.GetProperties();

                foreach (PropertyInfo var in propertyInfos)
                {
                    result.Add(var.Name, var.GetValue(obj, null));
                }
            }
            return result;
        }

        /// <summary>
        /// 是否是简单类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimpleType(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimpleType(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive
                || typeInfo.IsEnum
                || type == typeof(string)
                || type == typeof(decimal)
                //|| type == typeof(Guid)
                //|| type == typeof(DateTime)
                || type.IsSubclassOf(typeof(ValueType)); //Guid, Datetime, etc...
        }


        private static IList<Type> _notProcessPerpertyBaseTypes = new List<Type>()
        {
            //typeof(DynamicObject), typeof(Object), typeof(BaseViewModel),  typeof(BaseViewModel<>), typeof(Expando) 
        };

        /// <summary>
        /// 在这些类型中声明的属性不处理
        /// </summary>
        public static IList<Type> NotProcessPerpertyBaseTypes
        {
            get
            {
                return _notProcessPerpertyBaseTypes;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _notProcessPerpertyBaseTypes = value;
            }
        }
        
        /// <summary>
        /// 获取集合的单项类型
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static Type GetTypeFromCollection(IEnumerable collection)
        {
            Type type = collection.GetType();
            if (type.IsGenericType)
            {
                return type.GetInterfaces()
                  .Where(t => t.IsGenericType)
                  .Single(t => t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                  .GetGenericArguments().Last();
            }
            if (collection.GetType().IsArray)
            {
                return type.GetElementType();
            }
            // Who knows?
            return null;
        }
    }
}
