using UnityEngine;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed, _backwardSpeed, _towardSpeed;
        [SerializeField] private Animator _animator;
        [SerializeField] private WorldBounds _worldBounds;
        private Vector3 _movement;
        public Vector3 Movement => _movement;
   
        private void Update() => Move();

        private void Move()
       {
        _movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0);
        AnimatorSetState("Speed", 0);
        AnimatorSetState("Horizontal", _movement.x);
        AnimatorSetState("Vertical", _movement.y);

        if (_movement.x == 0 && _movement.y > 0)
        {
         transform.position += transform.forward.normalized * (_forwardSpeed * Time.deltaTime);
         AnimatorSetState("Speed", _movement.sqrMagnitude);
        }

        else if (_movement.x == 0 && _movement.y < 0)
        {
         transform.position += transform.forward.normalized * (-1 * (_backwardSpeed * Time.deltaTime));
         AnimatorSetState("Speed", _movement.sqrMagnitude);
        }

        else if (_movement.x > 0 && _movement.y == 0)
        {
         transform.position += transform.right.normalized  * (_towardSpeed * Time.deltaTime);
         AnimatorSetState("Speed", _movement.sqrMagnitude);
        }

        else if (_movement.x < 0 && _movement.y == 0)
        {
         transform.position += transform.right.normalized * (-1 * (_towardSpeed * Time.deltaTime));
         AnimatorSetState("Speed", _movement.sqrMagnitude);
        }
        else if (_movement.x > 0 && _movement.y > 0)
        {
         transform.position += (transform.forward + transform.right).normalized * (_towardSpeed * Time.deltaTime);
        AnimatorSetState("Speed", _movement.sqrMagnitude);
        }
        else if (_movement.x < 0 && _movement.y > 0)
        {
         transform.position += (transform.forward + transform.right * -1).normalized * (_towardSpeed * Time.deltaTime);
         AnimatorSetState("Speed", _movement.sqrMagnitude);
        }
        else if (_movement.x > 0 && _movement.y < 0)
        {
         transform.position += ((transform.forward * -1) + transform.right).normalized * (_backwardSpeed * Time.deltaTime);
         AnimatorSetState("Speed", _movement.sqrMagnitude);
        }
        else if (_movement.x < 0 && _movement.y < 0)
        {
         transform.position += ((transform.forward * -1) + (transform.right * -1)).normalized * (_backwardSpeed * Time.deltaTime);
         AnimatorSetState("Speed", _movement.sqrMagnitude);
        }
        _worldBounds.CheckBounds();
       }
        private void AnimatorSetState(string name, float value) => _animator.SetFloat(name, value);
         
        }
}
