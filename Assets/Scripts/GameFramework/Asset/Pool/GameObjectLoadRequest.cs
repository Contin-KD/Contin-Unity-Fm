using System;
using UnityEngine;

namespace TGame.Asset
{
    public class GameObjectLoadRequest<T> where T : GameObjectPoolAsset
    {
        public GameObjectLoadState State { get; private set; }
        public string Path { get; }
        public Action<GameObject> CreateNewCallback { get; }

        private Action<T> callback;

        public GameObjectLoadRequest(string path, Action<T> callback, Action<GameObject> createNewCallback)
        {
            Path = path;
            this.callback = callback;
            CreateNewCallback = createNewCallback;
        }

        public void LoadFinish(T obj)
        {
            if (State == GameObjectLoadState.Loading)
            {
                callback?.Invoke(obj);
                State = GameObjectLoadState.Finish;
            }
        }
    }
}
