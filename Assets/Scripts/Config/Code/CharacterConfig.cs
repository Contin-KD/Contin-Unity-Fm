using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
namespace Config
{
    public partial struct CharacterConfig
    {
        public static void DeserializeByAddressable(string directory)
        {
            string path = $"{directory}/CharacterConfig.json";
            UnityEngine.TextAsset ta = Addressables.LoadAssetAsync<UnityEngine.TextAsset>(path).WaitForCompletion();
            string json = ta.text;
            datas = new List<CharacterConfig>();
            indexMap = new Dictionary<int, int>();
            JArray array = JArray.Parse(json);
            Count = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                JObject dataObject = array[i] as JObject;
                CharacterConfig data = (CharacterConfig)dataObject.ToObject(typeof(CharacterConfig));
                datas.Add(data);
                indexMap.Add(data.ID, i);
            }
        }
        public static void DeserializeByFile(string directory)
        {
            string path = $"{directory}/CharacterConfig.json";
            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fs))
                {
                    datas = new List<CharacterConfig>();
                    indexMap = new Dictionary<int, int>();
                    string json = reader.ReadToEnd();
                    JArray array = JArray.Parse(json);
                    Count = array.Count;
                    for (int i = 0; i < array.Count; i++)
                    {
                        JObject dataObject = array[i] as JObject;
                        CharacterConfig data = (CharacterConfig)dataObject.ToObject(typeof(CharacterConfig));
                        datas.Add(data);
                        indexMap.Add(data.ID, i);
                    }
                }
            }
        }
        public static System.Collections.IEnumerator DeserializeByBundle(string directory, string subFolder)
        {
            string bundleName = $"{subFolder}/CharacterConfig.bytes".ToLower();
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
            datas = new List<CharacterConfig>();
            indexMap = new Dictionary<int, int>();
            JArray array = JArray.Parse(json);
            Count = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                JObject dataObject = array[i] as JObject;
                CharacterConfig data = (CharacterConfig)dataObject.ToObject(typeof(CharacterConfig));
                datas.Add(data);
                indexMap.Add(data.ID, i);
            }
        }
        public static int Count;
        private static List<CharacterConfig> datas;
        private static Dictionary<int, int> indexMap;
        public static CharacterConfig ByID(int id)
        {
            if (id <= 0)
            {
                return Null;
            }
            if (!indexMap.TryGetValue(id, out int index))
            {
                throw new System.Exception($"CharacterConfig找不到ID:{id}");
            }
            return ByIndex(index);
        }
        public static CharacterConfig ByIndex(int index)
        {
            return datas[index];
        }
        public bool IsNull { get; private set; }
        public static CharacterConfig Null { get; } = new CharacterConfig() { IsNull = true }; 
        public System.Int32 ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public MonsterType MonsterType { get; set; }
        public string Model { get; set; }
        public string Animator { get; set; }
        public string ModelHQ { get; set; }
        public string AnimatorHQ { get; set; }
        public string AI { get; set; }
        public System.Int32 GetHitFX { get; set; }
        public System.Int32 StepSFX { get; set; }
        public System.Int32 DeadSFX { get; set; }
        public System.Int32[] Skills { get; set; }
        public System.Int32[] Buffs { get; set; }
        public System.Int32 AppearSFX { get; set; }
        public System.Boolean NonTarget { get; set; }
        public System.Boolean WanderIfNoTarget { get; set; }
        public System.Single ModelScale { get; set; }
        public System.Single Size { get; set; }
        public System.Boolean ImmuneAttackStiff { get; set; }
        public System.Single ThinkMin { get; set; }
        public System.Single ThinkMax { get; set; }
        public System.Single SightRange { get; set; }
        public MainAttribute MainAttribute { get; set; }
        public System.Int32 Strength { get; set; }
        public System.Int32 Intelligence { get; set; }
        public System.Int32 Dexterity { get; set; }
        public System.Int32 Vitality { get; set; }
        public System.Int32 StrGrow { get; set; }
        public System.Int32 IntGrow { get; set; }
        public System.Int32 DexGrow { get; set; }
        public System.Int32 VitGrow { get; set; }
        public System.Int32 Life { get; set; }
        public System.Int32 Fury { get; set; }
        public System.Single Damage { get; set; }
        public System.Single Armor { get; set; }
        public System.Single PhysicalResistance { get; set; }
        public System.Single ColdResistance { get; set; }
        public System.Single FireResistance { get; set; }
        public System.Single LightningResistance { get; set; }
        public System.Single PoisonResistance { get; set; }
        public System.Single CriticalHit { get; set; }
        public System.Single CriticalHitDamage { get; set; }
        public System.Single Evasion { get; set; }
        public System.Single Block { get; set; }
        public System.Single MS { get; set; }
        public System.Int32[] ItemRewardGroup { get; set; }
        public System.Int32 MonsterExperience { get; set; }
    }
}
