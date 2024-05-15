using System;
using Cysharp.Threading.Tasks;

public class EventController
{
    public event Action ShotFired;
    public event Action SpreadReducing;
    public event Func<float, UniTaskVoid> ShootHited;
    public event Action BulletStoped;
    
    public void ShotFire()
    {
        ShotFired?.Invoke();
    }
    
    public void SpreadReduce()
    {
        SpreadReducing?.Invoke();
    }

    public void ShootHit()
    {
        ShootHited?.Invoke(0f);
        BulletStoped?.Invoke();
    }
        
}
