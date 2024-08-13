using Game.Player.PlayerStateMashine;
using Game.Player.States.Orientation;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerMoveInAim : PlayerOrientation
    {
        public PlayerMoveInAim(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //OnEnterAimState();
            Data.IsMove.Value = true;
            PlayerMoveConfig = Player.PlayerConfigs.MoveWithAim;
            Debug.Log("зашел в ходьбу в прицеле");
        }

        public override void OnExit()
        {
            base.OnExit();
            if(Data.IsCrouch.Value || Data.IsDashing.Value) 
                OnExitAimState();
            Data.IsMove.Value = false;
            Debug.Log("вышел из ходьбы в прицеле");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            Move();
            Debug.Log("обновляю ходьбу в прицеле");
            ChangeState();
        }

        private void ChangeState()
        {
            Player.StateChain.HandleState<PlayerAimIdleHandler>();
            Player.StateChain.HandleState<PlayerMoveHandler>();
            Player.StateChain.HandleState<PlayerDashHandle>();
            Player.StateChain.HandleState<PlayerSitDownCrouchHandle>();
        }
    }
}