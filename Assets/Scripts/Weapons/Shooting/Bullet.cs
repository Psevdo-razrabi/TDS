using System;
using Game.Core.Health;
using Game.Player.Weapons.WeaponConfigs;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        
        private EventController _eventController;
        private float _damage;
        private IDisposable _particleCompletionSubscription;

        private BaseWeaponConfig _gunConfig;
        private WeaponData _weaponData;
        
        [Inject]
        public void Construct(EventController eventController,WeaponData weaponData)
        {
            _eventController = eventController;
            _weaponData = weaponData;
        }

        public void Init()
        {
            _bullet.SetActive(true);
        }

        private void OnCollisionEnter(Collision other)
        {  
            if (other.collider.TryGetComponent(out BodyAim bodyAim))
            {
                if (bodyAim.Enemy.TryGetComponent(out IHealth healthObject))
                {
                    ApplyDamage(healthObject, bodyAim);
                }
                _eventController.OnEnemyHitBullet();
            }
            _bullet.SetActive(false);
        }

        private void ApplyDamage(IHealth healthObject, BodyAim bodyAim)
        {
            Debug.Log(_weaponData.IsReloading);
            if (_weaponData.DamageForType.TryGetValue(bodyAim.BodyPart, out float damage))
                _damage = damage;
            
            healthObject.HealthStats.SetDamage(_damage);
        }
        
    }

