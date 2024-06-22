        using UniRx;
        using UnityEngine;

namespace Game.Player.AnimatorScripts
{
    public class AnimatorController : MonoBehaviour
    {
        [field: SerializeField] public Animator PlayerAnimator { get; private set; }
        [field: SerializeField] public string NameHorizontalParameter { get; private set; }
        [field: SerializeField] public string NameVerticalParameter { get; private set; }
        [field: SerializeField] public string NameMoveParameter { get; private set; }
        [field: SerializeField] public string NameJumpParameter { get; private set; }
        [field: SerializeField] public string NameDashParameter { get; private set; }
        [field: SerializeField] public string NameAimParameter { get; private set; }
        [field: SerializeField] public string NameRemappedLateralSpeedNormalizedParameter { get; private set; }
        [field: SerializeField] public string NameRemappedForwardSpeedNormalizedParameter { get; private set; }
        [field: SerializeField] public string NameRemappedSpeedNormalizedParameter { get; private set; }


        public void SetFloatParameters(string nameParameters, float value)
        {
            PlayerAnimator.SetFloat(nameParameters, value);
        }

        public void SetBoolParameters(string nameParameters, bool isBool) => PlayerAnimator.SetBool(nameParameters, isBool);

        public void SetTriggerParameters(string nameParameters) => PlayerAnimator.SetTrigger(nameParameters);
        
        public void OnAnimatorStateSet(ref bool parameters, bool state, string nameStateAnimator)
        {
            parameters = state;
            SetBoolParameters(nameStateAnimator, state);
        }

        public void OnAnimatorStateSet(ReactiveProperty<bool> parameters, bool state, string nameStateAnimator)
        {
            parameters.Value = state;
            SetBoolParameters(nameStateAnimator, parameters.Value);
        }
    }
}