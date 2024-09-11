using System;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    private float _time;
    [SerializeField] float _between = 0.05f;
    public event Action ShotFired;
    private void Update()
    {
        if (UnityEngine.Input.GetButton("Fire1"))
        {
            if (_time > _between)
            {
                ShotFired?.Invoke();
                _time = 0;
            }

            _time += UnityEngine.Time.deltaTime;

        }
    }
}
