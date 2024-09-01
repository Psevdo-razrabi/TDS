using System;
using UnityEngine;

namespace GOAP
{
    public class BeliefBuilder
    {
        public readonly AgentBelief _agentBelief;

        public BeliefBuilder(string name)
        {
            _agentBelief = new AgentBelief(name);
        }

        public BeliefBuilder WithCondition(Func<bool> condition)
        {
            _agentBelief.SetCondition(condition);
            return this;
        }
        
        public BeliefBuilder WithLocation(Func<Vector3> location)
        {
            _agentBelief.SetObservedLocation(location);
            return this;
        }

        public AgentBelief BuildBelief()
        {
            if (_agentBelief.Conditions == null) WithCondition(() => false);
            if (_agentBelief.ObservedLocation == null) WithLocation(() => Vector3.zero);

            return _agentBelief;
        }
    }
}