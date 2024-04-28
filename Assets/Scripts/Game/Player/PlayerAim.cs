using System;
using Game.Player.Interfaces;
using Input.Interface;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerAim : MonoBehaviour, IPlayerAim
    {
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Camera _camera;
        [SerializeField] private Crosshair _crosshair;

        private Vector3 _lookPosition;
        private IMouse _mousePosition;
        private Vector3 _mouse;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        public (bool, Vector3) GetMousePosition()
        {
            Ray ray = _camera.ScreenPointToRay(_crosshair.CrossHair.position);
            return Physics.Raycast(ray, out var hit, 100f, _ground) ? (true, hit.point) : (false, Vector3.zero);
        }

        public void Aim()
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