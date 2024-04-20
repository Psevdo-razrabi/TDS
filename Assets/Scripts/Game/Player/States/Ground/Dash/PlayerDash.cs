using System;
using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;

namespace Game.Player.States.Dash
{
    public class PlayerDash : BaseMove
    {
        private bool _isDashing;
        private PlayerDashConfig _dashConfig;
        public PlayerDash(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _dashConfig = Player.PlayerConfigs.DashConfig;
            Debug.Log("вошел в состояние Dash");
            Move();
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("вышел из состояние Dash");
            OnAnimatorStateSet(ref Data.IsAim, false, Player.AnimatorController.NameAimParameters);
        }

        protected async override void Move()
        {
            await Dash();
        }

        private void SwitchState()
        {
            Player.StateChain.HandleState();
        }

        private async void AwaitDash()
        {
            await Player.AsyncWorker.Dash(-1);
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
            
            OnAnimatorStateSet(ref Data.IsDashing, false, Player.AnimatorController.NameDashParameters);

            SwitchState();
            AwaitDash();

            await UniTask.Delay(TimeSpan.FromSeconds(_dashConfig.DelayAfterEachDash));
        }

    }
}