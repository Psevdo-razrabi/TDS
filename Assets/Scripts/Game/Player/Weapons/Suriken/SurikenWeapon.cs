using System.Collections;
using Game.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Player.Weapons.Suriken
{
    public class SurikenWeapon : BaseWeapon, IActivatable
    {
        [SerializeField] private ObjectPool _objectPool;
        [SerializeField] private Transform _container;
        [SerializeField] private LayerMask _layerMask;
        private WaitForSeconds _timeBetweenAttack;
        private Coroutine _attackRoutine;
        private Vector3 _direction;

        private void OnEnable()
        {
            Setup();
            Activate();
        }

        public void Activate() => _attackRoutine = StartCoroutine(Throw());

        public void Deactivate() => StopCoroutine(_attackRoutine);

        public override void LevelUp()
        {
            base.LevelUp();
            Setup();
        }

        private void Setup() => _timeBetweenAttack = new WaitForSeconds(WeaponStats[CurrentLevel-1].TimeBetweenAttack);

        private IEnumerator Throw()
        {
            while (true)
            {
                Collider[] enemiesInRange =
                    Physics.OverlapSphere(transform.position, WeaponStats[CurrentLevel - 1].Range, _layerMask);
                if (enemiesInRange.Length > 0)
                {
                    Vector3 targetPosition = enemiesInRange[Random.Range(0, enemiesInRange.Length)].transform.position;
                    _direction = targetPosition - transform.position;
                    GameObject suriken = _objectPool.GetFromPool();
                    suriken.transform.SetParent(_container);
                    suriken.transform.position = transform.position;
                    suriken.transform.forward = _direction.normalized;
                    yield return _timeBetweenAttack;
                }
                else
                    yield return _timeBetweenAttack;
            }
        }
    }
}