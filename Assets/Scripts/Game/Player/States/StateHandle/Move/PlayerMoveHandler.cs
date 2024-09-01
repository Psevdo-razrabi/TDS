using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using UniRx;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public class PlayerMoveHandler : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        
        public PlayerMoveHandler(PlayerStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => !StateMachine.Data.IsInputZero() && !StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsAim).Value 
                                                                    && !StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsDashing).Value 
                                                                    && StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsGrounded).Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerMove>();
    }
}