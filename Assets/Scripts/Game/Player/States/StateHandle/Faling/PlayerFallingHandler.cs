
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Air;
using UniRx;

namespace Game.Player.States.StateHandle.Faling
{
    public class PlayerFallingHandler : IStateHandle
    {
        public PlayerStateMachine StateMachine { get; private set; }
        
        public PlayerFallingHandler(PlayerStateMachine stateMachine) => StateMachine = stateMachine;
        
        public bool CanHandle() => StateMachine.Data.GetValue<bool>(Name.IsPlayerInObstacle) && !StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsLookAtObstacle).Value 
                                                                        && !StateMachine.Data.GetValue<ReactiveProperty<bool>>(Name.IsGrounded).Value;

        public void Handle() => StateMachine.StateMachine.SwitchStates<PlayerFalling>();
    }
}