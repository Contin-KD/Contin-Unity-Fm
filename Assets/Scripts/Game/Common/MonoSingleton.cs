using System;
using UnityEngine;

namespace TGame.Common
{
    /// <summary>
    /// Mono单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T s_instance;

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    Type t = typeof(T);

                    string prefabPath = null;
                    object[] attrs = t.GetCustomAttributes(typeof(MonoSingletonAttribute), false);
                    if (attrs.Length > 0)
                    {
                        MonoSingletonAttribute attribute = attrs[0] as MonoSingletonAttribute;
                        prefabPath = attribute.PrefabPath;
                    }

                    if (prefabPath == null)
                    {
                        GameObject go = new GameObject()
                        {
                            name = t.Name
                        };
                        s_instance = go.AddComponent<T>();
                    }
                    else
                    {
                        T prefab = Resources.Load<T>(prefabPath);
                        T go = Instantiate(prefab);
                        go.name = t.Name;
                        s_instance = go;
                    }

                    if (s_instance != null)
                    {
                        s_instance.OnCreateInstance();
                    }
                    else
                    {
                        Debug.Log("instance is null");
                    }
                }
                return s_instance;
            }
        }

        /// <summary>
        /// 单例是否存在
        /// </summary>
        /// <returns></returns>
        public static bool Exsist()
        {
            return s_instance != null;
        }

        /// <summary>
        /// 销毁单例
        /// </summary>
        public static void Dispose(bool immediate = false)
        {
            if (s_instance == null)
                return;

            if (immediate)
            {
                DestroyImmediate(s_instance.gameObject);
            }
            else
            {
                Destroy(s_instance.gameObject);
            }
        }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this as T;
                s_instance.OnCreateInstance();
            }
            OnAwake();
        }
        protected virtual void OnAwake() { }

        protected virtual void OnCreateInstance() { }
    }

    /// <summary>
    /// 单例特性,要使用MonoSingleton<>需要该特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MonoSingletonAttribute : Attribute
    {
        public string PrefabPath { get; private set; }

        public MonoSingletonAttribute(string prefabPath)
        {
            PrefabPath = prefabPath;
        }
    }
}
