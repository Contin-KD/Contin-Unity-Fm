using System.Threading.Tasks;
using TGame.Procedure;
using UnityEngine;

namespace Koakuma.Game.Procedure
{
    public class InitProcedure : BaseProcedure
    {
        public override async Task OnEnterProcedure(object value)
        {

            MonoBehaviour monoBehaviour = GameObject.Find("GameManager").GetComponent<GameManager>() as MonoBehaviour;
            monoBehaviour.StartCoroutine(GameManager.UI.OpenUIAsync(UI.UIViewID.LoginUI));
            UnityEngine.Debug.Log("22222");
            await Task.Yield();
        }
    }
}