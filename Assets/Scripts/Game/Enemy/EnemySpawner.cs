using System;
using System.Collections;
using Game.Core;
using Game.Player;
using UnityEngine;
using Zenject;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnInterval;
        [SerializeField] private Transform _minPos, _maxPos, _enemyContainer;
        [SerializeField] private ObjectPool _enemyPool;
        private PlayerMovement _playerMovement;
        private RandomSpawnPoint _randomSpawnPoint;
        private WaitForSeconds _interval;
        private Coroutine _spawnRoutine;


        private void Awake()
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject enemy = _enemyPool.Create();
                enemy.transform.SetParent(_enemyContainer);
            }
            Activate();
        }
        private void Activate() => _spawnRoutine = StartCoroutine(Spawn());

        private void Deactivate()
        {
            if(_spawnRoutine != null)
                StopCoroutine(_spawnRoutine);
        }

        private void Start() => _interval = new WaitForSeconds(_spawnInterval);

        private IEnumerator Spawn()
        {
            while (true)
            {
                transform.position = _playerMovement.transform.position;
                GameObject enemy = _enemyPool.GetFromPool();
                enemy.transform.SetParent(_enemyContainer);
                enemy.transform.position = _randomSpawnPoint.GetRandomPoint(_minPos, _maxPos);
                yield return _interval;
            }
        }

        [Inject] private void Construct(RandomSpawnPoint point, PlayerMovement player)
        {
            _randomSpawnPoint = point;
            _playerMovement = player;
        }
    }
}