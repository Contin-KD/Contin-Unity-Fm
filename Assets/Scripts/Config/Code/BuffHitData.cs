using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
namespace Config
{
    public partial struct BuffHitData
    {
        public SkillDetectType DetectType { get; set; }
        public System.Single[] DetectArgs { get; set; }
        public CharacterRelationType TargetRelation { get; set; }
        public string Effect { get; set; }
    }
}
