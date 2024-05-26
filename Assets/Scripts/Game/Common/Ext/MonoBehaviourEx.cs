using System;
using System.Reflection;

namespace UnityEngine
{
    public static class MonoBehaviourEx
    {
        /// <summary>
        /// 绑定子节点
        /// </summary>
        /// <param name="monoBehaviour"></param>
        public static void BindChildren(this MonoBehaviour monoBehaviour)
        {
            Type monoType = monoBehaviour.GetType();

            FieldInfo[] fields = monoType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                object[] bindChildAttrs = fields[i].GetCustomAttributes(typeof(BindAttribute), false);
                if (bindChildAttrs.Length > 0)
                {
                    BindAttribute bindAttr = bindChildAttrs[0] as BindAttribute;
                    string childName = bindAttr.ChildName == "this" ? monoBehaviour.name : bindAttr.ChildName;
                    Component child = monoBehaviour.transform.FindChild(fields[i].FieldType, childName, bindAttr.Index);
                    if (child == null && bindAttr.AttachComponentType != null)
                    {
                        child = monoBehaviour.transform.FindChild<Transform>(childName, bindAttr.Index);
                        if (child != null)
                        {
                            child = child.gameObject.AddComponent(bindAttr.AttachComponentType);
                        }
                    }

                    if (child != null)
                    {
                        fields[i].SetValue(monoBehaviour, child);
                        child.gameObject.SetActive(bindAttr.IsDefaultActive);
                    }
                    else
                    {
                        throw new BindFaildException("绑定子物体失败,类型:" + fields[i].FieldType + "与名称:" + bindAttr.ChildName + "不匹配");
                    }
                }
            }
        }

        /// <summary>
        /// 绑定子物体失败异常
        /// </summary>
        [Serializable]
        public class BindFaildException : Exception
        {
            public BindFaildException() { }
            public BindFaildException(string message) : base(message) { }
            public BindFaildException(string message, Exception inner) : base(message, inner) { }
            protected BindFaildException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context)
            { }
        }
    }

    /// <summary>
    /// 绑定子物体特性,在需要的字段上添加该特性后再调用Monobehaviour.BindChildren可以自动绑定所有子物体
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class BindAttribute : Attribute
    {
        public string ChildName { get; }
        public int Index { get; }
        public bool IsDefaultActive { get; }
        public Type AttachComponentType { get; }

        public BindAttribute(string childName, int index, bool isDefaultActive, Type attachComponentType)
        {
            ChildName = childName;
            Index = index;
            IsDefaultActive = isDefaultActive;
            AttachComponentType = attachComponentType;
        }

        public BindAttribute(string childName) : this(childName, 0, true, null)
        {
        }

        public BindAttribute(string childName, bool isDefaultActive) : this(childName, 0, isDefaultActive, null)
        {
        }

        public BindAttribute(string childName, Type attachComponentType) : this(childName, 0, true, attachComponentType)
        {
        }

        public BindAttribute(int index) : this("*", index, true, null)
        {
        }

        public BindAttribute(int index, bool isDefaultActive) : this("*", index, isDefaultActive, null)
        {
        }

        public BindAttribute(int index, Type attachComponentType) : this("*", index, true, attachComponentType)
        {
        }

        public BindAttribute(string childName, int index) : this(childName, index, true, null)
        {
        }
    }
}
