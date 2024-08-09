using System;
using Cysharp.Threading.Tasks;

public class EventController
{
    public event Action EnemyDie;
    public event Action EnemyHitBullet;
    

    public void OnEnemyDie()
    {
        EnemyDie?.Invoke();
    }

    public void OnEnemyHitBullet()
    {
        EnemyHitBullet?.Invoke();
    }
}
