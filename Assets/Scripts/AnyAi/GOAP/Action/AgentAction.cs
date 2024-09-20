using System;
using System.Collections.Generic;
using BehaviourTree;
using Customs;
using Sirenix.Utilities;

namespace GOAP
{
    public class AgentAction
    {
        public string Name { get; private set; }
        public float Cost { get; private set; }

        private BTNodeStatus _status;

        public HashSet<AgentBelief> Precondition { get; } = new();
        public HashSet<AgentBelief> Effects { get; } = new();

        public Func<bool> IsActionToBeUse { get; private set; }

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

        public void SetConditionActionWork(Func<bool> precondition)
        {
            IsActionToBeUse = precondition;
        }
        
        public bool Complete => _actionStrategy.Complete;
        public void Start() => _actionStrategy.Start();

        public BTNodeStatus Update(float deltaTime)
        { 
            if (_actionStrategy.CanPerform)
            { 
                _actionStrategy.Update(deltaTime);
                _status = BTNodeStatus.Running;
            }
            
            if(_actionStrategy.Complete == false) return _status = BTNodeStatus.Running;

            Effects.ForEach(effect => effect.CheckCondition());
            _status = BTNodeStatus.Success;
            return _status;
        }

        public void Stop() => _actionStrategy.Stop();
    }
}