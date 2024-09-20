using System;

namespace BlackboardScripts
{
    public interface IExpert
    {
        int GetInsistence(Blackboard blackboard);
        void Execute(Blackboard blackboard);
    }
}
