using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Parkour;

namespace Game.Player.States.StateHandle.Parkour
{
    public class PlayerClimbToObstacleHandle : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        
        public PlayerClimbToObstacleHandle(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsClimbing.Value && StateMachine.Data.IsLookAtObstacle.Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerClimbToObstacle>();
    }
}