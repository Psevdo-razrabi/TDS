using Game.Player.PlayerStateMashine;
using UnityEngine;

namespace Game.Player.States.Parkour
{
    public abstract class BaseParkour : GroundState
    {
        protected BaseParkour(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }
        
        protected void MatchAnimatorState(string stateName, AvatarTarget target, Vector3 weightMask, float start, float end)
        {
            if (Player.AnimatorController.PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                Player.AnimatorController.PlayerAnimator
                    .MatchTarget(TargetPosition, Player.transform.rotation, target, new MatchTargetWeightMask(weightMask, 0),
                        start, end);
            }
        }
    }
}