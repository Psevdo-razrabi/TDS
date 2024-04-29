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
            UpdateAnimatorMouseInput();
        }

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
            
            //Player.AnimatorController.SetFloatParameters(Player.AnimatorController.NameHorizontalParameter, Data.XInput);
            //Player.AnimatorController.SetFloatParameters(Player.AnimatorController.NameVerticalParameter, Data.YInput);
        }

        private void UpdateAnimatorMouseInput()
        {
            Player.AnimatorController.SetFloatParameters(Player.AnimatorController.NameMouseXParameter, Data.MouseDirection.x);
            Player.AnimatorController.SetFloatParameters(Player.AnimatorController.NameMouseYParameter, Data.MouseDirection.y);
        }
    }
}