using UnityEngine;

namespace Game
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        private float _smoothTime = 0.3f;
        private Vector3 _velocity = Vector3.zero;

        private void Update() => 
            transform.position = Vector3.SmoothDamp(transform.position, _target.position + _offset, ref _velocity, _smoothTime);
    }
}