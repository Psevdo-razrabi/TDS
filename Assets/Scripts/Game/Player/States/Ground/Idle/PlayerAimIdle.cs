using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerAimIdle : BaseIdle
    {
        public PlayerAimIdle(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //OnEnterAimState();
            Debug.Log("Вход в idleAim state");
        }

        public override void OnExit()
        {
            base.OnExit();
            if(Data.IsCrouch.Value || Data.IsDashing.Value) 
                OnExitAimState();
            Debug.Log("Выход из idleAim state");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            Debug.Log("обновляю idle"); ;
            ChangeState();
        }

        private void ChangeState()
        {
            Player.StateChain.HandleState<PlayerAimMoveHandler>();
            Player.StateChain.HandleState<PlayerIdleHandler>();
            Player.StateChain.HandleState<PlayerSitDownCrouchHandle>();
        }
    }
}