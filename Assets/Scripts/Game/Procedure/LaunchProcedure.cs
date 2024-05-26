using TGame.Procedure;
using System.Threading.Tasks;

namespace Koakuma.Game.Procedure
{
    public class LaunchProcedure : BaseProcedure
    {
        public override async Task OnEnterProcedure(object value)
        {
            await Task.Yield();
        }
    }
}
