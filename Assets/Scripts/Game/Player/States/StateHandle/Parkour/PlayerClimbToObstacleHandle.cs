using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Parkour;
using UniRx;

namespace Game.Player.States.StateHandle.Parkour
{
    public class PlayerClimbToObstacleHandle : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        
        public PlayerClimbToObstacleHandle(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsClimbing).Value && StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsLookAtObstacle).Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerClimbToObstacle>();
    }
}