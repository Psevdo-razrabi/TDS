﻿using System;
using System.Threading;
using BlackboardScripts;
using Game.Player.PlayerStateMashine;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    public class MoveStrategy : IActionStrategy
    {
        public bool CanPerform => !Complete;
        public bool Complete => _agent.remainingDistance <= 2f && !_agent.pathPending;
        public CancellationTokenSource CancellationTokenSource { get; private set; } = null;
        
        private readonly NavMeshAgent _agent;
        private readonly Func<Vector3> _destination;
        private readonly bool _isUpdate;
        private readonly Transform _player;

        public MoveStrategy(BlackboardController blackboardController, Func<Vector3> destination, bool isUpdate)
        {
            _agent = blackboardController.GetValue<NavMeshAgent>(NameAIKeys.Agent);
            _player = blackboardController.GetValue<Transform>(NameAIKeys.PlayerTarget);
            _destination = destination;
            _isUpdate = isUpdate;
        }

        public void Start() => _agent.SetDestination(_destination());
        public void Stop() => _agent.ResetPath();
        public void Update(float deltaTime)
        {
            if(_isUpdate == false) return;
            _agent.SetDestination(_player.transform.position);
        }
    }
}