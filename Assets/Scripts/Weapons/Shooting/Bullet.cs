using System;
using Cysharp.Threading.Tasks;
using Game.Core.Health;
using Game.Player.Weapons.WeaponConfigs;
using UniRx;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        
        private EventController _eventController;
        private float _damage;
        private IDisposable _particleCompletionSubscription;
        private CompositeDisposable _compositeDisposable = new();
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
            
            SubscribeRayCheck();
        }

        private void SubscribeRayCheck()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => CheckRaycastHit())
                .AddTo(_compositeDisposable);
        }
        
        /*
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
        */
            
        private void CheckRaycastHit()
        {
            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward;
            
            float rayLength = 1.0f;
            
            RaycastHit hit;
            
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength))
            {
                if (hit.collider.TryGetComponent(out BodyAim bodyAim))
                {
                    if (bodyAim.Enemy.TryGetComponent(out IHealth healthObject))
                    {
                        ApplyDamage(healthObject, bodyAim);
                    }
                    _eventController.OnEnemyHitBullet();
                }
                _bullet.SetActive(false);
                Debug.Log("ПУЛЯ ПОПАЛА");
            }
        }

        private void ApplyDamage(IHealth healthObject, BodyAim bodyAim)
        {
            if (_weaponData.DamageForType.TryGetValue(bodyAim.BodyPart, out float damage))
                _damage = damage;
            
            healthObject.HealthStats.SetDamage(_damage);
        }
        
    }

