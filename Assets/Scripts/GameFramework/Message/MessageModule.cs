using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

public class MessageModule : BaseGameModule
{
    // 异步处理消息的委托
    public delegate Task MessageHandlerEventArgs<T>(T arg);
    // 全局消息
    private Dictionary<Type, List<object>> globalMessageHandlers;
    // 本地消息
    private Dictionary<Type, List<object>> localMessageHandlers;

    public Monitor Monitor { get; private set; }

    /// <summary>
    /// 初始化模块
    /// </summary>
    protected internal override void OnModuleInit()
    {
        base.OnModuleInit();
        // 初始本地消息字典
        localMessageHandlers = new Dictionary<Type, List<object>>();
        Monitor = new Monitor();
        // 全局消息字典
        LoadAllMessageHandlers();
    }
    /// <summary>
    /// 停用模块就回收存放消息的容器
    /// </summary>
    protected internal override void OnModuleStop()
    {
        base.OnModuleStop();
        globalMessageHandlers = null;
        localMessageHandlers = null;
    }

    /// <summary>
    /// 加载所有的消息处理程序
    /// </summary>
    private void LoadAllMessageHandlers()
    {
        globalMessageHandlers = new Dictionary<Type, List<object>>();
        // 反射机制获取当前程序集中的所有类型
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            // 如果当前类型是抽象类型，则跳过该类型，因为抽象类型不能被实例化。
            if (type.IsAbstract)
                continue;

            //检查类型是否包含MessageHandlerAttribute特性
            MessageHandlerAttribute messageHandlerAttribute = type.GetCustomAttribute<MessageHandlerAttribute>(true);
            if (messageHandlerAttribute != null)
            {
                // 创建实例接口
                IMessageHander messageHandler = Activator.CreateInstance(type) as IMessageHander;
                if (!globalMessageHandlers.ContainsKey(messageHandler.GetHandlerType()))
                {
                    globalMessageHandlers.Add(messageHandler.GetHandlerType(), new List<object>());
                }
                globalMessageHandlers[messageHandler.GetHandlerType()].Add(messageHandler);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="handler"></param>
    public void Subscribe<T>(MessageHandlerEventArgs<T> handler)
    {
        // 先获取消息类型T
        Type argType = typeof(T);
        // 检查是否已经有处理该类型消息。如果没有，则创建一个新的列表并添加到字典中
        if (!localMessageHandlers.TryGetValue(argType, out var handlerList))
        {
            handlerList = new List<object>();
            localMessageHandlers.Add(argType, handlerList);
        }

        handlerList.Add(handler);
    }

    /// <summary>
    /// 取消订阅
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="handler"></param>
    public void Unsubscribe<T>(MessageHandlerEventArgs<T> handler)
    {
        if (!localMessageHandlers.TryGetValue(typeof(T), out var handlerList))
            return;

        handlerList.Remove(handler);
    }

    /// <summary>
    /// 广播
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arg"></param>
    /// <returns></returns>
    public async Task Post<T>(T arg) where T : struct
    {
        if (globalMessageHandlers.TryGetValue(typeof(T), out List<object> globalHandlerList))
        {
            foreach (var handler in globalHandlerList)
            {
                // 是否包含该消息的处理列表
                if (!(handler is MessageHandler<T> messageHandler))
                    continue;

                // 执行所有消息处理的方法
                await messageHandler.HandleMessage(arg);
            }
        }

       
        if (localMessageHandlers.TryGetValue(typeof(T), out List<object> localHandlerList))
        {
            // 从对象池中拿取一个列表  Obtain 如果有就直接拿 如果没有了就创建一个
            List<object> list = ListPool<object>.Obtain();
            // 给列表赋值
            list.AddRangeNonAlloc(localHandlerList);
            foreach (var handler in list)
            {
                if (!(handler is MessageHandlerEventArgs<T> messageHandler))
                    continue;

                await messageHandler(arg);
            }
            // 回收对象池
            ListPool<object>.Release(list);
        }
    }
}

