using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
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
            Player.StateChain.HandleState<PlayerMoveHandler>();
            Player.StateChain.HandleState<PlayerSitDownCrouchHandle>();
        }
    }
}