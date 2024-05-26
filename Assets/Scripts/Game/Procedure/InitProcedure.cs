using System.Threading.Tasks;
using TGame.Procedure;

namespace Koakuma.Game.Procedure
{
    public class InitProcedure : BaseProcedure
    {
        public override async Task OnEnterProcedure(object value)
        {
            await Task.Yield();
        }
    }
}
