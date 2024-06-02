using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Unity.Properties;
using UnityEngine.Rendering.VirtualTexturing;
using TGame.Procedure;
using OpenCover.Framework.Model;
using TGame.UI;
using TGame.Asset;
using TGame.ECS;

namespace Koakuma.Game
{
    public class GameManager : MonoBehaviour
    {
        // 使用ModuleAttribute为不同模块提供优先级
        [Module(1)]
        public static AssetModule Asset { get => TGameFrameWork.Instance.GetModule<AssetModule>(); }
        [Module(2)]
        public static ProcedureModule Procedure { get => TGameFrameWork.Instance.GetModule<ProcedureModule>(); }
        [Module(3)]
        public static UIModule UI { get => TGameFrameWork.Instance.GetModule<UIModule>(); }

        [Module(4)]
        public static MessageModule Message { get => TGameFrameWork.Instance.GetModule<MessageModule>(); }

        [Module(5)]
        public static ECSModule ECS { get => TGameFrameWork.Instance.GetModule<ECSModule>(); }

        private bool activing;

        private void Awake()
        {
            // 检查TGameFrameWork实例是否存在，如果存在则销毁当前游戏对象
            if (TGameFrameWork.Instance != null)
            {
                Destroy(gameObject);
            }

            // 表示游戏被激活
            activing = true;

            // 设置对象在场景切换时不被销毁
            DontDestroyOnLoad(gameObject);

            // 初始化游戏框架
            TGameFrameWork.Initialize();

            // 启动所有模块
            StartupModules();

            // 初始化所有模块
            TGameFrameWork.Instance.InitModules();
        }

        private void StartupModules()
        {
            // 创建一个存放模块特性的列表
            List<ModuleAttribute> moduleAttrs = new List<ModuleAttribute>();
            // 通过反射获取所有静态和非公共的属性
            PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            // 声明一个BaseGameModule的类型
            Type baseCompType = typeof(BaseGameModule);

            // 遍历所有属性
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo property = propertyInfos[i];
                // 检查属性是否是BaseGameModule类或者是BaseGameModule的子类
                if (!baseCompType.IsAssignableFrom(property.PropertyType))
                {
                    continue;
                }

                // 获取所有的ModuleAttribute特性
                object[] attrs = property.GetCustomAttributes(typeof(ModuleAttribute), false);
                if (attrs.Length <= 0)
                {
                    continue;
                }

                // 获取到该对象子类对象中获取该组件
                Component comp = GetComponentInChildren(property.PropertyType);
                if (comp == null)
                {
                    Debug.Log("找不到Module" + property.PropertyType);
                    continue;
                }

                ModuleAttribute moduleAttribute = attrs[0] as ModuleAttribute;
                // 特性跟模块基类绑定在一起
                moduleAttribute.Module = comp as BaseGameModule;
                moduleAttrs.Add(moduleAttribute);
            }

            // 按优先级对模块特性进行排序
            moduleAttrs.Sort((a, b) =>
            {
                return a.Priority - b.Priority;
            });

            // 添加模块到TGameFrameWork实例中
            for (int i = 0; i < moduleAttrs.Count; i++)
            {
                TGameFrameWork.Instance.AddModule(moduleAttrs[i].Module);
            }
        }

        private void Start()
        {
            // 启动所有模块
            TGameFrameWork.Instance.StartModules();
            // 开始程序流程
            Procedure.StartProcedure().Coroutine();
        }

        private void Update()
        {
            // 每帧更新TGameFrameWork实例
            TGameFrameWork.Instance.Update();
        }

        private void LateUpdate()
        {
            // 每帧晚些时候更新TGameFrameWork实例
            TGameFrameWork.Instance.LateUpdate();
        }

        private void FixedUpdate()
        {
            // 每固定帧率更新TGameFrameWork实例
            TGameFrameWork.Instance.FixedUpdate();
        }

        private void OnDestroy()
        {
            // 脚本被销毁时调用
            if (activing)
            {
                // 销毁TGameFrameWork实例
                TGameFrameWork.Instance.Destroy();
            }
        }

        // 自定义ModuleAttribute类，用于为模块提供优先级
        [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
        public sealed class ModuleAttribute : Attribute, IComparable<ModuleAttribute>
        {
            // 优先级
            public int Priority { get; private set; }
            // 模块基类
            public BaseGameModule Module { get; set; }

            public ModuleAttribute(int priority)
            {
                Priority = priority;
            }

            // 实现IComparable接口，用于模块特性排序
            public int CompareTo(ModuleAttribute other)
            {
                return Priority.CompareTo(other.Priority);
            }
        }
    }
}
