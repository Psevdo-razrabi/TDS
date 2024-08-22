using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public class PlayerAimMoveHandler : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        
        public PlayerAimMoveHandler(PlayerStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => !StateMachine.Data.IsInputZero() && StateMachine.Data.IsAim.Value && !StateMachine.Data.IsDashing.Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerMoveInAim>();
    }
}