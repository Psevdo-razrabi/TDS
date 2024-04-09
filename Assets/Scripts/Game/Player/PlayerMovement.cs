using System;
using Input.Interface;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _forwardSpeed, _backwardSpeed, _towardSpeed;
        [SerializeField] private Animator _animator;
        private Vector3 _movement;
        private IMove _move;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        [Inject]
        private void Construct(IMove move)
        {
           _move = move;
           _move.Move
              .Subscribe(vector => _movement = vector)
              .AddTo(_compositeDisposable);
        }

        private void Update()
        {
           Move();
           Debug.Log(_movement);
        }

        private void Move()
     {
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
     }
        private void AnimatorSetState(string name, float value) => _animator.SetFloat(name, value);

        private void OnDisable()
        {
           _compositeDisposable.Clear();
           _compositeDisposable.Dispose();
        }
    }
}
