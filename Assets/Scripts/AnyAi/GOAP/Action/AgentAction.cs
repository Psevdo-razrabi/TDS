using System.Collections.Generic;
using Customs;
using Sirenix.Utilities;

namespace GOAP
{
    public class AgentAction
    {
        public string Name { get; private set; }
        public float Cost { get; private set; }

        public HashSet<AgentBelief> Precondition { get; } = new();
        public HashSet<AgentBelief> Effects { get; } = new();

        private IActionStrategy _actionStrategy;

        public AgentAction(string name)
        {
            Name = name;
        }

        public void SetCost(float cost)
        {
            Preconditions.CheckValidateData(cost);
            Cost = cost;
        }

        public void SetActionStrategy(IActionStrategy strategy)
        {
            Preconditions.CheckNotNull(strategy);
            _actionStrategy = strategy;
        }
        
        public bool Complete => _actionStrategy.Complete;

        public void Start() => _actionStrategy.Start();

        public void Update(float deltaTime)
        {
            if (_actionStrategy.CanPerform)
            {
                _actionStrategy.Update(deltaTime);
            }
            
            if(_actionStrategy.Complete == false) return;

            Effects.ForEach(effect => effect.CheckCondition());
        }

        public void Stop() => _actionStrategy.Stop();
    }
}