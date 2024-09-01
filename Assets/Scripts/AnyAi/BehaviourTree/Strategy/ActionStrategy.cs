using System;

namespace BehaviourTree
{
    public class ActionStrategy : IStrategy
    {
        public readonly Action action;

        public ActionStrategy(Action action)
        {
            this.action = action;
        }

        public BTNodeStatus Process()
        {
            action();
            return BTNodeStatus.Success;
        }
    }
}