using System;
using System.Threading.Tasks;
/// <summary>
/// ��ȡ���������Ϣ���͵Ľӿ�
/// </summary>
public interface IMessageHander
{
    Type GetHandlerType();
}

/// <summary>
/// �޶�����Ϊstruct
/// </summary>
/// <typeparam name="T"></typeparam>
[MessageHandler]
public abstract class MessageHandler<T> : IMessageHander where T : struct
{
    // ʵ�ֽӿڵķ����������������Ϣ����
    public Type GetHandlerType()
    {
        return typeof(T);
    }
    // ������Ϣ�ĳ��󷽷�
    public abstract Task HandleMessage(T arg);
}

/// <summary>
/// ����Լ�� ֻ��Ӧ������,���Ա��̳�,���ܶ��Ӧ�ø�����
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
sealed class MessageHandlerAttribute : Attribute { }

