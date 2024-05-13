using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Customs
{
    public class RagdollHelper : MonoBehaviour
    {
        private IReadOnlyList<Rigidbody> _rigidbodies;
        
        public void SetNotActive()
        {
            _rigidbodies.ForEach(x => x.isKinematic = true);
        }

        public void SetActive()
        {
            _rigidbodies.ForEach(x => x.isKinematic = false);
        }

        private void Start()
        {
            _rigidbodies = new List<Rigidbody>(gameObject.GetComponentsInChildren<Rigidbody>());
            SetNotActive();
        }
    }
}