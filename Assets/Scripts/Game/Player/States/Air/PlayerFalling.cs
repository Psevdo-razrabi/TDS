using System;
using Cysharp.Threading.Tasks;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;

namespace Game.Player.States.Air
{
    public class PlayerFalling : PlayerBehaviour
    {
        public PlayerFalling(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }
        
        public override async void OnEnter()
        {
            base.OnEnter();
            await RotatePlayer();
            await PlayerAnimationPlay();
            ZeroingRotation();
            ChangeState();
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.SetValue(Name.IsLockAim, false);
        }

        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerIdleHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerMoveHandler>();
        }

        private async UniTask RotatePlayer()
        {
            Data.SetValue(Name.IsLockAim, true);
            await RotatePlayerToObstacle();
        }

        private async UniTask PlayerAnimationPlay()
        {
            Player.PlayerAnimation.AnimatorController.PlayerAnimator.applyRootMotion = true;
            Player.PlayerAnimation.AnimatorController.SetTriggerParameters(Data.GetValue<LandingParameters>(Name.Landing).animationTriggerName);
            await UniTask.Delay(TimeSpan.FromSeconds(Data.GetValue<LandingParameters>(Name.Landing).animationClipDuration));
            
            Player.PlayerAnimation.AnimatorController.PlayerAnimator.applyRootMotion = false;
        }
    }
}