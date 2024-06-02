namespace TGame.ECS
{
    public interface IUpdateSystem
    {
        bool ObservingEntity(ECSEntity entity);
        void Update(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class UpdateSystem<C1> : IUpdateSystem where C1 : ECSComponent
    {
        public bool ObservingEntity(ECSEntity entity)
        {
            if (!entity.HasComponent<C1>())
                return false;

            return true;
        }

        public abstract void Update(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class UpdateSystem<C1, C2> : IUpdateSystem where C1 : ECSComponent where C2 : ECSComponent
    {
        public bool ObservingEntity(ECSEntity entity)
        {
            if (!entity.HasComponent<C1>())
                return false;

            if (!entity.HasComponent<C2>())
                return false;

            return true;
        }

        public abstract void Update(ECSEntity entity);
    }

    [ECSSystem]
    public abstract class UpdateSystem<C1, C2, C3> : IUpdateSystem where C1 : ECSComponent where C2 : ECSComponent where C3 : ECSComponent
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

        public abstract void Update(ECSEntity entity);
    }
}
