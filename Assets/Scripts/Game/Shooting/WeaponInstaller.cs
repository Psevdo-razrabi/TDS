using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class WeaponInstaller : MonoInstaller
{
    [SerializeField] private Transform _camera;

    public override void InstallBindings()
    {
        Container.Bind<Transform>().FromInstance(_camera).AsSingle();
        Container.Bind<TestGun>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CameraShake>().AsTransient();
        Container.Bind<Bullet>().AsTransient();
        Container.BindInterfacesAndSelfTo<ShootController>().AsSingle().NonLazy();
    }

}
