using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.Prefabs;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

public class Recoil : IConfigRelize, IInitializable
{
    private float _currentSpread;
    private float _baseRecoilForce;

    private readonly Crosshair _crosshair;
    private DistributionConfigs _distributionConfigs;
    private readonly CurrentWeapon _currentWeapon;
    private BaseWeaponConfig _gunConfig;
    private WeaponData _weaponData;

    public Recoil(Crosshair crosshair, WeaponConfigs weaponConfigs, DistributionConfigs distributionConfigs, WeaponData weaponData)
    {
        _crosshair = crosshair;
        _currentWeapon = new CurrentWeapon(weaponConfigs);
        _distributionConfigs = distributionConfigs;
        _weaponData = weaponData;
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
        _gunConfig = _currentWeapon.CurrentWeaponConfig;
        Vector3 forward = _weaponData.BulletPoint.forward;
        forward.Normalize();

        Vector3 perpendicular = Vector3.Cross(forward, Vector3.up);
        float sideRecoilStrength = Random.Range(-1f, 1f);
        Vector3 sideRecoil = perpendicular * sideRecoilStrength;

        float adjustedRecoilForce = _gunConfig.RecoilForce * Mathf.Lerp(0.5f, 1f, _currentSpread / _gunConfig.MaxSpread);
        Vector2 recoil = new Vector2(forward.x + sideRecoil.x, forward.z + sideRecoil.z) * adjustedRecoilForce;
        Debug.Log("рекоил = " + recoil);
        _crosshair.RecoilPlus(recoil);
    }

    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        _currentWeapon.LoadConfig(weaponComponent);
        _gunConfig = _currentWeapon.CurrentWeaponConfig;
    }
}
