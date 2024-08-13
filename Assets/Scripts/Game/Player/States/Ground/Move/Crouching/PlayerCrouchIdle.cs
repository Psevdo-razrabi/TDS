using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class PlayerCrouchIdle : BaseCrouching
    {
        public PlayerCrouchIdle(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("вошел в crouchIdle");
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("вышел из crouchIdle");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            Debug.Log("обновляю crouchIdle");
            ChangeState();
        }

        private void ChangeState()
        {
            Player.StateChain.HandleState<PlayerMoveCrouchHandle>();
            Player.StateChain.HandleState<PlayerStandUpCrouchHandler>();
        }
    }
}