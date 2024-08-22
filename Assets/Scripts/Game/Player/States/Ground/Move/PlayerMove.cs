using Game.Player.AnyScripts;
using Game.Player.States.Orientation;
using Game.Player.States.StateHandle;
using Game.Player.States.StateHandle.Faling;
using Game.Player.States.StateHandle.Parkour;

namespace Game.Player.States
{
    public class PlayerMove : PlayerOrientation
    {
        public PlayerMove(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Data.IsMove.Value = true;
            Data.IsLockAim = false;
            Data.PlayerMoveConfig = Player.PlayerConfigs.MovementConfigsProvider.BaseMove;
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsMove.Value = false;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            
            Move();
            GravityForce();
            
            ChangeState();
        }

        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerIdleHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerAimMoveHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerDashHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerSitDownCrouchHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerClimbToObstacleHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerFallingHandler>();
        }
    }
}