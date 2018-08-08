using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ZQNB.Common.Cruds
{
    public class CrudViewModelMeta
    {
        public ClassInfo ClassInfo { get; set; }
        public string Controller { get; set; }

        public static CrudViewModelMeta Create(Type viewModelType, Type controllerType)
        {
            var crudViewModelMeta = new CrudViewModelMeta();
            crudViewModelMeta.ClassInfo = ClassInfo.Create(viewModelType);
            crudViewModelMeta.Controller = controllerType.Name.Replace("Controller", "");
            return crudViewModelMeta;
        }
    }

    public class ClassInfo
    {
        /// <summary>
        /// 类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Caption { get; set; }

        public IList<ClassColumnInfo> ColumnInfos { get; set; }

        public static ClassInfo Create(Type viewModelType)
        {
            var classInfo = new ClassInfo();
            classInfo.Name = viewModelType.Name;
            classInfo.Caption = TryGetDisplayName(viewModelType);
            classInfo.ColumnInfos = ClassColumnInfo.Create(viewModelType);
            return classInfo;
        }

        private static string TryGetDisplayName(Type viewModelType)
        {
            var displayAttribute = viewModelType.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                return displayAttribute.Name;
            }

            var descriptionAttribute = viewModelType.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            var displayNameAttribute = viewModelType.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }


            return viewModelType.Name;
        }
    }

    /// <summary>
    /// 列定义
    /// </summary>
    public class ClassColumnInfo
    {
        /// <summary>
        /// 列定义
        /// </summary>
        public ClassColumnInfo()
        {
            Name = string.Empty;
            Caption = string.Empty;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 创建数据导入的列定义
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<ClassColumnInfo> Create(Type type)
        {
            var columnInfos = new List<ClassColumnInfo>();
            var properties = type.GetProperties();
            var index = 0;
            foreach (var property in properties)
            {
                var columnInfo = new ClassColumnInfo();
                columnInfo.Index = index;
                index++;
                columnInfo.Name = property.Name;
                columnInfo.Caption = TryGetDisplayName(property);
                columnInfos.Add(columnInfo);
            }
            return columnInfos;
        }

        private static string TryGetDisplayName(PropertyInfo property)
        {
            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                return displayAttribute.Name;
            }

            var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            var displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }


            return property.Name;
        }
    }
}
