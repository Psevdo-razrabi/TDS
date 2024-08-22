using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Crouching;

namespace Game.Player.States.StateHandle
{
    public class PlayerIdleCrouchHandle : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleCrouchHandle(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsCrouch.Value && StateMachine.Data.IsInputZero();

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerCrouchIdle>();
    }
}