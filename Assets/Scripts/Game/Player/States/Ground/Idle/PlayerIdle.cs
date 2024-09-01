using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using Game.Player.States.StateHandle.Faling;
using Game.Player.States.StateHandle.Parkour;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerIdle : BaseIdle
    {
        public PlayerIdle(PlayerStateMachine stateMachine) 
            : base(stateMachine)
        {
            
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Data.GetValue<ReactiveProperty<bool>>(Name.IsMove).Value = false;
            Data.SetValue(Name.IsLockAim, false);
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
            ChangeState();
        }
        
        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerMoveHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerAimIdleHandler>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerSitDownCrouchHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerClimbToObstacleHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerFallingHandler>();
        }
    }
}