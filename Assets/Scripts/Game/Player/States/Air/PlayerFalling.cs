using System;
using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States.Air
{
    public class PlayerFalling : PlayerBehaviour
    {
        public PlayerFalling(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }
        
        public override async void OnEnter()
        {
            base.OnEnter();
            Debug.Log("зашел в падение");
            await RotatePlayer();
            await PlayerAnimationPlay();
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
            Debug.Log("вышел из падения");
            Data.IsLockAim = false;
        }

        private void ChangeState()
        {
            Player.StateChain.HandleState<PlayerIdleHandler>();
            Player.StateChain.HandleState<PlayerMoveHandler>();
        }

        private async UniTask RotatePlayer()
        {
            Data.IsLockAim = true;
            await RotatePlayerToObstacle();
        }

        private async UniTask PlayerAnimationPlay()
        {
            Player.AnimatorController.PlayerAnimator.applyRootMotion = true;
            Player.AnimatorController.SetTriggerParameters(Data.Landing.animationTriggerName);
            await UniTask.Delay(TimeSpan.FromSeconds(Data.Landing.animationClipDuration));
            
            Player.AnimatorController.PlayerAnimator.applyRootMotion = false;
        }
    }
}