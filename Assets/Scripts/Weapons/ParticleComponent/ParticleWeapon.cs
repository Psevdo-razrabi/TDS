using System;
using UnityEngine;

[Serializable]
public class ParticleWeapon 
{
    [field: SerializeField] public  ParticleGunType ParticleGunType { get; private set; }
    [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
}
