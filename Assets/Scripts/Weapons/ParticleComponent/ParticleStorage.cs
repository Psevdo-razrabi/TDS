using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Player.Weapons;
using Game.Player.Weapons.AudioWeapon;
using Game.Player.Weapons.WeaponClass;
using UnityEngine;
using Weapons.InterfaceWeapon;

public class ParticleStorage : IVisitWeaponType
{
    private Dictionary<WeaponComponent, List<ParticleWeapon>> _particle = new();
    private WeaponParticle _weaponParticle;
    public IReadOnlyList<ParticleWeapon> GetParticle { get; private set; }

    public ParticleStorage(WeaponParticle weaponParticle)
    {
        _weaponParticle = weaponParticle;
    }
        
    public void AddOrUpdateWeaponParticle(WeaponComponent component)
    {
        if(CheckContainsKey(component)) return;
        
        component.Accept(this);
    }
    
    public void Visit(Pistol pistol)
    {
        _particle.Add(pistol, _weaponParticle.ParticleWeapon.Where(x => x.ParticleGunType == ParticleGunType.Pistol).ToList());
        
        GetParticle = _particle[pistol];
    }

    public void Visit(Rifle rifle)
    {
      _particle.Add(rifle, _weaponParticle.ParticleWeapon.Where(x => x.ParticleGunType == ParticleGunType.Rifle).ToList());
    }

    public void Visit(Shotgun shotgun)
    {
        _particle.Add(shotgun, _weaponParticle.ParticleWeapon.Where(x => x.ParticleGunType == ParticleGunType.Shotgun).ToList());
    }
    private bool CheckContainsKey(WeaponComponent component)
    {
        var containsKey = _particle.ContainsKey(component);
        if (containsKey) GetParticle = _particle[component];
        return containsKey;
    }
}

