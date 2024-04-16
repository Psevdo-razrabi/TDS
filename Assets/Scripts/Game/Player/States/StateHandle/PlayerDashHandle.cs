using Game.Player.PlayerStateMashine;
using Game.Player.States.Dash;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public class PlayerDashHandle : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; set; }
        
        public PlayerDashHandle(InitializationStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => !StateMachine.Data.IsInputZero() && StateMachine.Data.IsDashing;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerDash>();
    }
}