using TGame.Procedure;
using System.Threading.Tasks;
using System.Diagnostics;
using Config;

namespace Koakuma.Game.Procedure
{
    public class LaunchProcedure : BaseProcedure
    {
        public override async Task OnEnterProcedure(object value)
        {
            UnityEngine.Debug.Log("1111111111111");
            await LoadConfigs();
            await ChangeProcedure<InitProcedure>();
        }
        private async Task LoadConfigs()
        {
            UnityEngine.Debug.Log("===>º”‘ÿ≈‰÷√");
            ConfigManager.LoadAllConfigsByAddressable("Assets/BundleAssets/Config");
            UnityEngine.Debug.Log("===>≈‰÷√º”‘ÿÕÍ±œ");
            await Task.Yield();
        }
    }
}
