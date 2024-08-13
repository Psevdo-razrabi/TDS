using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using Game.Player.States.StateHandle.Faling;
using Game.Player.States.StateHandle.Parkour;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerIdle : BaseIdle
    {
        public PlayerIdle(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) 
            : base(stateMachine, player, stateMachineData)
        {
            
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Data.IsMove.Value = false;
            Data.IsLockAim = false;
            Debug.Log("Вход в idle state");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Выход из idle state");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
            Debug.Log("обновляю idle");
            ChangeState();
        }
        
        private void ChangeState()
        {
            Player.StateChain.HandleState<PlayerMoveHandler>();
            Player.StateChain.HandleState<PlayerAimIdleHandler>();
            Player.StateChain.HandleState<PlayerSitDownCrouchHandle>();
            Player.StateChain.HandleState<PlayerClimbToObstacleHandle>();
            Player.StateChain.HandleState<PlayerFallingHandler>();
        }
    }
}