using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.Orientation;
using Game.Player.States.StateHandle;
using UniRx;
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
            Data.GetValue<ReactiveProperty<bool>>(Name.IsMove).Value = true;
            Data.SetValue(Name.PlayerMoveConfig, Player.PlayerConfigs.MovementConfigsProvider.MoveWithAim);
        }

        public override void OnExit()
        {
            base.OnExit();
            if(Data.GetValue<ReactiveProperty<bool>>(Name.IsCrouch).Value || Data.GetValue<ReactiveProperty<bool>>(Name.IsDashing).Value) 
                OnExitAimState();
            Data.GetValue<ReactiveProperty<bool>>(Name.IsMove).Value = false;
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