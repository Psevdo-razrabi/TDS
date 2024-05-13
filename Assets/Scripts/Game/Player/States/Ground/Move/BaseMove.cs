using System.Collections.Generic;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.DirectionStrategy;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player.States
{
    public abstract class BaseMove : GroundState
    {
        protected Vector3 Movement;
        private float x;
        private float y;

        private Dictionary<Vector3, IDirectionCalculator> _dictionarySpeed = new()
        {
            { Vector3.forward, new ForwardCalculator() },
            { Vector3.back, new BackwardCalculator() },
            { Vector3.right, new StrafeCalculator() },
            { Vector3.left, new StrafeCalculator() }
        };

        protected BaseMove(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();

            Player.InputSystem.Move
                .Subscribe(vector => Movement = new Vector3(vector.x, 0f, vector.y).normalized)
                .AddTo(Disposable);
            
            Player.InputSystem.OnSubscribeDash(() =>
            {
                if(Data.DashCount == 0) return;
                
                OnAnimatorStateSet(ref Data.IsDashing, true, Player.AnimatorController.NameDashParameter);
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
            Data.CurrentSpeed = _dictionarySpeed[Movement].CalculatedSpeed(Data.MouseDirection, Movement, configs);

            x = Mathf.Abs(Data.MouseDirection.x) < Mathf.Abs(Data.MouseDirection.y) ? (Data.MouseDirection.y > 0 ? -1 : 1) : (Data.MouseDirection.x > 0 ? 1 : -1);

            y = Mathf.Approximately(Data.CurrentSpeed, configs.Speed) || Mathf.Approximately(Data.CurrentSpeed, configs.SpeedStrafe) ? 1 : -1;

            Player.AnimatorController.SetFloatParameters("MouseX", x);
            Player.AnimatorController.SetFloatParameters("MouseY", y);
        }

        protected virtual void Move()
        {
            var speed = Data.CurrentSpeed * Time.deltaTime;
            Player.CharacterController.Move(speed * Movement);
        }
    }
}