using System;
using System.Collections.Generic;
using System.Threading;
using BlackboardScripts;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public class PatrolStrategy : IActionStrategy
    {
        private readonly Transform[] _points;
        private readonly NavMeshAgent _agent;
        private readonly Transform _entry;
        private readonly float _duration;
        private bool _isPathCalculated;
        private int currentIndex;
        private IDisposable _disposable;
        public bool CanPerform => !Complete;
        public bool Complete { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; } = null;

        public PatrolStrategy(BlackboardController blackboardController, float duration)
        {
            _points = blackboardController.GetValue<Transform[]>(NameAIKeys.PatrolPoints);
            _agent = blackboardController.GetValue<NavMeshAgent>(NameAIKeys.Agent);;
            _entry = blackboardController.GetValue<Transform>(NameAIKeys.TransformAI);
            _duration = duration;
        }

        public void Start()
        {
            Complete = false;

            _disposable = Observable.Timer(TimeSpan.FromSeconds(_duration)).Subscribe(_ => Complete = true);
        }

        public void Update(float deltaTime)
        {
            if (currentIndex == _points.Length)
            {
                currentIndex = 0;
            }
            
            var target = _points[currentIndex];

            _entry.LookAt(target);
            _agent.SetDestination(target.position);

            if (_isPathCalculated && _agent.remainingDistance < 1f)
            {
                currentIndex++;
                _isPathCalculated = false;
            }

            if (_agent.pathPending) _isPathCalculated = true;
        }

        public void Stop()
        {
            _disposable.Dispose();
        }
    }
}