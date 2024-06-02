using System;
using System.Collections.Generic;
using UnityEngine; // 假设使用 UnityEngine 命名空间来进行日志记录和时间函数

public class TGameFrameWork
{
    // TGameFrameWork 的单例实例
    public static TGameFrameWork Instance { get; private set; }

    // 指示框架是否已初始化
    public static bool Initialized { get; private set; }

    // 存储游戏模块的字典，以模块类型作为键
    public Dictionary<Type, BaseGameModule> modules = new Dictionary<Type, BaseGameModule>();

    // 初始化框架实例
    public static void Initialize()
    {
        Instance = new TGameFrameWork();
    }

    // 初始化所有已注册的模块
    internal void InitModules()
    {
        // 防止重复初始化
        if (Initialized)
        {
            return;
        }

        // 标记为已初始化
        Initialized = true;

        // 遍历所有模块并调用它们的 OnModuleInit 方法
        foreach (var item in modules.Values)
        {
            item.OnModuleInit();
        }
    }

    /// <summary>
    /// 获取类型为 T 的模块（如果存在）。
    /// </summary>
    /// <typeparam name="T">要获取的模块类型</typeparam>
    /// <returns>如果找到则返回模块实例，否则返回 T 的默认值</returns>
    public T GetModule<T>() where T : BaseGameModule
    {
        if (modules.TryGetValue(typeof(T), out BaseGameModule module))
        {
            return module as T;
        }

        return default(T);
    }

    /// <summary>
    /// 添加模块到框架进行统一管理。
    /// </summary>
    /// <param name="module">要添加的模块实例</param>
    internal void AddModule(BaseGameModule module)
    {
        Type moduleType = module.GetType();

        // 防止添加重复的模块
        if (modules.ContainsKey(moduleType))
        {
            UnityEngine.Debug.Log($"模块添加失败，重复添加: {moduleType.Name}");
            return;
        }

        // 将模块添加到字典
        modules.Add(moduleType, module);
    }

    // 启动所有模块
    public void StartModules()
    {
        // 如果没有模块或未初始化则返回
        if (modules == null || !Initialized)
            return;

        // 遍历所有模块并调用它们的 OnModuleStart 方法
        foreach (var module in modules.Values)
        {
            module.OnModuleStart();
        }
    }

    // 更新所有模块
    public void Update()
    {
        // 如果没有模块或未初始化则返回
        if (modules == null || !Initialized)
            return;

        // 获取当前帧的时间
        float deltaTime = UnityEngine.Time.deltaTime;

        // 遍历所有模块并调用它们的 OnModuleUpdate 方法
        foreach (var module in modules.Values)
        {
            module.OnModuleUpdate(deltaTime);
        }
    }

    // LateUpdate 所有模块
    public void LateUpdate()
    {
        // 如果没有模块或未初始化则返回
        if (modules == null || !Initialized)
            return;

        // 获取当前帧的时间
        float deltaTime = UnityEngine.Time.deltaTime;

        // 遍历所有模块并调用它们的 OnModuleLateUpdate 方法
        foreach (var module in modules.Values)
        {
            module.OnModuleLateUpdate(deltaTime);
        }
    }

    // FixedUpdate 所有模块
    public void FixedUpdate()
    {
        // 如果没有模块或未初始化则返回
        if (modules == null || !Initialized)
            return;

        // 获取fixupdata每帧时间
        float deltaTime = UnityEngine.Time.fixedDeltaTime;

        // 遍历所有模块并调用它们的 OnModuleFixedUpdate 方法
        foreach (var module in modules.Values)
        {
            module.OnModuleFixedUpdate(deltaTime);
        }
    }

    // 销毁框架并停止所有模块
    public void Destroy()
    {
        // 如果未初始化则返回
        if (!Initialized)
            return;

        // 如果实例不是当前对象则返回
        if (Instance != this)
            return;

        // 如果没有模块则返回
        if (Instance.modules == null)
            return;

        // 遍历所有模块并调用它们的 OnModuleStop 方法
        foreach (var module in Instance.modules.Values)
        {
            module.OnModuleStop();
        }

        // 销毁实例
        Instance = null;
        Initialized = false;
    }
}
