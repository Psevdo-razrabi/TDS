using System;
using Cysharp.Threading.Tasks;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.StateHandle;
using UniRx;

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
            Data.SetValue(Name.IsLockAim, true);
            await RotatePlayerToObstacle();
            await AnimationPlayClip();
            Data.SetValue(Name.IsPlayerInObstacle, true);
            ZeroingRotation();
            ChangeState();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.GetValue<ReactiveProperty<bool>>(Name.IsClimbing).Value = false;
            Data.SetValue(Name.IsLockAim, false);
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            MatchAnimatorState(Data.GetValue<ClimbParameters>(Name.Climb).animationTriggerName, Data.GetValue<ObstacleParametersConfig>(Name.ObstacleConfig).AvatarTargetForClimb,
                Data.GetValue<ObstacleParametersConfig>(Name.ObstacleConfig).WeightMask, Data.GetValue<ObstacleParametersConfig>(Name.ObstacleConfig).AnimationClip.start,
                Data.GetValue<ObstacleParametersConfig>(Name.ObstacleConfig).AnimationClip.end);
        }

        private async UniTaskVoid PlayClimbAnimation(float durationAnimation)
        {
             Player.PlayerAnimation.AnimatorController.SetTriggerParameters(Data.GetValue<ClimbParameters>(Name.Climb).animationTriggerName);
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
            PlayClimbAnimation(Data.GetValue<ClimbParameters>(Name.Climb).animationClipDuration).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(Data.GetValue<ClimbParameters>(Name.Climb).animationClipDuration + 0.5f));
        }
    }
}