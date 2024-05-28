using Koakuma.Game.Procedure;
using System.Threading.Tasks;
using TGame.UI;
using UnityEngine;

namespace Koakuma.Game.UI
{
    public class LoginUIMediator : UIMediator<LoginUIView>
    {
        protected override void OnInit(LoginUIView view)
        {
            base.OnInit(view);
        }

        protected override void OnShow(object arg)
        {
            base.OnShow(arg);
         
        }

        protected override void OnHide()
        {
          
            base.OnHide();
        }
    }
}
