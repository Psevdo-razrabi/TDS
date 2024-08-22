using System;
using Enemy;
using Game.Core.Health;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using UniRx;
using UnityEngine;
using Zenject;
    

namespace Game.Player
{
    public class Player : IDisposable, ITickable, IHit, IInitializable<Player>
    {
        public PlayerConfigs PlayerConfigs { get; private set; }
        public Subject<Unit> Hit { get; private set; } = new();
        public AsyncOperation.AsyncWorker AsyncWorker { get; private set; }
        public readonly PlayerInputStorage PlayerInputStorage;
        public readonly PlayerComponents PlayerComponents;
        public readonly PlayerView PlayerView;
        public readonly PlayerStateMachine PlayerStateMachine;
        public readonly PlayerIK PlayerIK;
        public readonly PlayerAnimation PlayerAnimation;
        private readonly CompositeDisposable _disposable = new();

        public Player(PlayerConfigs playerConfigs, AsyncOperation.AsyncWorker asyncWorker, PlayerInputStorage playerInputStorage, 
            PlayerComponents playerComponents, PlayerView playerView, 
            PlayerStateMachine playerStateMachine, PlayerIK playerIK, PlayerAnimation playerAnimation, StateHandleChain stateHandleChain)
        {
            PlayerConfigs = playerConfigs;
            AsyncWorker = asyncWorker;
            PlayerInputStorage = playerInputStorage;
            PlayerComponents = playerComponents;
            PlayerView = playerView;
            PlayerStateMachine = playerStateMachine;
            PlayerIK = playerIK;
            PlayerAnimation = playerAnimation;

            PlayerStateMachine.InitStateChain(stateHandleChain);
            PlayerStateMachine.InitPlayer(this);
        }

        public void Dispose()
        {
            _disposable.Clear();
            _disposable.Dispose();
        }

        public async void Initialize()
        {
            await AsyncWorker.AwaitLoadConfigs(PlayerConfigs);
            PlayerStateMachine.Data.DashCount = PlayerConfigs.MovementConfigsProvider.DashConfig.NumberChargesDash;

            var die = new Die(PlayerComponents.RagdollHelper);

            var restoringHealth = new RestoringHealth<PlayerComponents>(
                new Health<Player>(PlayerConfigs.AnyPlayerConfigs.HealthConfig.MaxHealth, PlayerView.ValueModelHealth,
                    die),
                PlayerConfigs.AnyPlayerConfigs.HealthConfig, PlayerView.ValueModelHealth);

            HealthResoringConstyl._playerRestoring = restoringHealth;
            
            PlayerComponents.InitHealth(restoringHealth);
        }

        public void Tick()
        {
            if(PlayerStateMachine.StateMachine == null) return;
            if (!PlayerStateMachine.StateMachine.isUpdate) return;

            PlayerStateMachine.StateMachine?.currentStates.OnUpdateBehaviour();
        }
    }

    public interface IInitializable<T>
    {
        void Initialize();
    }
}