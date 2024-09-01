using System;

namespace BehaviourTree
{ 
    public class Conditions : IStrategy
    {
        public readonly Func<bool> predicate;

        public Conditions(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public BTNodeStatus Process() => predicate() ? BTNodeStatus.Success : BTNodeStatus.Failure;
    }
}