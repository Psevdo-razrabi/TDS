using Game.Player.PlayerStateMashine;

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
                if(Player.PlayerConfigs.IsLoadAllConfig == false) return;
                Data.IsAiming.Value = true;
                Player.AnimatorController.OnAnimatorStateSet(ref Data.IsAim, true, Player.AnimatorController.NameAimParameter);
                var config = Player.PlayerConfigs.FowConfig;
                Player.RadiusChanger.ChangerRadius(config.EndValueRadius, config.StartValueRadius, config.TimeToMaxRadius);
                Player.StateChain.HandleState();
            });
            
            Player.InputSystemMouse.OnSubscribeRightMouseClickDown(() =>
            {
                if(Player.PlayerConfigs.IsLoadAllConfig == false) return;
                Data.IsAiming.Value = false;
                Player.AnimatorController.OnAnimatorStateSet(ref Data.IsAim, false, Player.AnimatorController.NameAimParameter);
                var config = Player.PlayerConfigs.FowConfig;
                Player.RadiusChanger.ChangerRadius(config.StartValueRadius, config.EndValueRadius, config.TimeToMaxRadius);
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