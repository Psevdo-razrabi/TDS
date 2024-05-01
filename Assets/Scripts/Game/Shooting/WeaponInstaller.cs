using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class WeaponInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RifleConfig _rifle;
    [SerializeField] private CameraShakeConfig _cameraShake;
    [SerializeField] private BulletConfig _bulletCFG;
    
    public override void InstallBindings()
    {
        Debug.Log("3");
        Container.Bind<RifleConfig>().FromInstance(_rifle).AsSingle();
        Container.Bind<CameraShakeConfig>().FromInstance(_cameraShake).AsSingle();
        Container.Bind<BulletConfig>().FromInstance(_bulletCFG).AsSingle();
        Container.BindInterfacesAndSelfTo<PoolObject<Bullet>>().AsSingle();
    }
}
