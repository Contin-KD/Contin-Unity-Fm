using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class TGameFrameWork
{
    public static TGameFrameWork Instance { get; private set; }

    public static bool Initialized { get; private set; }

    public Dictionary<Type, BaseGameModule> modules = new Dictionary<Type, BaseGameModule>();
    public static void Initialize()
    {
        Instance = new TGameFrameWork();
    }

    internal void InitModules()
    {
        if (Initialized)
        {
            return;
        }
        Initialized = true;
        // 执行所有的模块初始化
        foreach (var item in modules.Values)
        {
            item.OnModuleInit();
        }
    }
    /// <summary>
    /// 获取模块
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetModule<T>() where T : BaseGameModule
    {
        if (modules.TryGetValue(typeof(T), out BaseGameModule module))
        {
            return module as T;
        }

        return default(T);
    }
    /// <summary>
    /// 添加模块统一管理
    /// </summary>
    /// <param name="module"></param>
    internal void AddModule(BaseGameModule module)
    {
        Type moduleType = module.GetType();
        if (modules.ContainsKey(moduleType))
        {
            UnityEngine.Debug.Log($"Module添加失败，重复添加:{moduleType.Name}");
            return;
        }
        modules.Add(moduleType, module);
    }

    public void StartModules()
    {
        if (modules == null)
            return;

        if (!Initialized)
            return;

        foreach (var module in modules.Values)
        {
            module.OnModuleStart();
        }
    }
    public void Update()
    {
        if (modules == null)
            return;

        if (!Initialized)
            return;

        float deltaTime = UnityEngine.Time.deltaTime;
        foreach (var module in modules.Values)
        {
            module.OnModuleUpdate(deltaTime);
        }
    }

    public void LateUpdate()
    {
        if (modules == null)
            return;

        if (!Initialized)
            return;

        float deltaTime = UnityEngine.Time.deltaTime;
        foreach (var module in modules.Values)
        {
            module.OnModuleLateUpdate(deltaTime);
        }
    }

    public void FixedUpdate()
    {
        if (modules == null)
            return;

        if (!Initialized)
            return;

        float deltaTime = UnityEngine.Time.fixedDeltaTime;
        foreach (var module in modules.Values)
        {
            module.OnModuleFixedUpdate(deltaTime);
        }
    }
    public void Destroy()
    {
        if (!Initialized)
            return;

        if (Instance != this)
            return;

        if (Instance.modules == null)
            return;

        foreach (var module in Instance.modules.Values)
        {
            module.OnModuleStop();
        }

        //Destroy(Instance.gameObject);
        Instance = null;
        Initialized = false;
    }
}
