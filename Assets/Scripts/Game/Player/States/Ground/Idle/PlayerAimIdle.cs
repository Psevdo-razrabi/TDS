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
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
            Player.StateChain.HandleState<PlayerAimMoveHandler>();
            Player.StateChain.HandleState<PlayerSitDownCrouchHandle>();
        }
    }
}