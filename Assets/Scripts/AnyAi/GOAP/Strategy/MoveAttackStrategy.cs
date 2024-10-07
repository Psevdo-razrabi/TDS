using System.Threading;
using BlackboardScripts;
using Game.Player.PlayerStateMashine;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public class MoveAttackStrategy : IActionStrategy
    {
        public bool CanPerform => !Complete;
        public bool Complete => (_agent.remainingDistance <= 1f && _agent.pathPending == false) || _commander.IsTargetAttack == false;
        public CancellationTokenSource CancellationTokenSource { get; } = null;

        private NavMeshAgent _agent;
        private CommanderAIGroup _commander;
        private Transform _playerTransform;
        private BlackboardController _blackboardController;

        public MoveAttackStrategy(BlackboardController blackboardController)
        {
            _agent = blackboardController.GetValue<NavMeshAgent>(NameAIKeys.Agent);
            _commander = blackboardController.GetValue<CommanderAIGroup>(NameExperts.CommanderExpert);
            _playerTransform = blackboardController.GetValue<Transform>(NameAIKeys.PlayerTarget);
            _blackboardController = blackboardController;
        }

        public void Start() => _agent.destination = _playerTransform.position;

        public void Update(float timeDelta)
        {
            _agent.destination = _playerTransform.position;
        }

        public void Stop()
        {
            _agent.ResetPath();
            if(_commander.IsTargetAttack == false) _blackboardController.SetValue(NameAIKeys.EnemySearch, true);
        }
    }
}