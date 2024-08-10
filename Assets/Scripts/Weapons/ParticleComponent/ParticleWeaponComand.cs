using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons;
using Game.Player.Weapons.Commands;
using UnityEngine;

public class ParticleWeaponComand : Command
{
    private readonly InitializeWeaponParticle _weaponParticle;

    public ParticleWeaponComand(InitializeWeaponParticle weaponParticle)
    {
        _weaponParticle = weaponParticle;
    }
        
    public override async UniTask Execute(WeaponComponent weaponComponent)
    {
        _weaponParticle.ChangeParticleWeapon(weaponComponent);
        await UniTask.Yield();
    }
}
