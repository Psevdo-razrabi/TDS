using Game.Player.PlayerStateMashine;

namespace Game.Player.States
{
    public abstract class GroundState : PlayerBehaviour
    {
        protected GroundState(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
            
        }

        protected void OnAnimatorStateSet(ref bool parameters, bool state, string nameStateAnimator)
        {
            parameters = state;
            Player.AnimatorController.SetBoolParameters(nameStateAnimator, state);
        }
        
        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();
            Player.InputSystemMouse.OnSubscribeRightMouseClickUp(() =>
            {
                OnAnimatorStateSet(ref Data.IsAim, true, Player.AnimatorController.NameAimParameter);
                Player.StateChain.HandleState();
            });
            
            Player.InputSystemMouse.OnSubscribeRightMouseClickDown(() =>
            {
                OnAnimatorStateSet(ref Data.IsAim, false, Player.AnimatorController.NameAimParameter);
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