using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class BaseMove : GroundState
    {
        protected Vector3 Movement;
        protected PlayerMoveConfig PlayerMoveConfig;
        private Vector3 _speed;
        private CancellationTokenSource _token = new ();

        protected BaseMove(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) :
            base(stateMachine, player, stateMachineData)
        {
        }

        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();

            Player.InputSystem.Move
                .Subscribe(vector =>
                {
                    Movement = new Vector3(vector.x, 0f, vector.y).normalized;
                    UpdateDesiredTargetSpeed(PlayerMoveConfig);
                })
                .AddTo(Disposable);

            Player.InputSystem.OnSubscribeDash(() =>
            {
                if (Data.DashCount == 0) return;

                Player.AnimatorController.OnAnimatorStateSet(Data.IsDashing, true, Player.AnimatorController.NameDashParameter);
                Player.StateChain.HandleState();
            });
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            Disposable.Clear();
        }
        
        protected async void UpdateDesiredTargetSpeed(PlayerMoveConfig configs)
        {
            CreateToken();
            switch (Data.XInput, Data.YInput)
            {
                case (var xInput, 0) when xInput != 0:
                    await InterpolateSpeed(Data.CurrentSpeed, configs.SpeedStrafe, configs.TimeInterpolateSpeed, _token.Token);
                    break;
                case (0, > 0):
                    await InterpolateSpeed(Data.CurrentSpeed, configs.Speed, configs.TimeInterpolateSpeed, _token.Token);
                    break;
                case (0, < 0):
                    await InterpolateSpeed(Data.CurrentSpeed, configs.SpeedBackwards, configs.TimeInterpolateSpeed, _token.Token);
                    break; 
                case (var xInput, > 0) when xInput != 0:
                    await InterpolateSpeed(Data.CurrentSpeed, configs.SpeedAngleForward, configs.TimeInterpolateSpeed, _token.Token);
                    break;
                case (var xInput,< 0) when xInput != 0:
                    await InterpolateSpeed(Data.CurrentSpeed, configs.SpeedAngleBackwards, configs.TimeInterpolateSpeed, _token.Token);
                    break;
            }
        }

        private async UniTask InterpolateSpeed(float currentSpeed, float endValue, float duration, CancellationToken cancellationToken)
        { 
            await DOTween
                .To(() => currentSpeed, x => Data.CurrentSpeed = x, endValue, duration)
                .WithCancellation(cancellationToken: cancellationToken);
        }

        private void ClearToken(CancellationTokenSource cancellationToken)
        {
            cancellationToken?.Cancel();
        }

        private void CreateToken()
        {
            ClearToken(_token);
            _token = new CancellationTokenSource();
        }

        protected virtual void Move()
        {
            var targetSpeed = Data.CurrentSpeed * Time.deltaTime * Movement;
            targetSpeed.y = Data.TargetDirectionY;
            Player.CharacterController.Move(targetSpeed);
        }
    }
}