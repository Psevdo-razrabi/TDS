using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Interfase;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class PlayerBehaviour : IState
    {
        protected readonly InitializationStateMachine StateMachine;
        protected CompositeDisposable Disposable = new();
        protected readonly Player Player;
        protected readonly StateMachineData Data;

        public virtual void OnEnter() => AddActionsCallbacks();

        public virtual void OnExit() => RemoveActionCallbacks();

        public virtual void OnUpdateBehaviour()
        {
            Player.PlayerAim.Aim();
            UpdateAnimatorInput();
        }
        public virtual void OnFixedUpdateBehaviour() {}

        protected PlayerBehaviour(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData)
        {
            StateMachine = stateMachine;
            Player = player;
            Data = stateMachineData;
        }

        protected virtual void AddActionsCallbacks() {}

        protected virtual void RemoveActionCallbacks() {}

        private void UpdateAnimatorInput()
        {
            Data.XInput = UnityEngine.Input.GetAxis("Horizontal");
            Data.YInput = UnityEngine.Input.GetAxis("Vertical");
        }
    }
}