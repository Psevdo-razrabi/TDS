using System.Collections;
using System.Collections.Generic;
using Game.Player.Weapons;
using UnityEngine;

public class InitializeWeaponParticle 
{
    private readonly ParticleStorage _storage;

    public InitializeWeaponParticle(ParticleStorage storage)
    {
        _storage = storage;
    }

    public void ChangeParticleWeapon(WeaponComponent weaponComponent)
    {
        _storage.AddOrUpdateWeaponParticle(weaponComponent);
    }
}
