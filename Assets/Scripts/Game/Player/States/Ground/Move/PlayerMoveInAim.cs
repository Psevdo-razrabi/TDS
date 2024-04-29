using Game.Player.PlayerStateMashine;
using Game.Player.States.Dash;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States
{
    public class PlayerMoveInAim : BaseMove
    {
        public PlayerMoveInAim(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            OnAnimatorStateSet(ref Data.IsMove, true, Player.AnimatorController.NameMoveParameter);
            Debug.Log("зашел в ходьбу в прицеле");
        }

        public override void OnExit()
        {
            base.OnExit();
            OnAnimatorStateSet(ref Data.IsMove, false, Player.AnimatorController.NameMoveParameter);
            Debug.Log("вышел из ходьбы в прицеле");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            
            Move();
            
            Debug.Log("обновляю ходьбу в прицеле");
            
            UpdateDesiredTargetSpeed(Player.PlayerConfigs.MoveWithAim);
            
            Player.StateChain.HandleState<PlayerAimIdleHandler>();
        }
    }
}