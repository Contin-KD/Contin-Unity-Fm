using System;
using System.Threading.Tasks;
/// <summary>
/// 获取所处理的消息类型的接口
/// </summary>
public interface IMessageHander
{
    Type GetHandlerType();
}

/// <summary>
/// 限定类型为struct
/// </summary>
/// <typeparam name="T"></typeparam>
[MessageHandler]
public abstract class MessageHandler<T> : IMessageHander where T : struct
{
    // 实现接口的方法返回所处理的消息类型
    public Type GetHandlerType()
    {
        return typeof(T);
    }
    // 处理消息的抽象方法
    public abstract Task HandleMessage(T arg);
}

/// <summary>
/// 特性约束 只能应用于类,可以被继承,不能多次应用该特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
sealed class MessageHandlerAttribute : Attribute { }

