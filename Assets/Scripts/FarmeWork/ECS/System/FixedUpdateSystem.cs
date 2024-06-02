namespace TGame.ECS
{
    public interface IFixedUpdateSystem
    {
        bool ObservingEntity(ECSEntity entity);
        void FixedUpdate(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class FixedUpdateSystem<C1> : IFixedUpdateSystem where C1 : ECSComponent
    {
        public bool ObservingEntity(ECSEntity entity)
        {
            if (!entity.HasComponent<C1>())
                return false;

            return true;
        }

        public abstract void FixedUpdate(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class FixedUpdateSystem<C1, C2> : IFixedUpdateSystem where C1 : ECSComponent where C2 : ECSComponent
    {
        public bool ObservingEntity(ECSEntity entity)
        {
            if (!entity.HasComponent<C1>())
                return false;

            if (!entity.HasComponent<C2>())
                return false;

            return true;
        }

        public abstract void FixedUpdate(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class FixedUpdateSystem<C1, C2, C3> : IFixedUpdateSystem where C1 : ECSComponent where C2 : ECSComponent where C3 : ECSComponent
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

        public abstract void FixedUpdate(ECSEntity entity);
    }
}
