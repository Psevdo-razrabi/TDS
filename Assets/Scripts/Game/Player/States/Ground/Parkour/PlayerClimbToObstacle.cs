using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States.Parkour
{
    public class PlayerClimbToObstacle : BaseParkour
    {
        public PlayerClimbToObstacle(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override async void OnEnter()
        {
            base.OnEnter();
            Data.IsLockAim = true;
            await RotatePlayerToObstacle();
            PlayClimbAnimation(Data.Climb.animationClipDuration).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(Data.Climb.animationClipDuration + 0.5f));
            Data.IsPlayerInObstacle = true;
            Player.StateChain.HandleState<PlayerIdleHandler>();
            Player.StateChain.HandleState<PlayerMoveHandler>();
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
             Player.AnimatorController.SetTriggerParameters(Data.Climb.animationTriggerName);
             Player.AnimatorController.PlayerAnimator.applyRootMotion = true;
             
             Player.transform.rotation = Data.Rotation;

             await UniTask.Delay(TimeSpan.FromSeconds(durationAnimation));
            
            Player.AnimatorController.PlayerAnimator.applyRootMotion = false;
        }
    }
}