using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Player.Weapons
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        [SerializeField] private List<WeaponStats>_weaponStats = new List<WeaponStats>();
        public List<WeaponStats> WeaponStats => _weaponStats;
        public int CurrentLevel => _currentLevel;
        
        [Inject] private DiContainer _container;
        private int _currentLevel = 1;
        private int _maxLevel = 5;
        

        private void Awake() => _container.Inject(this);
        

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                float damage = Random.Range(_weaponStats[_currentLevel - 1].Damage / 2f,
                    _weaponStats[_currentLevel - 1].Damage * 1.5f);
                enemy.TakeDamage(damage);
            }
        }

        public virtual void Shoot()
        {
            
        }

        public virtual void LevelUp()
        {
            if (_currentLevel < _maxLevel) 
                _currentLevel++;
        }
        
    }
}