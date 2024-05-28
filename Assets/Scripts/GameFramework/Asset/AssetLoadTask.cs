using System;

namespace TGame.Asset
{
    public class AssetLoadTask
    {
        public enum TaskState
        {
            Suspend = 0,
            Waiting = 1,
            Loading = 2,
            Finish = 3,
        }

        public delegate void OnLoadFinishEventHandler(UnityEngine.Object asset);

        private static int idGenerator;

        public int ID { get; set; }
        public TaskState State { get; internal set; }
        public Type AssetType { get; private set; }
        public AssetData Data { get; set; }
        public event OnLoadFinishEventHandler OnLoadFinish;

        public AssetLoadTask(AssetData data, Type assetType)
        {
            ID = ++idGenerator;
            Data = data;
            AssetType = assetType;
        }

        public void LoadFinish()
        {
            //Ulog.Log($"Load Asset Finish:{Data.Path}");
            OnLoadFinish?.Invoke(Data.Asset);
        }
    }
}
