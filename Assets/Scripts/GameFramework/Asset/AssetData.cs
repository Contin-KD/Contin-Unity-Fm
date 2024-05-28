using UnityEngine;

namespace TGame.Asset
{
    public class AssetData
    {
        public string Path { get; }
        public Object Asset { get; set; }
        public AssetLoadState State { get; set; }
        public string LuaAsset { get; set; }

        public AssetData(string path)
        {
            Path = path.ToLower();
        }
    }
}
