using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZQNB.Web.Models
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
    }
}
