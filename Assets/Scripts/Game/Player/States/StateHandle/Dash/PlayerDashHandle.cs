using Game.Player.PlayerStateMashine;
using Game.Player.States.Dash;

namespace Game.Player.States.StateHandle
{
    public class PlayerDashHandle : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; }
        
        public PlayerDashHandle(InitializationStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsInputZero() == false && StateMachine.Data.IsDashing.Value &&
                                   StateMachine.Data.DashCount != 0 && StateMachine.Data.IsCrouch.Value == false;
    
        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerDash>();
    }
}