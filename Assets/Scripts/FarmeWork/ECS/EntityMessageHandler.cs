using System;
using System.Threading.Tasks;
using TGame.ECS;

namespace TGame
{
    public interface IEntityMessageHandler
    {
        Type MessageType();
    }

    public interface IEntityMessageHandler<M> : IEntityMessageHandler
    {
        Task Post(ECSEntity entity, M m);
    }

    [EntityMessageHandler]
    public abstract class EntityMessageHandler<M> : IEntityMessageHandler<M>
    {
        public abstract Task HandleMessage(ECSEntity entity, M message);

        public Type MessageType()
        {
            return typeof(M);
        }

        public async Task Post(ECSEntity entity, M m)
        {
            await HandleMessage(entity, m);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class EntityMessageHandlerAttribute : Attribute
    {
    }


    public interface IEntityRpcHandler
    {
        Type RpcType();
    }

    public interface IEntityRpcResponse
    {
        bool Error { get; set; }
        string ErrorMessage { get; set; }
    }

    public interface IEntityRpcHandler<Request, Response> : IEntityRpcHandler where Response : IEntityRpcResponse
    {
        Task<Response> Post(ECSEntity entity, Request request);
    }

    [EntityRpcHandler]
    public abstract class EntityRpcHandler<Request, Response> : IEntityRpcHandler<Request, Response> where Response : IEntityRpcResponse
    {
        public abstract Task<Response> HandleRpc(ECSEntity entity, Request request);

        public async Task<Response> Post(ECSEntity entity, Request request)
        {
            return await HandleRpc(entity, request);
        }

        public Type RpcType()
        {
            return typeof(Request);
        }
    }


    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class EntityRpcHandlerAttribute : Attribute
    {
    }
}
