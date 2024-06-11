using System;
using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.Orientation;
using UnityEngine;

namespace Game.Player.States.Dash
{
    public class PlayerDash : PlayerOrientation
    {
        private bool _isDashing;
        private PlayerDashConfig _dashConfig;
        public PlayerDash(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            _dashConfig = Player.PlayerConfigs.DashConfig;
            Player.DashTrailEffect.ActivateVFXEffectDash();
            Debug.LogWarning("ВХОД В ДЕШ");
            Move();
        }

        public override void OnExit()
        {
            base.OnExit();
            Player.AnimatorController.OnAnimatorStateSet(ref Data.IsAim, false, Player.AnimatorController.NameAimParameter);
            Debug.LogWarning("ВЫХОД В ДЕШ");
            Player.AnimatorController.OnAnimatorStateSet(Data.IsDashing, false, Player.AnimatorController.NameDashParameter);
        }

        protected async override void Move()
        {
            await Dash();
        }

        private void SwitchState()
        {
            Player.StateChain.HandleState();
        }

        private void AwaitDash()
        {
            Player.AsyncWorker.Dash(-1);
        }

        private async UniTask Dash()
        {
            var startPosition = Player.transform.position;

            var endPosition = startPosition + Movement.normalized * _dashConfig.DashDistance;

            if (Physics.Raycast(startPosition, Movement.normalized, out var raycastHit,
                    _dashConfig.DashDistance, _dashConfig.LayerObstacle))
            {
                endPosition = raycastHit.point;
            }
            
            var elapsedTime = 0f;
            
            while (elapsedTime < _dashConfig.DashDuration)
            {
                var currentPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / _dashConfig.DashDuration);
                Player.CharacterController.Move(currentPosition - Player.transform.position);
                
                elapsedTime += Time.deltaTime;

                await UniTask.Yield();
            }

            Player.AnimatorController.OnAnimatorStateSet(Data.IsDashing, false, Player.AnimatorController.NameDashParameter);
            SwitchState();
            AwaitDash();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_dashConfig.DelayAfterEachDash));
            Debug.LogWarning("ДОШЕЛ ДО КОНЦА");
        }
    }
}