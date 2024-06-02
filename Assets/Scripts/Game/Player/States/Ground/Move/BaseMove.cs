using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.DirectionStrategy;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class BaseMove : GroundState
    {
        protected Vector3 Movement;
        private Vector3 _speed;

        private Dictionary<Vector3, IDirectionCalculator> _dictionarySpeed = new()
        {
            { Vector3.forward, new ForwardCalculator() },
            { Vector3.back, new BackwardCalculator() },
            { Vector3.right, new StrafeCalculator() },
            { Vector3.left, new StrafeCalculator() }
        };

        protected BaseMove(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) :
            base(stateMachine, player, stateMachineData)
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
                if (Data.DashCount == 0) return;

                OnAnimatorStateSet(ref Data.IsDashing, true, Player.AnimatorController.NameDashParameter);
                Player.DashTrailEffect.ActivateVFXEffectDash();
                Player.StateChain.HandleState();
            });
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();

            Player.InputSystem.OnUnsubscribeDash();

            Disposable.Clear();
        }

        protected async void UpdateDesiredTargetSpeed(PlayerMoveConfig configs)
        {
            switch (Data.XInput, Data.YInput)
            {
                case (var xInput, 0) when xInput != 0:
                    await InterpolateSpeed(Data.CurrentSpeed, configs.SpeedStrafe, 0.3f);
                    break;
                case (0, > 0):
                    await InterpolateSpeed(Data.CurrentSpeed, configs.Speed, 0.3f);
                    break;
                case (0, < 0):
                    await InterpolateSpeed(Data.CurrentSpeed, configs.SpeedBackwards, 0.3f);
                    break; 
                case (var xInput, > 0) when xInput != 0:
                    await InterpolateSpeed(Data.CurrentSpeed, configs.SpeedAngleForward, 0.3f);
                    break;
                case (var xInput,< 0) when xInput != 0:
                    await InterpolateSpeed(Data.CurrentSpeed, configs.SpeedAngleBackwards, 0.3f);
                    break;
            }
        }

        private async UniTask InterpolateSpeed(float currentSpeed, float endValue, float duration)
        {
            await DOTween.To(() => currentSpeed, x => Data.CurrentSpeed = x, endValue, duration);
        }

        protected virtual void Move()
        {
            var targetSpeed = Data.CurrentSpeed * Time.deltaTime * Movement;
            targetSpeed.y = Data.TargetDirectionY;
            Debug.LogWarning(targetSpeed);
            Player.CharacterController.Move(targetSpeed);
        }
    }
}