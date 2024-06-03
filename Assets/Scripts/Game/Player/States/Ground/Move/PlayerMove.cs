using Game.Player.PlayerStateMashine;
using Game.Player.States.Orientation;
using Game.Player.States.StateHandle;
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
            OnAnimatorStateSet(ref Data.IsMove, true, Player.AnimatorController.NameMoveParameter);
            Debug.Log("Вход в move state");
        }

        public override void OnExit()
        {
            base.OnExit();
            OnAnimatorStateSet(ref Data.IsMove, false, Player.AnimatorController.NameMoveParameter);
            Debug.Log("Выход из move state");
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            
            Move();
            
            Debug.Log("обновляю ходьбу без прицела");
            
            UpdateDesiredTargetSpeed(Player.PlayerConfigs.BaseMove);
            
            Player.StateChain.HandleState<PlayerIdleHandler>();
        }
    }
}