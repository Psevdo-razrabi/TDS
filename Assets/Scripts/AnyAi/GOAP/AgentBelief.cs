using System;
using UnityEngine;

namespace GOAP
{
    public class AgentBelief
    {
        public string Name { get; }

        public Func<bool> Conditions { get; private set; }
        public Func<Vector3> ObservedLocation { get; private set; }

        public Vector3 Location => ObservedLocation();
        
        public AgentBelief(string name)
        {
            Name = name;
        }

        public bool CheckCondition() => Conditions();

        public void SetCondition(Func<bool> condition) => Conditions = condition;
        public void SetObservedLocation(Func<Vector3> location) => ObservedLocation = location;
    }
}