using System;
using System.Collections.Generic;
using System.Reflection;

namespace TGame.Common
{
    /// <summary>
    /// 作者: Teddy
    /// 时间: 2018/03/02
    /// 功能: 
    /// </summary>
	public static partial class Utility
    {
        public static class Types
        {
            public readonly static Assembly GAME_CSHARP_ASSEMBLY = Assembly.Load("Assembly-CSharp");
            public readonly static Assembly GAME_EDITOR_ASSEMBLY = Assembly.Load("Assembly-CSharp-Editor");

            /// <summary>
            /// 获取所有能从某个类型分配的属性列表
            /// </summary>
            public static List<PropertyInfo> GetAllAssignablePropertiesFromType(Type basePropertyType, Type objType, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            {
                List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
                PropertyInfo[] properties = objType.GetProperties(bindingFlags);
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo propertyInfo = properties[i];
                    if (basePropertyType.IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        propertyInfos.Add(propertyInfo);
                    }
                }
                return propertyInfos;
            }

            /// <summary>
            /// 获取某个类型的所有子类型
            /// </summary>
            /// <param name="baseClass">父类</param>
            /// <param name="assemblies">程序集,如果为null则查找当前程序集</param>
            /// <returns></returns>
            public static List<Type> GetAllSubclasses(Type baseClass, bool allowAbstractClass, params Assembly[] assemblies)
            {
                List<Type> subclasses = new List<Type>();
                if (assemblies == null)
                {
                    assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
                }
                foreach (var assembly in assemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!baseClass.IsAssignableFrom(type))
                            continue;

                        if (!allowAbstractClass && type.IsAbstract)
                            continue;

                        subclasses.Add(type);
                    }
                }
                return subclasses;
            }
        }
    }
}