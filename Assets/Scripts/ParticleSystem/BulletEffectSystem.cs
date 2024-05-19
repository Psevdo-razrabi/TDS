using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectSystem
{
    private ParticleSystem _particleSystem;

    public void StartParticleSystem(ParticleSystem particleSystem, Vector3 position, bool isHealthObject)
    {
        particleSystem.transform.position = position;
        particleSystem.Play();
    }
}
