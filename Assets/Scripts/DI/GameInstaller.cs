﻿using Customs;
using Game.AsyncWorker;
using Game.Player;
using Game.Player.AnimatorScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using Game.Player.Weapons;
using Game.Player.Weapons.Mediators;
using Input;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace DI
{
    public class GameInstaller : BaseBindings
    {
        [SerializeField] private InputSystemMovement inputSystemMovement;
        [SerializeField] private AnimatorController animatorController;
        [SerializeField] private PlayerAim playerAim;
        [SerializeField] private Player player; 
        [SerializeField] private ChangeModeFire fireMode;
        [SerializeField] private InputSystemMouse inputSystemMouse;
        [SerializeField] private InputSystemWeapon inputSystemWeapon;
        
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
            BindMethodInfo();
        }

        private void BindInput()
        {
            BindNewInstance<InputSystem>();
            BindInstance(inputSystemMovement);
            BindInstance(inputSystemMouse);
            BindInstance(inputSystemWeapon);
            BindNewInstance<MouseInputObserver>();
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
    }
}