using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
namespace Config
{
    public partial struct BulletConfig
    {
        public static void DeserializeByAddressable(string directory)
        {
            string path = $"{directory}/BulletConfig.json";
            UnityEngine.TextAsset ta = Addressables.LoadAssetAsync<UnityEngine.TextAsset>(path).WaitForCompletion();
            string json = ta.text;
            datas = new List<BulletConfig>();
            indexMap = new Dictionary<int, int>();
            JArray array = JArray.Parse(json);
            Count = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                JObject dataObject = array[i] as JObject;
                BulletConfig data = (BulletConfig)dataObject.ToObject(typeof(BulletConfig));
                datas.Add(data);
                indexMap.Add(data.ID, i);
            }
        }
        public static void DeserializeByFile(string directory)
        {
            string path = $"{directory}/BulletConfig.json";
            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fs))
                {
                    datas = new List<BulletConfig>();
                    indexMap = new Dictionary<int, int>();
                    string json = reader.ReadToEnd();
                    JArray array = JArray.Parse(json);
                    Count = array.Count;
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject dataObject = array[i] as JObject;
                        BulletConfig data = (BulletConfig)dataObject.ToObject(typeof(BulletConfig));
                        datas.Add(data);
                        indexMap.Add(data.ID, i);
                    }
                }
            }
        }
        public static System.Collections.IEnumerator DeserializeByBundle(string directory, string subFolder)
        {
            string bundleName = $"{subFolder}/BulletConfig.bytes".ToLower();
            string fullBundleName = $"{directory}/{bundleName}";
            string assetName = $"assets/{bundleName}";
            #if UNITY_WEBGL && !UNITY_EDITOR
            UnityEngine.AssetBundle bundle = null;
            UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(fullBundleName);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                UnityEngine.Debug.LogError(request.error);
            }
            else
            {
                bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
            }
            #else
            yield return null;
            UnityEngine.AssetBundle bundle = UnityEngine.AssetBundle.LoadFromFile($"{fullBundleName}", 0, 0);
            #endif
            UnityEngine.TextAsset ta = bundle.LoadAsset<UnityEngine.TextAsset>($"{assetName}");
            string json = ta.text;
            datas = new List<BulletConfig>();
            indexMap = new Dictionary<int, int>();
            JArray array = JArray.Parse(json);
            Count = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                JObject dataObject = array[i] as JObject;
                BulletConfig data = (BulletConfig)dataObject.ToObject(typeof(BulletConfig));
                datas.Add(data);
                indexMap.Add(data.ID, i);
            }
        }
        public static int Count;
        private static List<BulletConfig> datas;
        private static Dictionary<int, int> indexMap;
        public static BulletConfig ByID(int id)
        {
            if (id <= 0)
            {
                return Null;
            }
            if (!indexMap.TryGetValue(id, out int index))
            {
                throw new System.Exception($"BulletConfig找不到ID:{id}");
            }
            return ByIndex(index);
        }
        public static BulletConfig ByIndex(int index)
        {
            return datas[index];
        }
        public bool IsNull { get; private set; }
        public static BulletConfig Null { get; } = new BulletConfig() { IsNull = true }; 
        public System.Int32 ID { get; set; }
        public string Description { get; set; }
        public BulletMode Mode { get; set; }
        public FireMode FireMode { get; set; }
        public System.Single[] FireArgs { get; set; }
        public System.Single FixedPointRandomOffset { get; set; }
        public System.Int32 FireCount { get; set; }
        public System.Single FireInterval { get; set; }
        public FXBindBone FireBone { get; set; }
        public System.Int32[] FX { get; set; }
        public System.Int32[] HitFX { get; set; }
        public System.Int32 SFX { get; set; }
        public System.Int32 HitSFX { get; set; }
        public System.Single Speed { get; set; }
        public System.Single AngularSpeed { get; set; }
        public System.Single Gravity { get; set; }
        public System.Single FixedPointDuration { get; set; }
        public System.Single Duration { get; set; }
        public System.Boolean CollideUnit { get; set; }
        public System.Int32 MaxHitUnitCount { get; set; }
        public string[] CollideUnitEffects { get; set; }
        public System.Boolean CollideTerrain { get; set; }
        public string[] CollideTerrainEffects { get; set; }
    }
}
