using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using Game.Player.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using Weapons.InterfaceWeapon;
using Unity.VisualScripting;
using Game.Player.Weapons.Commands.Recievers;
using Game.AsyncWorker.Interfaces;

public class CurrentWeapon : IInitializable, IConfigRelize, IVisitWeaponType
{
    private readonly WeaponConfigs _weaponConfigs;
    private BaseWeaponConfig _gunConfig;
    private readonly CompositeDisposable _compositeDisposable = new();
    private IDisposable _reductionSubscription;
    private DistributionConfigs _distributionConfigs;
    private readonly IAwaiter _awaiter;

    public BaseWeaponConfig GunConfig => _gunConfig;

    private CurrentWeapon(WeaponConfigs weaponConfigs)
    {
        _weaponConfigs = weaponConfigs;
    }


    private async void LoadConfigs()
    {
        await _awaiter.AwaitLoadWeaponConfigs(_weaponConfigs);
    }

    public void Visit(Pistol pistol)
    {
        _gunConfig = _weaponConfigs.PistolConfig;
        Debug.Log("‡‡‡‡");
    }

    public void Visit(Rifle rifle)
    {
        _gunConfig = _weaponConfigs.RifleConfig;
        Debug.Log("‡‡‡1‡");
    }

    public void Visit(Shotgun shotgun)
    {
        _gunConfig = _weaponConfigs.ShotgunConfig;
        Debug.Log("‡‡‡‡2");
    }

    public void VisitWeapon(WeaponComponent component)
    {
        Visit((dynamic)component);
    }

    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        VisitWeapon(weaponComponent);
    }

    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
        LoadConfigs();
    }
}
