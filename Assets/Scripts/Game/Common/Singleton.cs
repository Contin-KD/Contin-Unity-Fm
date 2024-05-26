using System;
using System.Reflection;
using UnityEngine;

namespace TGame.Common
{
    /// <summary>
    /// 单例类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class
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

                    if (t.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        throw new InheritMonoException(t.Name);
                    }

                    ConstructorInfo ci = t.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null);
                    if (ci == null)
                    {
                        throw new PublicDefaultConstructorException(t.Name);
                    }

                    s_instance = ci.Invoke(null) as T;
                }
                return s_instance;
            }
        }

        /// <summary>
        /// 销毁单例
        /// </summary>
        public static void Dispose()
        {
            s_instance = null;
        }

        public static void MergeFrom(T t)
        {
            if (t == null)
                return;

            s_instance = t;
        }
    }

    /// <summary>
    /// 继承Mono异常
    /// </summary>
    [Serializable]
    public class InheritMonoException : Exception
    {
        public InheritMonoException(string className) : base(className + "要使用Singleton不能继承MonoBehaviour") { }
        protected InheritMonoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }

    /// <summary>
    /// 公开默认构造器异常
    /// </summary>
    [Serializable]
    public class PublicDefaultConstructorException : Exception
    {
        public PublicDefaultConstructorException(string className) : base(className + "要使用Singleton必须私有化默认构造器") { }
        protected PublicDefaultConstructorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
    }
}