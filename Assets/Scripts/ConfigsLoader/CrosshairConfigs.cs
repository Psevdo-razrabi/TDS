using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Zenject;

public class CrosshairConfigs
{

    public bool IsLoadPistolConfig { get; private set; }

    private const string NameLoadPistolConfig = "Pistol";
    private const string NameLoadRifleConfig = "Rifle";
    private const string NameLoadBulletConfig = "Bullet";
        
    private Loader _loader;
        
    [Inject]
    private async void Construct(Loader loader)
    {
        _loader = loader;

        await LoadConfigs();
    }
        
    private async UniTask LoadConfigs()
    {
        IsLoadPistolConfig = true;
    }
}
