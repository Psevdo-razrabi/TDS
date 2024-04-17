using Game.Player.PlayerStateMashine;

namespace Game.Player.States
{
    public class BaseIdle : GroundState
    {
        public BaseIdle(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }
    }
}