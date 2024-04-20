using System;
using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.Dash;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class BaseMove : GroundState
    {
        protected Vector3 Movement;

        protected BaseMove(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();

            Player.InputSystem.Move
                .Subscribe(vector => Movement = new Vector3(vector.x, 0f, vector.y))
                .AddTo(Disposable);
            
            Player.InputSystem.OnSubscribeDash(() =>
            {
                if(Data.DashCount == 0) return;
                
                OnAnimatorStateSet(ref Data.IsDashing, true, Player.AnimatorController.NameDashParameters);
                Player.StateChain.HandleState();
            });
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();

            Player.InputSystem.OnUnsubscribeDash();

            Disposable.Clear();
        }
        
        protected void UpdateDesiredTargetSpeed(PlayerMoveConfig configs)
        {
            Data.CurrentSpeed = Data.CurrentSpeed switch
            {
                _ when Data.XInput != 0 => configs.SpeedStrafe,
                _ when Data.YInput < 0 => configs.SpeedBackwards,
                _ when Data.YInput > 0 => configs.Speed,
                _ => Data.CurrentSpeed
            };
        }

        protected virtual void Move()
        {
            var speed = Movement.magnitude * Data.CurrentSpeed * Time.deltaTime;

            Player.CharacterController.Move(speed * Movement);
            
            Debug.Log("я выполняюсь");
        }
    }
}