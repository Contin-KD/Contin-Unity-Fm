using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
namespace Config
{
    public partial struct SkillHitData
    {
        public System.Single Time { get; set; }
        public SkillDetectType DetectType { get; set; }
        public System.Single[] DetectArgs { get; set; }
        public SkillDetectCenterType DetectCenterType { get; set; }
        public System.Single CenterOffset { get; set; }
        public CharacterRelationType TargetRelation { get; set; }
        public string[] Effects { get; set; }
    }
}
