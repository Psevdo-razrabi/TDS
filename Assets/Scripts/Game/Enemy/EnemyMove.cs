using System.Collections;
using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Enemy
{
    public class EnemyMove : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _freezeTimer;
        private PlayerMovement _playerMovement;
        private Vector3 _direction;
        private float _initialMoveSpeed;
        private WaitForSeconds _timer;

        private void Update() => MoveToPlayer();

        private void Start()
        {
            _initialMoveSpeed = _moveSpeed;
            _timer = new WaitForSeconds(_freezeTimer);
        }

        private void MoveToPlayer()
        {
            _direction = (_playerMovement.transform.position - transform.position).normalized;
            transform.position += _direction * (_moveSpeed * Time.deltaTime);
            transform.forward = Vector3.Distance(transform.position,_playerMovement.transform.position) > 0.01f ? _direction : transform.forward;
        }

        public void Freeze()
        {
            if (gameObject.activeSelf)
                StartCoroutine(StartFreeze());
        }
        
        public void StopEnemy(bool value) => _moveSpeed = value ? 0f : _initialMoveSpeed;
        
        private IEnumerator StartFreeze()
        {
            _moveSpeed /= 2;
            yield return _timer;
            _moveSpeed = _initialMoveSpeed;
        }
        
        [Inject]
        private void Construct(PlayerMovement playerMovement) => _playerMovement = playerMovement;
    }
}