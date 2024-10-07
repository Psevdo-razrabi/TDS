﻿using System;
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
        private readonly NavMeshAgent _agent;
        private readonly Transform _entry;
        private readonly GeneratorChunksRound _generatorChunks;
        private readonly float _duration;
        private bool _isPathCalculated;
        private CompositeDisposable _disposable;
        private NavGrid _navGrid;
        public bool CanPerform => !Complete;
        public bool Complete { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; } = new();

        public PatrolStrategy(BlackboardController blackboardController, float duration)
        {
            _generatorChunks = blackboardController.GetValue<GeneratorChunksRound>(NameAIKeys.GeneratorChunks);
            _agent = blackboardController.GetValue<NavMeshAgent>(NameAIKeys.Agent);
            _entry = blackboardController.GetValue<Transform>(NameAIKeys.TransformAI);
            _navGrid = blackboardController.GetValue<NavGrid>(NameAIKeys.NavGrid);
            _duration = duration;
        }

        public void Start()
        {
            Complete = false;
            _disposable = new CompositeDisposable();

            Observable.Timer(TimeSpan.FromSeconds(_duration)).Subscribe(_ => Complete = true).AddTo(_disposable);
        }

        public void Update(float deltaTime)
        {
            _generatorChunks.Update();

            if (_isPathCalculated && _agent.remainingDistance < 1f)
            {
                ChangePosition();
                _isPathCalculated = false;
            }

            if (_agent.pathPending == false) _isPathCalculated = true;
        }

        public void Stop()
        {
            _disposable.Clear();
            _disposable.Dispose();
        }

        private void ChangePosition()
        {
            var pointMap = _navGrid.GetPointMap();
            var point = pointMap.GetPointAtPosition(_entry.position + Vector3.up * 2);
            if(point != null)
            {
                _agent.destination = pointMap.FindFreePointAtPointRandomDist(point, 3, 10).Position;
            }
        }
    }
}