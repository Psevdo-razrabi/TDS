using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class GroundState : PlayerBehaviour
    {
        protected GroundState(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
            
        }
        
        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();
            Player.InputSystemMouse.OnSubscribeRightMouseClickUp(() =>
            {
                Data.IsAiming.Value = true;
                Player.AnimatorController.OnAnimatorStateSet(ref Data.IsAim, true, Player.AnimatorController.NameAimParameter);
                Player.StateChain.HandleState();
            });
            
            Player.InputSystemMouse.OnSubscribeRightMouseClickDown(() =>
            {
                Data.IsAiming.Value = false;
                Player.AnimatorController.OnAnimatorStateSet(ref Data.IsAim, false, Player.AnimatorController.NameAimParameter);
                Player.StateChain.HandleState();
            });
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            
            Player.InputSystemMouse.OnUnsubscribeRightMouseClickUp();
            
            Player.InputSystemMouse.OnUnsubscribeRightMouseClickDown();
        }

        //private void OnJumpPressedKey() => StateMachine.PlayerStateMachine.SwitchStates<>(); //Jump
    }
}