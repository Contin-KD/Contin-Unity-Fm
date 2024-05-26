using System;

namespace UnityEngine
{
    /// <summary>
    /// 作者: Teddy
    /// 时间: 2018/03/21
    /// 功能: 
    /// </summary>
	public static class TransformExt
    {
        /// <summary>
        /// 设置父节点并且重置位置,旋转和缩放
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="parent"></param>
        public static void SetParentAndResetAll(this Transform transform, Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 查找子物体,根据子物体名称和组件类型
        /// </summary>
        /// <typeparam name="T">子物体上的某个组件类型</typeparam>
        /// <param name="transform"></param>
        /// <param name="name">子物体名称,*匹配任意名称</param>
        /// <param name="index">子物体序号</param>
        /// <returns></returns>
        public static T FindChild<T>(this Transform transform, string name, int index) where T : Component
        {
            return FindChild(transform, typeof(T), name, index) as T;
        }

        public static Component FindChild(this Transform transform, Type type, string name, int index)
        {
            int currentIndex = 0;
            Component[] components = transform.GetComponentsInChildren(type, true);
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].name == name || name == "*")
                {
                    if (index == currentIndex++)
                    {
                        return components[i];
                    }
                }
            }
            return null;
        }
    }
}