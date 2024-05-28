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
using Input;
using Zenject;

public class CurrentWeapon : IVisitWeaponType
{
    private readonly WeaponConfigs _weaponConfigs;
    
    private InputSystemMouse _inputSystemMouse;
    private BaseWeaponConfig _weaponConfig;
    private BaseWeaponConfig _aimWeaponConfig;
    
    private bool _isAiming;

    public CurrentWeapon(WeaponConfigs weaponConfigs)
    {
        _weaponConfigs = weaponConfigs;
    }
    
    [Inject]
    public void Construct(InputSystemMouse systemMouse) // аим пока нихуя не раболтает, поэтому даю тока дефолт конфиг
    {
        if (_inputSystemMouse != null)
            return;
        _inputSystemMouse = systemMouse;
        _inputSystemMouse.RightMouseButtonDown += OnRightMouseButtonDown;
        _inputSystemMouse.RightMouseButtonUp += OnRightMouseButtonUp;
    }
    
    public BaseWeaponConfig CurrentWeaponConfig => _weaponConfig;

    public void LoadConfig(WeaponComponent weaponComponent)
    {
        VisitWeapon(weaponComponent);
    }

    public void VisitWeapon(WeaponComponent component)
    {
        Visit((dynamic)component);
    }

    public void Visit(Pistol pistol)
    {
        _weaponConfig = _weaponConfigs.PistolConfig;
        _aimWeaponConfig = _weaponConfigs.PistolAimConfig;
    }

    public void Visit(Rifle rifle)
    {
        _weaponConfig = _weaponConfigs.RifleConfig;
        _aimWeaponConfig = _weaponConfigs.RifleAimConfig;
    }

    public void Visit(Shotgun shotgun)
    {
        _weaponConfig = _weaponConfigs.ShotgunConfig;
        _aimWeaponConfig = _weaponConfigs.ShotgunAimConfig;
    }
    
    private void OnRightMouseButtonDown()
    {
        _isAiming = true;
        Debug.Log("ИЗ АИМИНГ");
    }

    private void OnRightMouseButtonUp()
    {
        _isAiming = false;
            Debug.Log("ХУЙ ТАМ ЛОЛ");
    }
}