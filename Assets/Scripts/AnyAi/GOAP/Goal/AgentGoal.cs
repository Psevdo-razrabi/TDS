using System.Collections.Generic;
using Customs;

namespace GOAP
{
    public class AgentGoal
    {
        public string Name { get; }
        public float Priority { get; private set; }
        public HashSet<AgentBelief> DesiredEffects { get; } = new();

        public void SetPriority(float priority)
        {
            Preconditions.CheckValidateData(priority);
            Priority = priority;
        }
        
        public AgentGoal(string name)
        {
            Name = name;
        }
    }
}