using System;
using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Input.Interface;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerAim : MonoBehaviour, ICameraProvider, IPlayerAim
    {
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Camera _camera;
        [SerializeField] private Crosshair _crosshair;
        
        private StateMachineData _stateMachineData;

        public Transform CameraTransform => _camera.transform;
        
        [Inject]
        private void Construct(StateMachineData stateMachineData)
        {
            _stateMachineData = stateMachineData;
        }

        public (bool, Vector3) GetMousePosition()
        {
            var directionCrosshair = new Vector2((_crosshair.transform.position.x - transform.position.x) / Screen.width * 2 - 1, (_crosshair.transform.position.y - transform.position.y) / Screen.height * 2 - 1);

            _stateMachineData.MouseDirection =
                new Vector2(Mathf.Clamp(directionCrosshair.x, -1, 1), Mathf.Clamp(directionCrosshair.y, -1, 1));
            
            Ray ray = _camera.ScreenPointToRay(_crosshair.transform.position);
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
    }
}