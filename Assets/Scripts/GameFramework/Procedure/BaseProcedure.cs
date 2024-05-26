using Koakuma.Game;
using System.Threading.Tasks;

namespace TGame.Procedure
{
    public abstract class BaseProcedure
    {
        public async Task ChangeProcedure<T>(object value = null) where T : BaseProcedure
        {
            await GameManager.Procedure.ChangeProcedure<T>(value);
        }

        public virtual async Task OnEnterProcedure(object value)
        {
            await Task.Yield();
        }

        public virtual async Task OnLeaveProcedure()
        {
            await Task.Yield();
        }
    }
}