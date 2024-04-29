using Customs;
using Game.AsyncWorker;
using Game.Player;
using Game.Player.AnimatorScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using Game.Player.Weapons;
using Game.Player.Weapons.Mediators;
using Input;
using UnityEngine;
using Zenject;

namespace DI
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private InputSystemPC inputSystemPC;
        [SerializeField] private AnimatorController animatorController;
        [SerializeField] private PlayerAim playerAim;
        [SerializeField] private Player player; 
        [SerializeField] private ChangeModeFire fireMode;
        
        public override void InstallBindings()
        {
            BindInput();
            BindAnimator();
            BindPlayerAim();
            BindPlayer();
            BindLoader();
            BindPlayerConfig();
            BindInitStateMachine();
            BindHandlesState();
            BindStateMachineData();
            BindAsyncWorker();
            BindWeaponComponents();
            BindMediator();
            BindChangeFire();
            BindMethodInfo();
        }

        private void BindChangeFire() => BindInstance(fireMode);

        private void BindMediator() => BindNewInstance<MediatorFireStrategy>();

        private void BindWeaponComponents() => BindNewInstance<WeaponComponent>();

        private void BindInput()
        {
            BindNewInstance<InputSystem>();
            BindInstance(inputSystemPC);
        }

        private void BindAnimator() => BindInstance(animatorController);

        private void BindPlayerAim() => BindInstance(playerAim);

        private void BindPlayer() => BindInstance(player);

        private void BindLoader() => BindNewInstance<Loader>();

        private void BindInitStateMachine() => BindNewInstance<InitializationStateMachine>();

        private void BindPlayerConfig() => BindNewInstance<PlayerConfigs>();

        private void BindStateMachineData() => BindNewInstance<StateMachineData>();

        private void BindHandlesState()
        {
            Container.Bind<IStateHandle>().To<PlayerAimIdleHandler>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerAimMoveHandler>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerDashHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerIdleHandler>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerMoveHandler>().AsSingle();
            
            BindNewInstance<StateHandleChain>();
            
        }

        private void BindMethodInfo() => BindNewInstance<MethodList>();

        private void BindAsyncWorker() => BindNewInstance<AsyncWorker>();

        private void BindNewInstance<T>() => Container
            .BindInterfacesAndSelfTo<T>()
            .AsSingle()
            .NonLazy();

        private void BindInstance<T>(T instance) =>
            Container
                .BindInterfacesAndSelfTo<T>()
                .FromInstance(instance)
                .AsSingle()
                .NonLazy();
    }
}