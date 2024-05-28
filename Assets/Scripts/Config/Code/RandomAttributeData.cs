using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
namespace Config
{
    public partial struct RandomAttributeData
    {
        public System.Int32 AttributeType { get; set; }
        public System.Single[] AttributeInterval { get; set; }
    }
}
