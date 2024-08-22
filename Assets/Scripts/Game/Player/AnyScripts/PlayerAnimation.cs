using Game.Player.AnimatorScripts;

namespace Game.Player.AnyScripts
{
    public class PlayerAnimation
    {
        public AnimatorController AnimatorController { get; private set; }
        
        public PlayerAnimation(AnimatorController animatorController)
        {
            AnimatorController = animatorController;
        }
    }
}