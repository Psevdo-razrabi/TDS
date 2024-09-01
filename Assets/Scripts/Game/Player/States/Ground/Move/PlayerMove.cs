using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Orientation;
using Game.Player.States.StateHandle;
using Game.Player.States.StateHandle.Faling;
using Game.Player.States.StateHandle.Parkour;
using UniRx;

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
            Data.GetValue<ReactiveProperty<bool>>(Name.IsMove).Value = true;
            Data.SetValue(Name.IsLockAim, false);
            Data.SetValue(Name.PlayerMoveConfig, Player.PlayerConfigs.MovementConfigsProvider.BaseMove);
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.GetValue<ReactiveProperty<bool>>(Name.IsMove).Value = false;
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