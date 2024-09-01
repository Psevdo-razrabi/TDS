using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Dash;
using UniRx;

namespace Game.Player.States.StateHandle
{
    public class PlayerDashHandle : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }

        public PlayerDashHandle(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.IsInputZero() == false && StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsDashing).Value &&
                                   StateMachine.Data.DashCount != 0 && StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsCrouch).Value == false;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerDash>();

    }
}