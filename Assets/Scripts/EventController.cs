using System;
using Cysharp.Threading.Tasks;

public class EventController
{
    public event Action ShotFired;
    public event Action SpreadReducing;
    public event Action EnemyDie;
    public event Action EnemyHitBullet;

    public void ShotFire()
    {
        ShotFired?.Invoke();
    }
    
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
}
