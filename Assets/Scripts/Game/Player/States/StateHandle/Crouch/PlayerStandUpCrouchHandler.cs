using Game.Player.PlayerStateMashine;
using Game.Player.States.Crouching;

namespace Game.Player.States.StateHandle
{
    public class PlayerStandUpCrouchHandler : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; }
        public PlayerStandUpCrouchHandler(InitializationStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsAim || StateMachine.Data.IsDashing.Value || !StateMachine.Data.IsCrouch;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerStandUp>();
    }
}