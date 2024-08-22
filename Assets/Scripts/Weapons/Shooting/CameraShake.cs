using DG.Tweening;
using Game.Player.Weapons;
using Game.Player.Weapons.Commands.Recievers;
using Game.Player.Weapons.WeaponClass;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Weapons.InterfaceWeapon;
using Zenject;

public class CameraShake : IConfigRelize, IVisitWeaponType, IInitializable
{ 
    private CameraShakeConfigs _cameraShake;
    private CameraShakeConfig _shakeConfig;
    private ICameraProvider _cameraProvider;
    private DistributionConfigs _distributionConfigs;

    private Vector3 _shakeOffset = Vector3.zero;
    private bool _isShaking = false;

    public CameraShake(CameraShakeConfigs shake, ICameraProvider cameraProvider, DistributionConfigs distributionConfigs)
    {
        _cameraShake = shake;
        _shakeConfig = _cameraShake.RifleShakeConfig;
        _cameraProvider = cameraProvider;
        _distributionConfigs = distributionConfigs;
    }
    
    public void Initialize()
    {
        _distributionConfigs.ClassesWantConfig.Add(this);
    }

    public void ShakeCamera()
    {
        if (_isShaking) return;
        
        _isShaking = true;

        _cameraProvider.CameraTransform.DOKill(complete: true);

        _cameraProvider.CameraTransform
            .DOShakePosition(_shakeConfig.ShakeDuration, _shakeConfig.ShakeStrength, 5, 90f, false, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InOutBounce)
            .OnUpdate(() =>
            {
                Vector3 currentNoShakePosition = _cameraProvider.CameraTransform.position - _shakeOffset;
                _shakeOffset = _cameraProvider.CameraTransform.position - currentNoShakePosition;
                _cameraProvider.CameraTransform.position = currentNoShakePosition;
            })
            .OnComplete(() =>
            {
                _shakeOffset = Vector3.zero;
                _isShaking = false;
            });
    }

    public void GetWeaponConfig(WeaponComponent weaponComponent)
    {
        weaponComponent.Accept(this);
    }

    public void Visit(Pistol pistol)
    {
        _shakeConfig = _cameraShake.PistolShakeConfig;
    }

    public void Visit(Rifle rifle)
    {
        _shakeConfig = _cameraShake.RifleShakeConfig;
    }

    public void Visit(Shotgun shotgun)
    {
        _shakeConfig = _cameraShake.ShotGunShakeConfig;
    }
}
