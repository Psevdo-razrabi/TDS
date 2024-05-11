using System;

public class EventController
{
    public event Action ShotFired;
    public event Action SpreadReducing;
    public event Action ShotHited;

    public void ShotFire()
    {
        ShotFired?.Invoke();
    }
    
    public void SpreadReduce()
    {
        SpreadReducing?.Invoke();
    }

    public void ShotHit()
    {
        ShotHited?.Invoke();
    }
        
}
