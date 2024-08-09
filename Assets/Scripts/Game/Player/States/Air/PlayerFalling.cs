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
            Data.IsLockAim = true;
            await RotatePlayerToObstacle();
            Player.AnimatorController.PlayerAnimator.applyRootMotion = true;
            Player.AnimatorController.SetTriggerParameters(Player.AnimatorController.NameIsLandingParameter);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            
            Player.AnimatorController.PlayerAnimator.applyRootMotion = false;
            Player.StateChain.HandleState<PlayerIdleHandler>();
            Player.StateChain.HandleState<PlayerMoveHandler>();
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
    }
}