using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    private CompositeDisposable _disposable = new();
    private float _damage;
    
    public void Init(float damage)
    {
        _damage = damage;
        
        _collider.OnCollisionEnterAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("ЗАШЕЛ");
                ApplyDamage();
            })
            .AddTo(_disposable);
    }
    
    private void ApplyDamage()
    {
        Debug.Log($"Логика нанесения урона {_damage} ");
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}

