using Game.Player.AnyScripts;
using Game.Player.States.StateHandle;

namespace Game.Player.States.Crouching
{
    public class PlayerCrouchIdle : BaseCrouching
    {
        public PlayerCrouchIdle(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }
        
        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            ChangeState();
        }

        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerMoveCrouchHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerStandUpCrouchHandler>();
        }
    }
}