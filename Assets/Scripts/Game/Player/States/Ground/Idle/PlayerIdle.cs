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
        }

        public override void OnExit()
        {
            base.OnExit();
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