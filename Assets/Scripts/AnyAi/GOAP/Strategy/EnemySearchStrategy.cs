using System;
using System.Threading;
using BlackboardScripts;
using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public class EnemySearchStrategy : IActionStrategy
    {
        public bool CanPerform => Complete == false;
        public bool Complete { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; } = null;
        
        private float _searchRadius;
        private float _timeToSearch;
        private int _countIteration;
        private CompositeDisposable _disposable = new();
        private Transform _unit;
        private NavMeshAgent _navMesh;
        private BlackboardController _blackboardController;

        public EnemySearchStrategy(BlackboardController blackboardController)
        {
            _searchRadius = blackboardController.GetValue<float>(NameAIKeys.SearchEnemyRadius);
            _timeToSearch = blackboardController.GetValue<float>(NameAIKeys.TimeToSearchEnemy);
            _unit = blackboardController.GetValue<Transform>(NameAIKeys.TransformAI);
            _navMesh = blackboardController.GetValue<NavMeshAgent>(NameAIKeys.Agent);
            _countIteration = blackboardController.GetValue<int>(NameAIKeys.CountIterationSearchEnemy);
            _blackboardController = blackboardController;
        }

        public void Start()
        {
            Complete = false;
            Observable
                .Timer(TimeSpan.FromSeconds(_timeToSearch / _countIteration),
                    TimeSpan.FromSeconds(_timeToSearch / _countIteration))
                .Subscribe(_ => EnemySearch()).AddTo(_disposable);

            Observable.Timer(TimeSpan.FromSeconds(_timeToSearch))
                .Subscribe(_ => Complete = true)
                .AddTo(_disposable);
        }

        private void EnemySearch()
        {
            var randomDirection = UnityEngine.Random.insideUnitSphere * _searchRadius;
            randomDirection += _unit.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _searchRadius, NavMesh.AllAreas))
            {
                _navMesh.destination = hit.position;
            }
        }

        public void Stop()
        {
            _disposable?.Dispose();
            _blackboardController.SetValue(NameAIKeys.EnemySearch, false);
        }
    }
}