using System;
using UnityEngine;

namespace TGame.Common
{
    /// <summary>
    /// 作者: Teddy
    /// 时间: 2018/03/16
    /// 功能: 
    /// </summary>
    [Serializable]
    public sealed class TypeSelector
    {
#pragma warning disable CS0414 // 内联变量声明
        [SerializeField]
        private string baseTypeName = null;
#pragma warning restore CS0414 // 内联变量声明
        [SerializeField]
        private string selectedType = null;

        private object instance;

        public TypeSelector(Type baseType)
        {
            baseTypeName = baseType.FullName;
        }

        public T GetOrCreateInstance<T>()
        {
            if (instance != null)
            {
                return (T)instance;
            }

            if (string.IsNullOrEmpty(selectedType) || selectedType == "<Null>")
            {
                Debug.LogError($"Type selector doesn't set a type");
                return default(T);
            }

            Type t = Type.GetType(selectedType);
            if (t == null)
            {
                Debug.LogError($"Type selector can't find a type you select:{selectedType}");
                return default(T);
            }

            instance = Activator.CreateInstance(t);
            if (instance == null)
            {
                Debug.LogError($"Type selector create instance failed:{selectedType}");
                return default(T);
            }

            return (T)instance;
        }
    }
}