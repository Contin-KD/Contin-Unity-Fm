using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Unity.Properties;
using UnityEngine.Rendering.VirtualTexturing;

public class GameManager : MonoBehaviour
{
    [Module(6)]
    public static MessageModule Message { get => TGameFrameWork.Instance.GetModule<MessageModule>(); }
    private bool activing;
    private void Awake()
    {
        if (TGameFrameWork.Instance != null)
        {
            Destroy(gameObject);
        }

        // 表示游戏被激活
        activing = true;

        //跳转不删除
        DontDestroyOnLoad(gameObject);

        // Application.logMessageReceived += OnReceiveLog;

        // 初始化游戏框架
        TGameFrameWork.Initialize();

        StartupModules();

        TGameFrameWork.Instance.InitModules();
    }

    private void StartupModules()
    {
        List<ModuleAttribute> moduleAttrs = new List<ModuleAttribute>();
        PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
        // 声明一个BaseGameModule的类型
        Type baseCompType = typeof(BaseGameModule);
        // 遍历反射获得的所有模块
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

        moduleAttrs.Sort((a, b) =>
        {
            return a.Priority - b.Priority;
        });

        for (int i = 0; i < moduleAttrs.Count; i++)
        {
            TGameFrameWork.Instance.AddModule(moduleAttrs[i].Module);
        }
    }
    private void Start()
    {
        TGameFrameWork.Instance.StartModules();
        // Procedure.StartProcedure().Coroutine();
    }

    private void Update()
    {
        TGameFrameWork.Instance.Update();
    }

    private void LateUpdate()
    {
        TGameFrameWork.Instance.LateUpdate();
    }

    private void FixedUpdate()
    {
        TGameFrameWork.Instance.FixedUpdate();
    }

    private void OnDestroy()
    {
        // 脚本被销毁时调用
        // 在脚本启用时activing会被赋ture表示被启用
        if (activing)
        {
            //// 取消日志绑定
            //Application.logMessageReceived -= OnReceiveLog;
            // 销毁TGameFrameWork实例
            TGameFrameWork.Instance.Destroy();
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ModuleAttribute : Attribute, IComparable<ModuleAttribute>
    {
        // 优先级
        public int Priority { get; private set; }
        // 模块基类(包含Unity生命周期方法和一些自定义生命周期方法,以便管理和使用模块)
        public BaseGameModule Module { get; set; }
        public ModuleAttribute(int priority)
        {
            Priority = priority;
        }
        /// <summary>
        /// 比较器
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int CompareTo(ModuleAttribute other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
    //private void OnReceiveLog(string condition, string stackTrace, LogType type)
    //{

    //}
}
