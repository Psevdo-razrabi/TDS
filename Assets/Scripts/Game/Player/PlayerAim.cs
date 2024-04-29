using System;
using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
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

        private Vector3 _lookPosition;
        private IMouse _mousePosition;
        private Vector3 _mouse;
        private readonly CompositeDisposable _compositeDisposable = new();
        private StateMachineData _stateMachineData;

        [Inject]
        private void Construct(IMouse mouse, StateMachineData stateMachineData)
        {
            _mousePosition = mouse;
            _mousePosition.PositionMouse
                .Subscribe(vector => _mouse = vector)
                .AddTo(_compositeDisposable);
            _stateMachineData = stateMachineData;
        }

        public (bool, Vector3) GetMousePosition()
        {
            //var directionToMouseX = (_mouse.x - transform.position.x) / Screen.width * 2 - 1;
            //var directionToMouseY = (_mouse.y - transform.position.y) / Screen.height * 2 - 1;
            
            //Debug.Log(new Vector2(directionToMouseX, directionToMouseY));
            
            var directionMouse = new Vector2((_mouse.x - transform.position.x) / Screen.width * 2 - 1, (_mouse.y - transform.position.y) / Screen.height * 2 - 1);

            _stateMachineData.MouseDirection =
                new Vector2(Mathf.Clamp(directionMouse.x, -1, 1), Mathf.Clamp(directionMouse.y, -1, 1));
            
            Ray ray = _camera.ScreenPointToRay(_mouse);
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
                transform.rotation = rotation;
            }
        }

        private void OnDisable()
        {
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }
    }
}