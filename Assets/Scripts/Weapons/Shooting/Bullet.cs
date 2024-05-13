using Customs;
using Game.Core.Health;
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    private EventController _eventController;
    private float _damage;

    [Inject]
    public void Construct(EventController eventController)
    {
        _eventController = eventController;
    }
    
    public void Initialize(float damage)
    {
        _damage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {  
        Debug.Log("попал");
        if (other.collider.TryGetComponent(out IHealth health))
        {
            ApplyDamage(health);
        }

        if (other.collider.TryGetComponent(out Enemy.Enemy enemy))
        {
            _eventController.OnEnemyHitBullet();
        }
        
        gameObject.SetActive(false);
    }

    private void ApplyDamage(IHealth healthObject)
    {
        _eventController.ShootHit();
        healthObject.HealthStats.SetDamage(_damage);
        Debug.Log($"Логика нанесения урона {_damage} ");
    }
}

