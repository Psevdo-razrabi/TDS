using System;
using Cysharp.Threading.Tasks;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;

namespace Game.Player.States.Parkour
{
    public class PlayerClimbToObstacle : BaseParkour
    {
        public PlayerClimbToObstacle(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override async void OnEnter()
        {
            base.OnEnter();
            Data.IsLockAim = true;
            await RotatePlayerToObstacle();
            await AnimationPlayClip();
            Data.IsPlayerInObstacle = true;
            ZeroingRotation();
            ChangeState();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsClimbing.Value = false;
            Data.IsLockAim = false;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            MatchAnimatorState(Data.Climb.animationTriggerName, Data.ObstacleConfig.AvatarTargetForClimb,
                Data.ObstacleConfig.WeightMask, Data.ObstacleConfig.AnimationClip.start,
                Data.ObstacleConfig.AnimationClip.end);
        }

        private async UniTaskVoid PlayClimbAnimation(float durationAnimation)
        {
             Player.PlayerAnimation.AnimatorController.SetTriggerParameters(Data.Climb.animationTriggerName);
             Player.PlayerAnimation.AnimatorController.PlayerAnimator.applyRootMotion = true;
             
             await UniTask.Delay(TimeSpan.FromSeconds(durationAnimation));
            
            Player.PlayerAnimation.AnimatorController.PlayerAnimator.applyRootMotion = false;
        }

        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerIdleHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerMoveHandler>();
        }

        private async UniTask AnimationPlayClip()
        {
            PlayClimbAnimation(Data.Climb.animationClipDuration).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(Data.Climb.animationClipDuration + 0.5f));
        }
    }
}