using System;
using UnityEngine;

namespace Game.Player.AnyScripts
{
    public class LandingColliders : MonoBehaviour
    {
        [SerializeField] private Collider[] triggerColliders;

        private void Awake()
        {
            ActiveColliders(false);
        }


        public void ActiveColliders(bool isActive)
        {
            foreach (var triggerCollider in triggerColliders)
            {
                triggerCollider.gameObject.SetActive(isActive);
            }
        }
    }
}