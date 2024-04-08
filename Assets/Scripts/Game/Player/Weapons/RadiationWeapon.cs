using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Enemy;
using UnityEngine;

namespace Game.Player.Weapons
{
    public class RadiationWeapon : BaseWeapon, IActivatable
    {
        [SerializeField] private Transform _plane;
        [SerializeField] private SphereCollider _collider;
        private List<EnemyHealth> _enemiesInZone  = new List<EnemyHealth>();
        private WaitForSeconds _timeBetweenAttack;
        private Coroutine _radiationRoutine;

        private void Start()
        {
            Setup();
            Activate();
        }

        private void OnEnable() => Setup();

        public void RemoveEnemy(EnemyHealth enemyHealth) => _enemiesInZone.Remove(enemyHealth);

        private void Setup()
        {
            _timeBetweenAttack = new WaitForSeconds(WeaponStats[CurrentLevel - 1].TimeBetweenAttack);
            _collider.radius = WeaponStats[CurrentLevel - 1].Range;
            _plane.localScale = Vector3.one * WeaponStats[CurrentLevel - 1].Range / 4.5f;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out EnemyHealth enemy)) 
                _enemiesInZone.Add(enemy);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out EnemyHealth enemy)) 
               RemoveEnemy(enemy);
        }

        public void Activate() => _radiationRoutine = StartCoroutine(CheckZone());

        public void Deactivate() => StopCoroutine(_radiationRoutine);

        public override void LevelUp()
        {
            base.LevelUp();
            Setup();
        }

        private IEnumerator CheckZone()
        {
            while (true)
            {
                if (_enemiesInZone.Count > 0)
                {
                    for (int i = 0; i < _enemiesInZone.Count; i++)
                    {
                        _enemiesInZone[i].TakeDamage(WeaponStats[CurrentLevel - 1].Damage);
                    }
                }
                yield return _timeBetweenAttack;
            }   
        }
    }
}