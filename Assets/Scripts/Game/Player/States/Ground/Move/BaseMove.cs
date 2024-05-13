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
           Data.CurrentSpeed = Data.CurrentSpeed switch
            {
                _ when Mathf.Abs(Data.MouseDirection.x) < Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.y > 0 && Movement == Vector3.forward => configs.Speed,
                _ when Mathf.Abs(Data.MouseDirection.x) < Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.y < 0 && Movement == Vector3.back => configs.Speed,
                _ when Mathf.Abs(Data.MouseDirection.x) > Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.x > 0 && Movement == Vector3.right => configs.Speed,
                _ when Mathf.Abs(Data.MouseDirection.x) > Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.x < 0 && Movement == Vector3.left => configs.Speed,
                _ => configs.SpeedBackwards
            };

            x = x switch
            {
                _ when Mathf.Abs(Data.MouseDirection.x) < Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.y > 0 && Movement == Vector3.left => x=-1f,
                _ when Mathf.Abs(Data.MouseDirection.x) < Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.y < 0 && Movement == Vector3.right => x=-1f,
                _ when Mathf.Abs(Data.MouseDirection.x) > Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.x > 0 && Movement == Vector3.forward => x=-1f,
                _ when Mathf.Abs(Data.MouseDirection.x) > Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.x < 0 && Movement == Vector3.back => x=-1f,
                _ when Mathf.Abs(Data.MouseDirection.x) < Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.y > 0 && Movement == Vector3.right => x = 1f,
                _ when Mathf.Abs(Data.MouseDirection.x) < Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.y < 0 && Movement == Vector3.left => x = 1f,
                _ when Mathf.Abs(Data.MouseDirection.x) > Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.x > 0 && Movement == Vector3.back => x = 1f,
                _ when Mathf.Abs(Data.MouseDirection.x) > Mathf.Abs(Data.MouseDirection.y) && Data.MouseDirection.x < 0 && Movement == Vector3.forward => x = 1f,
                _ => x=0
            };

            y = y switch
            {
                _ when Data.CurrentSpeed == configs.Speed => y = 1,
                _ when Data.CurrentSpeed == configs.SpeedBackwards => y = -1,
                _ => y = 0
            };

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