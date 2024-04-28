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
    [SerializeField] private Bullet _bullet;
    
    public override void InstallBindings()
    {
        Debug.Log("ALLGOOD");
        
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        Container.Bind<TestGun>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Crosshair>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<ShootController>().AsSingle().NonLazy();
        Container.Bind<RifleConfig>().FromInstance(_rifle).AsSingle();
        Container.Bind<CameraShakeConfig>().FromInstance(_cameraShake).AsSingle();
        Container.Bind<BulletConfig>().FromInstance(_bulletCFG).AsSingle();
        Container.Bind<Bullet>().FromInstance(_bullet).AsSingle();
        Container.BindInterfacesAndSelfTo<PoolObject<Bullet>>().AsSingle();
    }
}
