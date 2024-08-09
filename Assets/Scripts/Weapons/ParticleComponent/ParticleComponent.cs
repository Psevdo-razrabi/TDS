using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleComponent : IParticleWeapon
{
    private readonly ParticleStorage _storage;

    public ParticleComponent(ParticleStorage storage)
    {
        _storage = storage;
    }
    
    public void ShowParticle(ParticleGunType type, Transform position)
    {
        var particleWeapons = _storage.GetParticle.Where(pw => pw.ParticleGunType == type).ToList();
        
        foreach (var particleWeapon in particleWeapons)
        {
            ParticleSystem particleSystem = particleWeapon.ParticleSystem;
            ParticleSystem particleInstance = GameObject.Instantiate(particleSystem, position.position, position.rotation);
            particleInstance.Play();
        }
    }
}
