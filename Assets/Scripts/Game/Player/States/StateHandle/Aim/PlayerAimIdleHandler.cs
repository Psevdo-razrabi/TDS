using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using UniRx;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public class PlayerAimIdleHandler : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }

        public PlayerAimIdleHandler(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsInputZero() && StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsAim).Value
                                                                   && !StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsDashing).Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerAimIdle>();
    }
}