using System;
using Customs;
using Game.Core.Health;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    
    private EventController _eventController;
    private float _damage;
    private ParticleSystem _particleSystem;
    private IDisposable _particleCompletionSubscription;
    private BulletEffectSystem _bulletEffectSystem;

    [Inject]
    public void Construct(EventController eventController, BulletEffectSystem bulletEffectSystem)
    {
        _eventController = eventController;
        _bulletEffectSystem = bulletEffectSystem;
    }

    public void Initialize(float damage)
    {
        _damage = damage;
        _bullet.SetActive(true);
    }

    private void OnCollisionEnter(Collision other)
    {  
        if (other.collider.TryGetComponent(out IHealth healthObject))
        {
            ApplyDamage(healthObject);
        }
        
        if (other.collider.TryGetComponent(out Enemy.Enemy enemy))
        {
            _eventController.OnEnemyHitBullet();
        }
        
        _bullet.SetActive(false);
    }

    private void ApplyDamage(IHealth healthObject)
    {
        healthObject.HealthStats.SetDamage(_damage);
    }

}

