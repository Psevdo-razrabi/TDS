using Game.Player.PlayerStateMashine;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public class PlayerAimIdleHandler : IStateHandle
    {
        public InitializationStateMachine StateMachine { get; private set; }

        public PlayerAimIdleHandler(InitializationStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsInputZero() && StateMachine.Data.IsAim.Value && !StateMachine.Data.IsDashing.Value;

        public void Handle() => StateMachine.PlayerStateMachine.SwitchStates<PlayerAimIdle>();
    }
}