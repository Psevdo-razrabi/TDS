using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Zenject;

public class Recoil
{
    private float _currentSpread;
    private float _baseRecoilForce;
    
    private RifleConfig _gunConfig;
    private BulletConfig _bulletConfig;
    private Crosshair _crosshair;
    private ChangeCrosshair _changeCrosshair;
    private WeaponConfigs _weaponConfigs;
    

    public Recoil(Crosshair crosshair, ChangeCrosshair changeCrosshair, WeaponConfigs weaponConfigs)
    {
        _crosshair = crosshair;
        _changeCrosshair = changeCrosshair;
        _weaponConfigs = weaponConfigs;
        LoadConfigs();
    }
    
    private async void LoadConfigs()
    {
        while (_weaponConfigs.IsLoadConfigs == false)
            await UniTask.Yield();
        
        _gunConfig = _weaponConfigs.RifleConfig;
        _baseRecoilForce = _gunConfig.RecoilForce;
    }
    public void UpdateSpread(float currentSpread)
    {
        _currentSpread = currentSpread;
    }
    
    public void RecoilCursor()
    {
        if (_gunConfig == null)
            return;

        Vector3 forward = _gunConfig.BulletPoint.transform.forward;
        forward.Normalize();
        
        Vector3 perpendicular = Vector3.Cross(forward, Vector3.up);
        float sideRecoilStrength = Random.Range(-1f, 1f);
        Vector3 sideRecoil = perpendicular * sideRecoilStrength;
        
        float adjustedRecoilForce = _baseRecoilForce * Mathf.Lerp(0.5f, 1f, _currentSpread / _gunConfig.MaxSpread);
        Vector2 recoil = new Vector2(forward.x + sideRecoil.x, forward.z + sideRecoil.z) * adjustedRecoilForce;

        _crosshair.RecoilPlus(recoil);
    }
    
}
