using Game.Player.PlayerStateMashine;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public class PlayerIdleHandler : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; set; }
        
        public PlayerIdleHandler(InitializationStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => StateMachine.Data.IsInputZero() && !StateMachine.Data.IsAim.Value && !StateMachine.Data.IsDashing.Value && StateMachine.Data.IsGrounded.Value;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerIdle>();
    }
}