using Cysharp.Threading.Tasks;
using Game.AsyncWorker.Interfaces;
using UnityEngine;
using Zenject;

public class CameraShakeConfigs : ILoadable
{
    public CameraShakeConfig PistolShakeConfig { get; private set; }
    public CameraShakeConfig RifleShakeConfig { get; private set; }
    public CameraShakeConfig ShotGunShakeConfig { get; private set; }
    public bool IsLoaded { get; private set; }

    private const string NameLoadPistolShakeConfig = "PistolShake";
    private const string NameLoadRifleShakeConfig = "RifleShake";
    private const string NameLoadShotgunShakeConfig = "ShotgunShake";
        
    private Loader _loader;
        
    [Inject]
    private async void Construct(Loader loader)
    {
        _loader = loader;
        await LoadConfigs();
    }
        
    private async UniTask LoadConfigs()
    {
        PistolShakeConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadPistolShakeConfig)).resources as CameraShakeConfig;
        RifleShakeConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadRifleShakeConfig)).resources as CameraShakeConfig;
        ShotGunShakeConfig = (await _loader.LoadResources<ScriptableObject>(NameLoadShotgunShakeConfig)).resources as CameraShakeConfig;
        
        IsLoaded = true;
    }
}
