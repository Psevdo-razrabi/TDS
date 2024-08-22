using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Orientation;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerMoveInAim : PlayerOrientation
    {
        public PlayerMoveInAim(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //OnEnterAimState();
            Data.IsMove.Value = true;
            Data.PlayerMoveConfig = Player.PlayerConfigs.MovementConfigsProvider.MoveWithAim;
        }

        public override void OnExit()
        {
            base.OnExit();
            if(Data.IsCrouch.Value || Data.IsDashing.Value) 
                OnExitAimState();
            Data.IsMove.Value = false;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            Move();
            ChangeState();
        }

        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerAimIdleHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerMoveHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerDashHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerSitDownCrouchHandle>();
        }
    }
}