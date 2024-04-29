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
            Player.InputSystem.OnSubscribeMouseClickUp(() =>
            {
                OnAnimatorStateSet(ref Data.IsAim, true, Player.AnimatorController.NameAimParameter);
                Player.StateChain.HandleState();
            });
            
            Player.InputSystem.OnSubscribeMouseClickDown(() =>
            {
                OnAnimatorStateSet(ref Data.IsAim, false, Player.AnimatorController.NameAimParameter);
                Player.StateChain.HandleState();
            });
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            
            Player.InputSystem.OnUnsubscribeMouseClickUp();
            
            Player.InputSystem.OnUnsubscribeMouseClickDown();
        }

        //private void OnJumpPressedKey() => StateMachine.PlayerStateMachine.SwitchStates<>(); //Jump
    }
}