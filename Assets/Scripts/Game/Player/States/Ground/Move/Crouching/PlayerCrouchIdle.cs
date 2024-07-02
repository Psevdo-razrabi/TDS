using Game.Player.PlayerStateMashine;

namespace Game.Player.States.Crouching
{
    public class PlayerCrouchIdle : PlayerSitsDown
    {
        public PlayerCrouchIdle(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            Player.StateChain.HandleState();
        }
    }
}