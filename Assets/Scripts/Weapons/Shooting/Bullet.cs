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
    private IDisposable particleCompletionSubscription;

    [Inject]
    public void Construct(EventController eventController)
    {
        _eventController = eventController;
    }

    public void Initialize(float damage)
    {
        _damage = damage;
        _bullet.SetActive(true);
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision other)
    {  
        if (other.collider.TryGetComponent(out IHealth healthObject))
        {
            ApplyDamage(healthObject);
        }
        Debug.Log("система частиц должна");
        _particleSystem.Play();
        _bullet.SetActive(false);
        SubscribeParticle();
    }

    private void ApplyDamage(IHealth healthObject)
    {
        _eventController.ShootHit();
    }

    private void SubscribeParticle()
    {
        particleCompletionSubscription?.Dispose();
        particleCompletionSubscription = Observable.EveryUpdate()
            .Where(_ => !_particleSystem.isEmitting)
            .First()
            .Subscribe(_ =>
            {
                gameObject.SetActive(false);
            });
    }
}

