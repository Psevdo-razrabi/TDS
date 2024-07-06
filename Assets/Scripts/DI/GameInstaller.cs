using CharacterOrEnemyEffect;
using CharacterOrEnemyEffect.Factory;
using FOW;
using Game.AsyncWorker;
using Game.Player;
using Game.Player.AnimatorScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.Buffer;
using Game.Player.States.StateHandle;
using Game.Player.States.Subscribers;
using Input;
using UnityEngine;

namespace DI
{
    public sealed class GameInstaller : BaseBindings
    {
        [SerializeField] private InputSystemMovement inputSystemMovement;
        [SerializeField] private AnimatorController animatorController;
        [SerializeField] private PlayerAim playerAim;
        [SerializeField] private Player player;
        [SerializeField] private DashTrailEffect dashTrailEffect;
        [SerializeField] private ChangeModeFire fireMode;
        [SerializeField] private InputSystemMouse inputSystemMouse;
        [SerializeField] private InputSystemWeapon inputSystemWeapon;
        [SerializeField] private InputSystemUi inputSystemUi;
        [SerializeField] private InputBuffer inputBuffer;
        [SerializeField] private FogOfWarRevealer3D fogOfWarRevealer3D;
        [SerializeField] private StorageAssetReference _storageAssetReference;
        
        public override void InstallBindings()
        {
            BindEventController();
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
                .AsSingle()
                .WithArguments(true, "Mesh", 10)
                .WhenInjectedInto<CreateVFXTrail>()
                .NonLazy();
        }

        private void BindEventController() => BindNewInstance<EventController>();

        private void BindInput()
        {
            BindNewInstance<InputSystem>();
            BindInstance(inputSystemMovement);
            BindInstance(inputSystemMouse);
            BindInstance(inputSystemWeapon);
            BindInstance(inputSystemUi);
            BindNewInstance<InputObserver>();
            BindNewInstance<BufferAction>();
            BindInstance(inputBuffer);
        }

        private void BindEffect()
        {
            BindNewInstance<BulletEffectSystem>();
        }
        
        private void BindAnimator() => BindInstance(animatorController);

        private void BindPlayerAim() => BindInstance(playerAim);

        private void BindPlayer()
        {
            BindInstance(player);
            BindInstance(dashTrailEffect);
            BindNewInstance<CrouchSubscribe>();
        }

        private void BindLoader()
        {
            BindNewInstance<Loader>();
            BindInstance(_storageAssetReference);
        }

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
            Container.Bind<IStateHandle>().To<PlayerIdleCrouchHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerMoveCrouchHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerSitDownCrouchHandle>().AsSingle();
            Container.Bind<IStateHandle>().To<PlayerStandUpCrouchHandler>().AsSingle();
            
            BindNewInstance<StateHandleChain>();
        }

        private void BindAsyncWorker() => BindNewInstance<AsyncWorker>();
    }
}