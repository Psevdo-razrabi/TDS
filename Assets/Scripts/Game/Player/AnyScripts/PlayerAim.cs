using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerAim : MonoBehaviour, ICameraProvider, IPlayerAim
    {
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Camera _camera;
        [SerializeField] private Crosshair _crosshair;
        [SerializeField] private GameObject _gun;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Player _player;
        
        private StateMachineData _stateMachineData;

        public Transform CameraTransform => _camera.transform;

        [Inject]
        private void Construct(StateMachineData stateMachineData)
        {
            _stateMachineData = stateMachineData;
        }
        
        
        public (bool, Vector3) GetMousePosition()
        {
            Vector3 screenPosition = new Vector3(_crosshair.Center.position.x, _crosshair.Center.position.y, 0f);
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            
            if (Physics.Raycast(ray, out var hit, 100f, _ground))
            {
                return (true, hit.point);
            }

            return (true, ray.GetPoint(100f));
        }

        public void Aim()
        {
            (bool success, Vector3 targetPosition) = GetMousePosition();
            if (success)
            {
                Vector3 gunPosition = _gun.transform.position;
                
                Vector3 directionToTarget = (targetPosition - gunPosition).normalized;
                
                Vector3 flatDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z).normalized;
                
                if (flatDirection != Vector3.zero)
                {
                    Vector3 lookPosition = transform.position + flatDirection;
                    transform.LookAt(new Vector3(lookPosition.x, transform.position.y, lookPosition.z));
                }
                
                _gun.transform.rotation = Quaternion.LookRotation(directionToTarget);
                
                DebugAimLine(gunPosition, directionToTarget);
            }
        }
        
        public void FreezeAim(Quaternion rotation)
        {
            _player.transform.rotation = rotation;
        }
        
        private void DebugAimLine(Vector3 origin, Vector3 direction)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, origin);
            _lineRenderer.SetPosition(1, origin + direction * 100f);
        }
    }
}
