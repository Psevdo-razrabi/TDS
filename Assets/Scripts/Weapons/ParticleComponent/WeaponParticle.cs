using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParticle : MonoBehaviour
{
    [field: SerializeField] private List<ParticleWeapon> _particleWeapons;

    public IReadOnlyList<ParticleWeapon> ParticleWeapon => _particleWeapons;
}
