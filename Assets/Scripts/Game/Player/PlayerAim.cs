using System;
using Input.Interface;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Camera _camera;

        private Vector3 _lookPosition;
        private IMouse _mousePosition;
        private Vector3 _mouse;
        private readonly CompositeDisposable _compositeDisposable = new();

        [Inject]
        private void Construct(IMouse mouse)
        {
            _mousePosition = mouse;
            _mousePosition.PositionMouse
                .Subscribe(vector => _mouse = vector)
                .AddTo(_compositeDisposable);
        }
        
        private void Update() => Aim();

        private (bool, Vector3) GetMousePosition()
        {
            Ray ray = _camera.ScreenPointToRay(_mouse);
            return Physics.Raycast(ray, out var hit, 100f, _ground) ? (true, hit.point) : (false, Vector3.zero);
        }

        private void Aim()
        {
            (bool success, Vector3 position) = GetMousePosition();
            if (success)
            {
                Vector3 direction = position - transform.position;
                direction.y = 0f;
                var rotation = Quaternion.LookRotation(direction);
                transform.forward = Vector3.Lerp(transform.forward, direction, 5f * Time.deltaTime);
            }
        }

        private void OnDisable()
        {
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }
    }
}