using System;
using UnityEngine;

namespace Game.Core.Health
{
    public abstract class ObjectHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;

        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        private void OnEnable() => _currentHealth = _maxHealth;
        
        public virtual void TakeDamage(float damage)
        {
            if(damage <=0)
                throw new ArgumentOutOfRangeException(nameof(damage));
            _currentHealth -= damage;
        }

        public virtual void TakeHeal(float heal)
        {
            if(heal <=0)
                throw new ArgumentOutOfRangeException(nameof(heal));
            _currentHealth += heal;
            if (_currentHealth > _maxHealth)
                _currentHealth = _maxHealth;
        }
    }
}