using System;
using BlackboardScripts;
using GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterOrEnemyEffect.Factory
{
    public class StrategyFactory
    {
        public IActionStrategy CreateIdleStrategy(float duration, BlackboardController blackboardController) =>
            new IdleStrategy(duration, blackboardController);

        public IActionStrategy CreatePatrolStrategy(BlackboardController blackboardController, float duration)
            => new PatrolStrategy(blackboardController, duration);

        public IActionStrategy CreateMoveToPointStrategy(BlackboardController blackboardController, Func<Vector3> destination) 
            => new MoveStrategy(blackboardController, destination);

        public IActionStrategy CreateAttackStrategy() => new AttackStrategy();
    }
}