using UnityEngine;

namespace Game.Player.AnimatorScripts
{
    public class AnimatorController : MonoBehaviour
    {
        [field: SerializeField] public Animator PlayerAnimator { get; private set; }
        [field: SerializeField] public string NameHorizontalParameters { get; private set; }
        [field: SerializeField] public string NameVerticalParameters { get; private set; }
        [field: SerializeField] public string NameMoveParameters { get; private set; }
        [field: SerializeField] public string NameJumpParameters { get; private set; }
        [field: SerializeField] public string NameDashParameters { get; private set; }
        [field: SerializeField] public string NameAimParameters { get; private set; }


        public void SetInputParameters(float horizontal, float vertical)
        {
            PlayerAnimator.SetFloat(NameHorizontalParameters, horizontal);
            PlayerAnimator.SetFloat(NameVerticalParameters, vertical);
        }

        public void SetBoolParameters(string nameParameters , bool isBool) => PlayerAnimator.SetBool(nameParameters, isBool);

        public void SetTriggerParameters(string nameParameters) => PlayerAnimator.SetTrigger(nameParameters);
    }
}