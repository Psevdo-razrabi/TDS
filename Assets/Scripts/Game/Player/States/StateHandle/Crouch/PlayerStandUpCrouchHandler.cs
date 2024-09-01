using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Crouching;
using UniRx;

namespace Game.Player.States.StateHandle
{
    public class PlayerStandUpCrouchHandler : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerStandUpCrouchHandler(PlayerStateMachine stateMachine) => StateMachine = stateMachine;

        public bool CanHandle() => StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsAim).Value 
                                   || StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsDashing).Value 
                                   || !StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsCrouch).Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerStandUp>();
    }
}