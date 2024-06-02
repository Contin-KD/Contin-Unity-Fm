using System.Collections.Generic;

namespace TGame.ECS
{
    public class ECSScene : ECSEntity
    {
        private Dictionary<long, ECSEntity> entities;

        public ECSScene()
        {
            entities = new Dictionary<long, ECSEntity>();
        }

        public override void Dispose()
        {
            if (Disposed)
                return;

            List<long> entityIDList = ListPool<long>.Obtain();
            foreach (var entityID in entities.Keys)
            {
                entityIDList.Add(entityID);
            }
            foreach (var entityID in entityIDList)
            {
                ECSEntity entity = entities[entityID];
                entity.Dispose();
            }
            ListPool<long>.Release(entityIDList);

            base.Dispose();
        }

        public void AddEntity(ECSEntity entity)
        {
            if (entity == null)
                return;

            ECSScene oldScene = entity.Scene;
            if (oldScene != null)
            {
                oldScene.RemoveEntity(entity.InstanceID);
            }

            entities.Add(entity.InstanceID, entity);
            entity.SceneID = InstanceID;
            UnityEngine.Debug.Log($"Scene Add Entity, Current Count:{entities.Count}");
        }

        public void RemoveEntity(long entityID)
        {
            if (entities.Remove(entityID))
            {
                UnityEngine.Debug.Log($"Scene Remove Entity, Current Count:{entities.Count}");
            }
        }

        public void FindEntities<T>(List<long> list) where T : ECSEntity
        {
            foreach (var item in entities)
            {
                if (item.Value is T)
                {
                    list.Add(item.Key);
                }
            }
        }

        public void FindEntitiesWithComponent<T>(List<long> list) where T : ECSComponent
        {
            foreach (var item in entities)
            {
                if (item.Value.HasComponent<T>())
                {
                    list.Add(item.Key);
                }
            }
        }

        public void GetAllEntities(List<long> list)
        {
            foreach (var item in entities)
            {
                list.Add(item.Key);
            }
        }
    }
}