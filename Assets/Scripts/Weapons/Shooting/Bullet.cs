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
        _eventController.ShotHit();
        healthObject.TakeDamage(_damage);
        Debug.Log($"Логика нанесения урона {_damage} ");
    }
}
