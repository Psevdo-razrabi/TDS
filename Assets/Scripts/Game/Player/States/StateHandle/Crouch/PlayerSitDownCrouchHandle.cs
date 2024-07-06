using Game.Player.PlayerStateMashine;
using Game.Player.States.Crouching;

namespace Game.Player.States.StateHandle
{
    public class PlayerSitDownCrouchHandle : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; }
        public PlayerSitDownCrouchHandle(InitializationStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsCrouch;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerSitsDown>();
    }
}