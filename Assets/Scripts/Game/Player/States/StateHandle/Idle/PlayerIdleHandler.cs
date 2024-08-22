using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public class PlayerIdleHandler : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        
        public PlayerIdleHandler(PlayerStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => StateMachine.Data.IsInputZero() && !StateMachine.Data.IsAim.Value && !StateMachine.Data.IsDashing.Value && StateMachine.Data.IsGrounded.Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerIdle>();
    }
}