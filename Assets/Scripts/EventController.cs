using System;
<<<<<<< Updated upstream
=======
using Cysharp.Threading.Tasks;
>>>>>>> Stashed changes

public class EventController
{
    public event Action ShotFired;
    public event Action SpreadReducing;
<<<<<<< Updated upstream
    public event Action ShotHited;
=======
    public event Func<float, UniTaskVoid> ShootHited;
>>>>>>> Stashed changes

    public void ShotFire()
    {
        ShotFired?.Invoke();
    }
    
    public void SpreadReduce()
    {
        SpreadReducing?.Invoke();
    }

<<<<<<< Updated upstream
    public void ShotHit()
    {
        ShotHited?.Invoke();
=======
    public void ShootHit()
    {
        ShootHited?.Invoke(0f);
>>>>>>> Stashed changes
    }
        
}
