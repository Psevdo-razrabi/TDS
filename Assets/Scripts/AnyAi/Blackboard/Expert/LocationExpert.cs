using System;

namespace BlackboardScripts.Expert
{
    public class LocationExpert : IExpert
    {
        private readonly BlackboardController _blackboardController;
        private readonly BlackboardKey _locationKey;
        private readonly BlackboardKey _locationPredicateKey;
        private Action _action;

        public LocationExpert(BlackboardKey locationKey, BlackboardKey locationPredicateKey,
            BlackboardController blackboardController)
        {
            _blackboardController = blackboardController;
            _locationKey = locationKey;
            _locationPredicateKey = locationPredicateKey;
        }

        public int GetInsistence(Blackboard blackboard)
        {
            if (_blackboardController.GetBlackboard().TryGetValue<Func<bool>>(_locationPredicateKey, out var value))
            {
                return value() ? 2 : 0;
            }

            return 0;
        }

        public void Execute(Blackboard blackboard)
        {
            _action = null;
            _action = () =>
            {
                if (blackboard.TryGetValue(_locationPredicateKey, out Func<bool> predicate))
                {
                    blackboard.SetValue(_locationKey, predicate());
                }
            };
            blackboard.AddActions(_action);
        }
    }
}