namespace GOAP
{
    public class ActionBuilder
    {
        private readonly AgentAction agentAction;

        public ActionBuilder(string name)
        {
            agentAction = new AgentAction(name);
        }

        public ActionBuilder WithCost(float cost)
        {
            agentAction.SetCost(cost);
            return this;
        }

        public ActionBuilder WithActionStrategy(IActionStrategy strategy)
        {
            agentAction.SetActionStrategy(strategy);
            return this;
        }

        public ActionBuilder WithPrecondition(AgentBelief precondition)
        {
            agentAction.Precondition.Add(precondition);
            return this;
        }

        public ActionBuilder WithEffect(AgentBelief agentBelief)
        {
            agentAction.Effects.Add(agentBelief);
            return this;
        }

        public AgentAction BuildAgentAction() => agentAction;

    }
}