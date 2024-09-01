namespace BlackboardScripts
{
    public interface IExpert
    {
        int GetBlackboard(Blackboard blackboard);
        void Execute(Blackboard blackboard);
    }
}
