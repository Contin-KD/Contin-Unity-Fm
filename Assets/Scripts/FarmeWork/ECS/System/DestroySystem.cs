using System;

namespace TGame.ECS
{
    public interface IDestroySystem : ISystem
    {
        Type ComponentType();
        void Destroy(object c);
    }

    [ECSSystem]
    public abstract class DestroySystem<C> : IDestroySystem where C : ECSComponent
    {
        public abstract void Destroy(C c);
        public void Destroy(object c)
        {
            Destroy((C)c);
        }

        public Type ComponentType()
        {
            return typeof(C);
        }

        public Type SystemType()
        {
            return GetType();
        }
    }

    [ECSSystem]
    public abstract class DestroySystem<C, P1> : IDestroySystem where C : ECSComponent
    {
        public abstract void Destroy(C c, P1 p1);
        public void Destroy(object c) { }

        public Type ComponentType()
        {
            return typeof(C);
        }

        public Type SystemType()
        {
            return GetType();
        }
    }


    [ECSSystem]
    public abstract class DestroySystem<C, P1, P2> : IDestroySystem where C : ECSComponent
    {
        public abstract void Destroy(C c, P1 p1, P2 p2);
        public void Destroy(object c) { }

        public Type ComponentType()
        {
            return typeof(C);
        }

        public Type SystemType()
        {
            return GetType();
        }
    }
}
