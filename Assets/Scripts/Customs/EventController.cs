using System;
using Cysharp.Threading.Tasks;

namespace Customs
{
    public class EventController
    {
        public event Func<float,UniTaskVoid> ShootHit;
        public event Action DiePlayer;
        public event Action DieEnemy;

        public void ShootHitEventInvoke() => ShootHit?.Invoke(0f);

        public void EnemyDie() => DieEnemy?.Invoke();
        public void PlayerDie() => DiePlayer?.Invoke();
    }
}