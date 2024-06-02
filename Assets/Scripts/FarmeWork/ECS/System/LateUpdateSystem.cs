namespace TGame.ECS
{
    public interface ILateUpdateSystem
    {
        bool ObservingEntity(ECSEntity entity);
        void LateUpdate(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class LateUpdateSystem<C1> : ILateUpdateSystem where C1 : ECSComponent
    {
        public bool ObservingEntity(ECSEntity entity)
        {
            if (!entity.HasComponent<C1>())
                return false;

            return true;
        }

        public abstract void LateUpdate(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class LateUpdateSystem<C1, C2> : ILateUpdateSystem where C1 : ECSComponent where C2 : ECSComponent
    {
        public bool ObservingEntity(ECSEntity entity)
        {
            if (!entity.HasComponent<C1>())
                return false;

            if (!entity.HasComponent<C2>())
                return false;

            return true;
        }

        public abstract void LateUpdate(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class LateUpdateSystem<C1, C2, C3> : ILateUpdateSystem where C1 : ECSComponent where C2 : ECSComponent where C3 : ECSComponent
    {
        public bool ObservingEntity(ECSEntity entity)
        {
            if (!entity.HasComponent<C1>())
                return false;

            if (!entity.HasComponent<C2>())
                return false;

            if (!entity.HasComponent<C3>())
                return false;

            return true;
        }

        public abstract void LateUpdate(ECSEntity entity);
    }
}
