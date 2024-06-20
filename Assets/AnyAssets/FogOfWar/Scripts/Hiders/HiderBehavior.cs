using System;
using UnityEngine;

namespace FOW
{
    [RequireComponent(typeof(FogOfWarHider))]
    public abstract class HiderBehavior : MonoBehaviour
    {
        protected bool IsEnabled;
        private void Awake()
        {
            GetComponent<FogOfWarHider>().OnActiveChanged += OnStatusChanged;
        }

        private void OnStatusChanged(bool isEnabled)
        {
            IsEnabled = isEnabled;
            if (isEnabled)
                OnReveal();
            else
                OnHide();
        }

        public abstract void OnReveal();
        public abstract void OnHide();
    }
}
