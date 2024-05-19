using System;
using Cysharp.Threading.Tasks;

public class EventController
{
    public event Action ShotFired;
    public event Action SpreadReducing;
    public event Action EnemyDie;
    public event Action EnemyHitBullet;
    public event Action BulletStoped;
    
    public void SpreadReduce()
    {
        SpreadReducing?.Invoke();
    }

    public void OnEnemyDie()
    {
        EnemyDie?.Invoke();
    }

    public void OnEnemyHitBullet()
    {
        EnemyHitBullet?.Invoke();
    }

    public void BulletHit()
    {
        BulletStoped?.Invoke();
    }
}
