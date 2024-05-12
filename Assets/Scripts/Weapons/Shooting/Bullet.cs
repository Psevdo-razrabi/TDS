using UnityEngine;

public class Bullet : MonoBehaviour
{
    private EventController _eventController;
    private float _damage;

    public Bullet(EventController eventController)
    {
        _eventController = eventController;
    }

    public void Initialize(float damage)
    {
        _damage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {  
        if (other.collider.TryGetComponent(out IHealth healthObject))
        {
            ApplyDamage(healthObject);
        }
        gameObject.SetActive(false);
    }

    private void ApplyDamage(IHealth healthObject)
    {
<<<<<<< Updated upstream
        _eventController.ShotHit();
        healthObject.TakeDamage(_damage);
=======
        _eventController.ShootHit();
        healthObject.HealthStats.SetDamage(_damage);
>>>>>>> Stashed changes
        Debug.Log($"Логика нанесения урона {_damage} ");
    }
}
