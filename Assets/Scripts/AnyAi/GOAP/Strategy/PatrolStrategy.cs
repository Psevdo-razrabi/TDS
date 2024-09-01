using System;
using System.Collections.Generic;
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

        public PatrolStrategy(Transform[] points, NavMeshAgent agent, Transform entry, float duration)
        {
            _points = points;
            _agent = agent;
            _entry = entry;
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