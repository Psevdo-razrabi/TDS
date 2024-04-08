using UnityEngine;

namespace Game.Player
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private LayerMask _ground;
        [SerializeField] private Camera _camera;

        private Vector3 _lookPosition;
        
        private void Update() => Aim();

        private (bool, Vector3) GetMousePosition()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
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
    }
}