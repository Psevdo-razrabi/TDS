using System;
using UnityEngine;

namespace FOW
{
    public class HiderDisableObjects : HiderBehavior
    {
        public GameObject[] ObjectsToHide;

        public override void OnHide()
        {
            foreach (GameObject o in ObjectsToHide)
                o.SetActive(false);
        }

        public override void OnReveal()
        {
            foreach (GameObject o in ObjectsToHide)
                o.SetActive(true);
        }

        public void ModifyHiddenObjects(GameObject[] newObjectsToHide)
        {
            OnReveal();
            ObjectsToHide = newObjectsToHide;
            if (!enabled)
                return;

            if (!IsEnabled)
                OnHide();
            else
                OnReveal();
        }
    }
}