using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace BehaviourTree
{
    public class PatrolStrategy : IStrategy
    {
        public readonly Transform entry;
        public readonly NavMeshAgent agent;
        public readonly List<Vector3> patrolPoints;
        public float speed;
        public int currentIndex;
        private bool _isPathCalculated;

        public PatrolStrategy(Transform entry, NavMeshAgent agent, List<Vector3> patrolPoints, float speed)
        {
            this.entry = entry;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.speed = speed;
        }


        public BTNodeStatus Process()
        {
            if (currentIndex == patrolPoints.Count) return BTNodeStatus.Success;

            var target = patrolPoints[currentIndex];

            agent.SetDestination(target);
            entry.LookAt(target);

            if (_isPathCalculated && agent.remainingDistance < 0.1f)
            {
                currentIndex++;
                _isPathCalculated = false;
            }

            if (agent.pathPending) _isPathCalculated = true;

            return BTNodeStatus.Running;
        }

        public void Reset() => currentIndex = 0;
    }
}