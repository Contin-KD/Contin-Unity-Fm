namespace TGame.ECS
{
    public abstract class ECSComponent
    {
        public long ID { get; private set; }
        public long EntityID { get; set; }
        public bool Disposed { get; set; }

        public ECSEntity Entity
        {
            get
            {
                if (EntityID == 0)
                    return default;

                return TGameFrameWork.Instance.GetModule<ECSModule>().FindEntity(EntityID);
            }
        }

        public ECSComponent()
        {
            ID = IDGenerator.NewInstanceID();
            Disposed = false;
        }
    }
}
