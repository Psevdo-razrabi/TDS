using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerAimIdle : BaseIdle
    {
        public PlayerAimIdle(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void OnExit()
        {
            base.OnExit();
            if(Data.GetValue<ReactiveProperty<bool>>(Name.IsCrouch).Value || Data.GetValue<ReactiveProperty<bool>>(Name.IsDashing).Value) 
                OnExitAimState();
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            ChangeState();
        }

        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerAimMoveHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerIdleHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerSitDownCrouchHandle>();
        }
    }
}