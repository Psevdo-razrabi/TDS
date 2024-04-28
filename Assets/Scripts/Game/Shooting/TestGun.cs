using System;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    public event Action ShotFired;
    private void Update()
    {
        if (UnityEngine.Input.GetButtonDown("Fire1"))
        {
            ShotFired?.Invoke();
        }
    }
}
