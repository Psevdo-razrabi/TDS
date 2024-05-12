using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Zenject;

public class Recoil
{
    private float _currentSpread;
    private float _stepSpread;
    
    private RifleConfig _gunConfig;
    private BulletConfig _bulletConfig;
    private Crosshair _crosshair;
    private ChangeCrosshair _changeCrosshair;
    private WeaponConfigs _weaponConfigs;
    

    public Recoil(Crosshair crosshair, ChangeCrosshair changeCrosshair, WeaponConfigs weaponConfigs)
    {
        Debug.Log("не отъебнуло");
        _crosshair = crosshair;
        _changeCrosshair = changeCrosshair;
        _weaponConfigs = weaponConfigs;
        LoadConfigs();
    }
    
    private async void LoadConfigs()
    {
        while (_weaponConfigs.IsLoadConfigs == false)
            await UniTask.Yield();
        
        _gunConfig = _weaponConfigs.PistolConfig;
    }
    public void RecoilCursor()
    {
        Vector3 forward = _gunConfig.BulletPoint.transform.forward;
        
        forward.Normalize();
        
        Vector3 perpendicular = Vector3.Cross(forward, Vector3.up);
        float sideRecoilStrength = Random.Range(-1f, 1f);
        Vector3 sideRecoil = perpendicular * sideRecoilStrength;
        Vector2 recoil = new Vector2(forward.x + sideRecoil.x, forward.z + sideRecoil.z) * _gunConfig.RecoilForce;

        _crosshair.RecoilPlus(recoil);
    }
    
}
