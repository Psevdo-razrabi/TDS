using Game.Player.PlayerStateMashine;
using Game.Player.States.Air;

namespace Game.Player.States.StateHandle.Faling
{
    public class PlayerFallingHandler : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; set; }
        
        public PlayerFallingHandler(InitializationStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => StateMachine.Data.IsPlayerInObstacle && !StateMachine.Data.IsLookAtObstacle.Value && !StateMachine.Data.IsGrounded.Value;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerFalling>();
    }
}