namespace UnityEngine
{
    public class WaitForCondition : CustomYieldInstruction
    {
        public override bool keepWaiting
        {
            get { return condition != null && !condition(); }
        }

        private System.Func<bool> condition;

        public WaitForCondition(System.Func<bool> condition)
        {
            this.condition = condition;
        }
    }
}