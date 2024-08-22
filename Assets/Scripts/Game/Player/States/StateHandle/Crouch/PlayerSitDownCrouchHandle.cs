using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Crouching;

namespace Game.Player.States.StateHandle
{
    public class PlayerSitDownCrouchHandle : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerSitDownCrouchHandle(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsCrouch.Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerSitsDown>();
    }
}