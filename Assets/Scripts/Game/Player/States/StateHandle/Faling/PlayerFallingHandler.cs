using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Air;

namespace Game.Player.States.StateHandle.Faling
{
    public class PlayerFallingHandler : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        
        public PlayerFallingHandler(PlayerStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => StateMachine.Data.IsPlayerInObstacle && !StateMachine.Data.IsLookAtObstacle.Value && !StateMachine.Data.IsGrounded.Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerFalling>();
    }
}