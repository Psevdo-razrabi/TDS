using System;
using GOAP;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterOrEnemyEffect.Factory
{
    public class StrategyFactory
    {
        public IActionStrategy CreateIdleStrategy(float duration, Transform transform) =>
            new IdleStrategy(duration, transform);

        public IActionStrategy CreatePatrolStrategy(Transform[] points, NavMeshAgent agent, Transform entry,
            float duration) => new PatrolStrategy(points, agent, entry, duration);

        public IActionStrategy CreateMoveToPointStrategy(NavMeshAgent agent, Func<Vector3> destination) => new MoveStrategy(agent, destination);

        public IActionStrategy CreateAttackStrategy() => new AttackStrategy();
    }
}