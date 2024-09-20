using System;

namespace BlackboardScripts.Expert
{
    public class StatsExpert : IExpert
    {
        private readonly BlackboardController _blackboardController;
        private readonly BlackboardKey _statsKey;
        private readonly BlackboardKey _statsPredicateKey;
        private Action _action;
        private int _priority;
        private readonly Func<float, bool> _precondition;

        public StatsExpert(BlackboardKey statsKey, BlackboardKey statsPredicateKey, BlackboardController blackboardController, int priority, Func<float, bool> precondition)
        {
            _blackboardController = blackboardController;
            _statsKey = statsKey;
            _statsPredicateKey = statsPredicateKey;
            _priority = priority;
            _precondition = precondition;
        }

        public int GetInsistence(Blackboard blackboard)
        {
            if (_blackboardController.GetBlackboard().TryGetValue<Func<float>>(_statsPredicateKey, out var value))
            {
                return _precondition(value()) ? _priority : 0;
            }

            return 0;
        }

        public void Execute(Blackboard blackboard)
        {
            _action = null;
            _action = () =>
            {
                if (blackboard.TryGetValue(_statsPredicateKey, out Func<float> predicate))
                {
                    blackboard.SetValue(_statsKey, predicate());
                }
            };
            blackboard.AddActions(_action);
        }
    }
}