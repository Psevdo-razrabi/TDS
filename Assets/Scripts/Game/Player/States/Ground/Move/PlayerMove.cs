using Game.Player.PlayerStateMashine;
using Game.Player.States.Orientation;
using Game.Player.States.StateHandle;
using Game.Player.States.StateHandle.Faling;
using Game.Player.States.StateHandle.Parkour;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerMove : PlayerOrientation
    {
        public PlayerMove(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Data.IsMove.Value = true;
            Data.IsLockAim = false;
            PlayerMoveConfig = Player.PlayerConfigs.BaseMove;
            Debug.Log("Вход в move state");
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsMove.Value = false;
            Debug.Log("Выход из move state");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            
            Move();
            GravityForce();
            
            Debug.Log("обновляю ходьбу без прицела");
            ChangeState();
        }

        private void ChangeState()
        {
            Player.StateChain.HandleState<PlayerIdleHandler>();
            Player.StateChain.HandleState<PlayerAimMoveHandler>();
            Player.StateChain.HandleState<PlayerDashHandle>();
            Player.StateChain.HandleState<PlayerSitDownCrouchHandle>();
            Player.StateChain.HandleState<PlayerClimbToObstacleHandle>();
            Player.StateChain.HandleState<PlayerFallingHandler>();
        }
    }
}