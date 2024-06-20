using UnityEngine;

namespace FOW
{
    public class HiderDisableRenderers : HiderBehavior
    {
        public Renderer[] ObjectsToHide;

        public override void OnHide()
        {
            foreach (Renderer renderer in ObjectsToHide)
                renderer.enabled = false;
        }

        public override void OnReveal()
        {
            foreach (Renderer renderer in ObjectsToHide)
                renderer.enabled = true;
        }
    }
}