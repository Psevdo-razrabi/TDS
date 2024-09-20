using System;
using Game.Player.PlayerStateMashine;

namespace BlackboardScripts.Expert
{
    public class MovementExpert : IExpert
    {
        private readonly BlackboardController _blackboardController;
        private BlackboardKey _movementKey;
        private Action _action;

        public MovementExpert(BlackboardKey blackboardKey, BlackboardController blackboardController)
        {
            _movementKey = blackboardKey;
            _blackboardController = blackboardController;
        }

        public int GetInsistence(Blackboard blackboard)
        {
            return _blackboardController.GetValue<Func<bool>>(NameExperts.MovementPredicate)() ? 1 : 0;
        }

        public void Execute(Blackboard blackboard)
        {
            _action = null;
            _action = () =>
            {
                if (blackboard.TryGetValue(_movementKey, out Func<bool> predicate))
                {
                    _blackboardController.SetValue(NameExperts.Movement, predicate());
                }
            };
            blackboard.AddActions(_action);
        }
    }
}