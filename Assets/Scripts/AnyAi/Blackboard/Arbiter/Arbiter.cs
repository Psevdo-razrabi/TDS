using System;
using System.Collections.Generic;
using Customs;

namespace BlackboardScripts
{
    public class Arbiter
    {
        private readonly HashSet<IExpert> _experts = new();

        public void Register(IExpert expert)
        {
            Preconditions.CheckNotNull(expert);
            _experts.Add(expert);
        }

        public void Remove(IExpert expert)
        {
            Preconditions.CheckNotNull(expert);
            _experts.Remove(expert);
        }

        public IReadOnlyList<Action> BlackboardIteration(Blackboard blackboard)
        {
            IExpert bestExpert = null;
            int highPriority = 0;

            foreach (var expert in _experts)
            {
                var priority = expert.GetInsistence(blackboard);

                if (priority > highPriority)
                {
                    highPriority = priority;
                    bestExpert = expert;
                }
            }

            bestExpert?.Execute(blackboard);

            var action = new List<Action>(blackboard.PassedActions);
            blackboard.ClearAction();

            return action;
        }
    }
}