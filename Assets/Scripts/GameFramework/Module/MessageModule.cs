using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

public class MessageModule : BaseGameModule
{
    // �첽������Ϣ��ί��
    public delegate Task MessageHandlerEventArgs<T>(T arg);
    // ȫ����Ϣ
    private Dictionary<Type, List<object>> globalMessageHandlers;
    // ������Ϣ
    private Dictionary<Type, List<object>> localMessageHandlers;

    public Monitor Monitor { get; private set; }

    /// <summary>
    /// ��ʼ��ģ��
    /// </summary>
    protected internal override void OnModuleInit()
    {
        base.OnModuleInit();
        // ��ʼ������Ϣ�ֵ�
        localMessageHandlers = new Dictionary<Type, List<object>>();
        Monitor = new Monitor();
        // ȫ����Ϣ�ֵ�
        LoadAllMessageHandlers();
    }
    /// <summary>
    /// ͣ��ģ��ͻ��մ����Ϣ������
    /// </summary>
    protected internal override void OnModuleStop()
    {
        base.OnModuleStop();
        globalMessageHandlers = null;
        localMessageHandlers = null;
    }

    /// <summary>
    /// �������е���Ϣ�������
    /// </summary>
    private void LoadAllMessageHandlers()
    {
        globalMessageHandlers = new Dictionary<Type, List<object>>();
        // ������ƻ�ȡ��ǰ�����е���������
        foreach (var type in Assembly.GetCallingAssembly().GetTypes())
        {
            // �����ǰ�����ǳ������ͣ������������ͣ���Ϊ�������Ͳ��ܱ�ʵ������
            if (type.IsAbstract)
                continue;

            //��������Ƿ����MessageHandlerAttribute����
            MessageHandlerAttribute messageHandlerAttribute = type.GetCustomAttribute<MessageHandlerAttribute>(true);
            if (messageHandlerAttribute != null)
            {
                // ����ʵ���ӿ�
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
        // �Ȼ�ȡ��Ϣ����T
        Type argType = typeof(T);
        // ����Ƿ��Ѿ��д����������Ϣ�����û�У��򴴽�һ���µ��б���ӵ��ֵ���
        if (!localMessageHandlers.TryGetValue(argType, out var handlerList))
        {
            handlerList = new List<object>();
            localMessageHandlers.Add(argType, handlerList);
        }

        handlerList.Add(handler);
    }

    /// <summary>
    /// ȡ������
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
    /// �㲥
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
                // �Ƿ��������Ϣ�Ĵ����б�
                if (!(handler is MessageHandler<T> messageHandler))
                    continue;

                // ִ��������Ϣ����ķ���
                await messageHandler.HandleMessage(arg);
            }
        }

       
        if (localMessageHandlers.TryGetValue(typeof(T), out List<object> localHandlerList))
        {
            // �Ӷ��������ȡһ���б�  Obtain ����о�ֱ���� ���û���˾ʹ���һ��
            List<object> list = ListPool<object>.Obtain();
            // ���б�ֵ
            list.AddRangeNonAlloc(localHandlerList);
            foreach (var handler in list)
            {
                if (!(handler is MessageHandlerEventArgs<T> messageHandler))
                    continue;

                await messageHandler(arg);
            }
            // ���ն����
            ListPool<object>.Release(list);
        }
    }
}

