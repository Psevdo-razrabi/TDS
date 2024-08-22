using CharacterOrEnemyEffect;
using CharacterOrEnemyEffect.Factory;
using FOW;
using Game.AsyncOperation;
using Game.Player;
using Game.Player.AnimatorScripts;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.Buffer;
using Game.Player.States.StateHandle;
using Game.Player.States.StateHandle.Faling;
using Game.Player.States.StateHandle.Parkour;
using Input;
using UnityEngine;

namespace DI
{
    public sealed class GameInstaller : BaseBindings
    {
        [SerializeField] private AnimatorController animatorController;
        [SerializeField] private PlayerAim playerAim;
        [SerializeField] private ChangeModeFire fireMode;
        [SerializeField] private FogOfWarRevealer3D fogOfWarRevealer3D;
        [SerializeField] private StorageAssetReference _storageAssetReference;
        [SerializeField] private PlayerComponents _playerComponents;
        [SerializeField] private IKSystem _ikSystem;
        [SerializeField] private PlayerView _playerView;


        public override void InstallBindings()
        {
            BindInput();
            BindAnimator();
            BindPlayerAim();
            BindPlayerConfig();
            BindStateMachineData();
            BindPlayer();
            BindLoader();
            BindHandlesState();
            BindAsyncWorker();
            BindEffect();
            BindFactory();
            BindPool();
            BindRevealer();
        }

        private void BindRevealer()
        {
            BindInstance(fogOfWarRevealer3D);
            BindNewInstance<FOWRadiusChanger>();
            BindNewInstance<HiderManager>();
        }

        private void BindPool()
        {
            BindNewInstance<PoolObject>();
        }

        private void BindFactory()
        {
            BindNewInstance<FactoryComponent>();
            BindNewInstance<FactoryGameObject>();
            Container
                .Bind<FactoryComponentWithMonoBehaviour>()
                .To<FactoryComponentWithMonoBehaviour>()
                .WithArguments(true, "Mesh", 10)
                .WhenInjectedInto<CreateVFXTrail>()
                .NonLazy();
            Container.Bind<FactoryComponentWithMonoBehaviour>().To<FactoryComponentWithMonoBehaviour>().WithArguments(true, "Bullet", 30).WhenInjectedInto<BulletLifeCycle>().NonLazy();
        }

        private void BindInput()
        {
            BindNewInstance<InputSystem>();
            BindNewInstance<InputSystemMovement>();
            BindNewInstance<InputSystemMouse>();
            BindNewInstance<InputSystemWeapon>();
            BindNewInstance<InputSystemUi>();
            BindNewInstance<InputObserver>();
            BindNewInstance<BufferAction>();
            BindNewInstance<InputBuffer>();
        }

        private void BindEffect()
        {
            BindNewInstance<BulletEffectSystem>();
        }
        
        private void BindAnimator() => BindInstance(animatorController);

        private void BindPlayerAim() => BindInstance(playerAim);

        private void BindPlayer()
        {
            BindNewInstance<Player>();
            BindNewInstance<PlayerAnimation>();
            BindInstance(_playerComponents);
            BindInstance(_ikSystem);
            BindNewInstance<PlayerInputStorage>();
            BindNewInstance<PlayerStateMachine>();
            BindInstance(_playerView);
            BindNewInstance<PlayerIK>();
            BindNewInstance<Landing>();
        }

        private void BindLoader()
        {
            BindNewInstance<Loader>();
            BindInstance(_storageAssetReference);
        }

        private void BindPlayerConfig() => BindNewInstance<Game.Player.PlayerStateMashine.PlayerConfigs>();

        private void BindStateMachineData() => BindNewInstance<StateMachineData>();

        private void BindHandlesState()
        {
            Container.Bind<IStateHandle>().To<PlayerAimIdleHandler>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerAimMoveHandler>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerDashHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerIdleHandler>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerMoveHandler>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerIdleCrouchHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerMoveCrouchHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerSitDownCrouchHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerStandUpCrouchHandler>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerClimbToObstacleHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerFallingHandler>().AsSingle();
            
            BindNewInstance<StateHandleChain>();
        }

        private void BindAsyncWorker() => BindNewInstance<AsyncWorker>();
    }
}