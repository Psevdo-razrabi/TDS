using System;
using UnityEngine;

namespace Game.Player.Weapons
{
    [CreateAssetMenu(menuName = "Weapon HealthStats", fileName = "WeaponStats")]
    public class WeaponStats : ScriptableObject
    {
        [SerializeField] private float _speed, _damage, _range, _timeBetweenAttack;

        public float Speed => _speed;
        public float Damage => _damage;
        public float Range => _range;
        public float TimeBetweenAttack => _timeBetweenAttack;
    }
}