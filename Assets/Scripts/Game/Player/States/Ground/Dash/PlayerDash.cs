using System;
using Cysharp.Threading.Tasks;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.Orientation;
using UnityEngine;

namespace Game.Player.States.Dash
{
    public class PlayerDash : PlayerOrientation
    {
        private bool _isDashing;
        private PlayerDashConfig _dashConfig;
        
        public PlayerDash(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            _dashConfig = Player.PlayerConfigs.MovementConfigsProvider.DashConfig;
            Player.PlayerView.DashTrailEffect.ActivateVFXEffectDash();
            Move();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsAim.Value = false;
            Player.PlayerAnimation.AnimatorController.OnAnimatorStateSet(Data.IsDashing, false, Player.PlayerAnimation.AnimatorController.NameDashParameter);
        }

        protected override async void Move()
        {
            await Dash();
        }

        private void SwitchState()
        {
            Player.PlayerStateMachine.StateChain.HandleState();
        }

        private void AwaitDash()
        {
            Player.AsyncWorker.Dash(-1);
        }

        private async UniTask Dash()
        {
            var startPosition = Player.PlayerComponents.transform.position;

            var endPosition = startPosition + Data.Movement.normalized * _dashConfig.DashDistance;

            if (Physics.Raycast(startPosition, Data.Movement.normalized, out var raycastHit,
                    _dashConfig.DashDistance, _dashConfig.LayerObstacle))
            {
                endPosition = raycastHit.point;
            }
            
            var elapsedTime = 0f;
            
            while (elapsedTime < _dashConfig.DashDuration)
            {
                var currentPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / _dashConfig.DashDuration);
                Player.PlayerComponents.CharacterController.Move(currentPosition - Player.PlayerComponents.transform.position);
                
                elapsedTime += Time.deltaTime;

                await UniTask.Yield();
            }

            Player.PlayerAnimation.AnimatorController.OnAnimatorStateSet(Data.IsDashing, false, Player.PlayerAnimation.AnimatorController.NameDashParameter);
            SwitchState();
            AwaitDash();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_dashConfig.DelayAfterEachDash));
        }
    }
}