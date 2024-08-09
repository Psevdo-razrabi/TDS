using Game.Player.PlayerStateMashine;
using Game.Player.States.Parkour;

namespace Game.Player.States.StateHandle.Parkour
{
    public class PlayerClimbToObstacleHandle : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; set; }
        
        public PlayerClimbToObstacleHandle(InitializationStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsClimbing.Value && StateMachine.Data.IsLookAtObstacle.Value;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerClimbToObstacle>();
    }
}