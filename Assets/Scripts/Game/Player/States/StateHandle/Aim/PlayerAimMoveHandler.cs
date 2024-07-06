using Game.Player.PlayerStateMashine;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public class PlayerAimMoveHandler : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; set; }
        
        public PlayerAimMoveHandler(InitializationStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => !StateMachine.Data.IsInputZero() && StateMachine.Data.IsAim && !StateMachine.Data.IsDashing.Value;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerMoveInAim>();
    }
}