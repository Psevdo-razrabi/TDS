using System;
using UnityEngine;

public class TestGun : MonoBehaviour
{
    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private CameraShakeConfig _cameraShakeConfig;
    [SerializeField] private GunConfig _gunConfig;

    public event Action<CameraShakeConfig,BulletConfig,GunConfig> ShotFired;
    private void Update()
    {
        if (UnityEngine.Input.GetButtonDown("Fire1"))
        {
            ShotFired?.Invoke(_cameraShakeConfig,_bulletConfig,_gunConfig);
        }
    }
}
