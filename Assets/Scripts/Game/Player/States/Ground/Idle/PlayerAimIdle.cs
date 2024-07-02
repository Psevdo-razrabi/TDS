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
            Debug.Log("Вход в idleAim state");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("Выход из idleAim state");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
            Player.StateChain.HandleState<PlayerAimMoveHandler>();
        }
    }
}