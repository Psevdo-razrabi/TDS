using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

public class Recoil : IConfigRelize, IVisitWeaponType, IInitializable
{
    private float _currentSpread;
    private float _baseRecoilForce;
    
    private readonly Crosshair _crosshair;
    private readonly WeaponConfigs _weaponConfigs;
    private DistributionConfigs _distributionConfigs;
    private BaseWeaponConfig _weaponConfig;
    
    public Recoil(Crosshair crosshair, WeaponConfigs weaponConfigs, DistributionConfigs distributionConfigs)
    {
        _crosshair = crosshair;
        _weaponConfigs = weaponConfigs;
        _distributionConfigs = distributionConfigs;
    }
    
    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
    }
    
    public void UpdateSpread(float currentSpread)
    {
        _currentSpread = currentSpread;
    }
    
    public void RecoilCursor()
    {
        Vector3 forward = _weaponConfig.BulletPoint.transform.forward;
        forward.Normalize();
        
        Vector3 perpendicular = Vector3.Cross(forward, Vector3.up);
        float sideRecoilStrength = Random.Range(-1f, 1f);
        Vector3 sideRecoil = perpendicular * sideRecoilStrength;
        
        float adjustedRecoilForce = _baseRecoilForce * Mathf.Lerp(0.5f, 1f, _currentSpread / _weaponConfig.MaxSpread);
        Vector2 recoil = new Vector2(forward.x + sideRecoil.x, forward.z + sideRecoil.z) * adjustedRecoilForce;

        _crosshair.RecoilPlus(recoil);
    }

    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        VisitWeapon(weaponComponent);
    }

    public void Visit(Pistol pistol)
    {
        _weaponConfig = _weaponConfigs.PistolConfig;
    }

    public void Visit(Rifle rifle)
    {
        _weaponConfig = _weaponConfigs.RifleConfig;
    }

    public void Visit(Shotgun shotgun)
    {
        _weaponConfig = _weaponConfigs.ShotgunConfig;
    }

    public void VisitWeapon(WeaponComponent component)
    {
        Visit((dynamic)component);
    }
}
