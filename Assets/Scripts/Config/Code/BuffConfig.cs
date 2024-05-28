using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
namespace Config
{
    public partial struct BuffConfig
    {
        public static void DeserializeByAddressable(string directory)
        {
            string path = $"{directory}/BuffConfig.json";
            UnityEngine.TextAsset ta = Addressables.LoadAssetAsync<UnityEngine.TextAsset>(path).WaitForCompletion();
            string json = ta.text;
            datas = new List<BuffConfig>();
            indexMap = new Dictionary<int, int>();
            JArray array = JArray.Parse(json);
            Count = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                JObject dataObject = array[i] as JObject;
                BuffConfig data = (BuffConfig)dataObject.ToObject(typeof(BuffConfig));
                datas.Add(data);
                indexMap.Add(data.ID, i);
            }
        }
        public static void DeserializeByFile(string directory)
        {
            string path = $"{directory}/BuffConfig.json";
            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fs))
                {
                    datas = new List<BuffConfig>();
                    indexMap = new Dictionary<int, int>();
                    string json = reader.ReadToEnd();
                    JArray array = JArray.Parse(json);
                    Count = array.Count;
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject dataObject = array[i] as JObject;
                        BuffConfig data = (BuffConfig)dataObject.ToObject(typeof(BuffConfig));
                        datas.Add(data);
                        indexMap.Add(data.ID, i);
                    }
                }
            }
        }
        public static System.Collections.IEnumerator DeserializeByBundle(string directory, string subFolder)
        {
            string bundleName = $"{subFolder}/BuffConfig.bytes".ToLower();
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
            datas = new List<BuffConfig>();
            indexMap = new Dictionary<int, int>();
            JArray array = JArray.Parse(json);
            Count = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                JObject dataObject = array[i] as JObject;
                BuffConfig data = (BuffConfig)dataObject.ToObject(typeof(BuffConfig));
                datas.Add(data);
                indexMap.Add(data.ID, i);
            }
        }
        public static int Count;
        private static List<BuffConfig> datas;
        private static Dictionary<int, int> indexMap;
        public static BuffConfig ByID(int id)
        {
            if (id <= 0)
            {
                return Null;
            }
            if (!indexMap.TryGetValue(id, out int index))
            {
                throw new System.Exception($"BuffConfig找不到ID:{id}");
            }
            return ByIndex(index);
        }
        public static BuffConfig ByIndex(int index)
        {
            return datas[index];
        }
        public bool IsNull { get; private set; }
        public static BuffConfig Null { get; } = new BuffConfig() { IsNull = true }; 
        public System.Int32 ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public System.Boolean IsDebuff { get; set; }
        public System.Int32[] ImmuneBuffs { get; set; }
        public System.Int32 MaxStack { get; set; }
        public BuffSameStackActionType SameStackAction { get; set; }
        public System.Single Duration { get; set; }
        public BuffTriggerType TriggerType { get; set; }
        public System.Double[] TriggerValue { get; set; }
        public CharacterState TriggerTargetState { get; set; }
        public System.Single[] TriggerProbability { get; set; }
        public System.Single[] TriggerInterval { get; set; }
        public System.Int32 TriggerCount { get; set; }
        public System.Boolean RemoveWhenReachTriggerCount { get; set; }
        public System.Int32[] FX { get; set; }
        public System.Int32[] TriggerFX { get; set; }
        public System.Int32 AddSFX { get; set; }
        public System.Int32 RemoveSFX { get; set; }
        public BuffHitData[] TriggerHits { get; set; }
    }
}
