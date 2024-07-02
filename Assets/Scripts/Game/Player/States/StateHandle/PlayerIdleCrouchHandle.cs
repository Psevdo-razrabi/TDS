using Game.Player.PlayerStateMashine;
using Game.Player.States.Crouching;

namespace Game.Player.States.StateHandle
{
    public class PlayerIdleCrouchHandle : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; }
        public PlayerIdleCrouchHandle(InitializationStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsPlayerSitDown && StateMachine.Data.IsInputZeroX();

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerCrouchIdle>();
    }
}