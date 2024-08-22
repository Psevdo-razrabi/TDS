using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Crouching;

namespace Game.Player.States.StateHandle
{
    public class PlayerStandUpCrouchHandler : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerStandUpCrouchHandler(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsAim.Value || StateMachine.Data.IsDashing.Value || !StateMachine.Data.IsCrouch.Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerStandUp>();
    }
}