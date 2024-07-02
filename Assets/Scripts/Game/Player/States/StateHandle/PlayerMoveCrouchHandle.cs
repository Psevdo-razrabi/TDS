using Game.Player.PlayerStateMashine;
using Game.Player.States.Crouching;

namespace Game.Player.States.StateHandle
{
    public class PlayerMoveCrouchHandle : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; }
        public PlayerMoveCrouchHandle(InitializationStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsPlayerSitDown;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerCrouch>();
    }
}