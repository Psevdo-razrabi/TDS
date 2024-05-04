using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private CompositeDisposable _disposable = new();
    private float _damage;
    
    public void Initialize(float damage)
    {
        _damage = damage;
    }

    private void OnCollisionEnter(Collision other)
    {
        ApplyDamage(); 
        gameObject.SetActive(false);
    }

    private void ApplyDamage()
    {
        Debug.Log($"Логика нанесения урона {_damage} ");
    }
}

