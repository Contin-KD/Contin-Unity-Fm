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

    // public Monitor Monitor { get; private set; }

    /// <summary>
    /// 初始化模块
    /// </summary>
    protected internal override void OnModuleInit()
    {
        base.OnModuleInit();
        // 初始本地消息字典
        localMessageHandlers = new Dictionary<Type, List<object>>();
        // Monitor = new Monitor();
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
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            if (type.IsAbstract)
                continue;

            MessageHandlerAttribute messageHandlerAttribute = type.GetCustomAttribute<MessageHandlerAttribute>(true);
            if (messageHandlerAttribute != null)
            {
                IMessageHander messageHandler = Activator.CreateInstance(type) as IMessageHander;
                if (!globalMessageHandlers.ContainsKey(messageHandler.GetHandlerType()))
                {
                    globalMessageHandlers.Add(messageHandler.GetHandlerType(), new List<object>());
                }
                globalMessageHandlers[messageHandler.GetHandlerType()].Add(messageHandler);
            }
        }
    }

    public void Subscribe<T>(MessageHandlerEventArgs<T> handler)
    {
        Type argType = typeof(T);
        if (!localMessageHandlers.TryGetValue(argType, out var handlerList))
        {
            handlerList = new List<object>();
            localMessageHandlers.Add(argType, handlerList);
        }

        handlerList.Add(handler);
    }

    public void Unsubscribe<T>(MessageHandlerEventArgs<T> handler)
    {
        if (!localMessageHandlers.TryGetValue(typeof(T), out var handlerList))
            return;

        handlerList.Remove(handler);
    }

    public async Task Post<T>(T arg) where T : struct
    {
        if (globalMessageHandlers.TryGetValue(typeof(T), out List<object> globalHandlerList))
        {
            foreach (var handler in globalHandlerList)
            {
                if (!(handler is MessageHandler<T> messageHandler))
                    continue;

                await messageHandler.HandleMessage(arg);
            }
        }

        //if (localMessageHandlers.TryGetValue(typeof(T), out List<object> localHandlerList))
        //{
        //    List<object> list = ListPool<object>.Obtain();
        //    list.AddRangeNonAlloc(localHandlerList);
        //    foreach (var handler in list)
        //    {
        //        if (!(handler is MessageHandlerEventArgs<T> messageHandler))
        //            continue;

        //        await messageHandler(arg);
        //    }
        //    ListPool<object>.Release(list);
        //}
    }
}

