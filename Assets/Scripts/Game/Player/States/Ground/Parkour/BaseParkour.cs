using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using UnityEngine;

namespace Game.Player.States.Parkour
{
    public abstract class BaseParkour : GroundState
    {
        protected BaseParkour(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }
        
        protected void MatchAnimatorState(string stateName, AvatarTarget target, Vector3 weightMask, float start, float end)
        {
            if (Player.PlayerAnimation.AnimatorController.PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                Player.PlayerAnimation.AnimatorController.PlayerAnimator
                    .MatchTarget(TargetPosition, Player.PlayerComponents.transform.rotation, target, new MatchTargetWeightMask(weightMask, 0),
                        start, end);
            }
        }
    }
}