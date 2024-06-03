using System;
using System.Threading;
using Customs;
using Cysharp.Threading.Tasks;
using Enemy.interfaces;
using UI.Storage;
using UnityEngine;

namespace Game.Core.Health
{
    public class Health<T> : IHealthStats, IDisposable
    {
        public float MaxHealth { get; }
        public float CurrentHealth { get; private set; }
        private readonly ValueCountStorage<float> _healthValue;
        private readonly IDie<T> _objectHealth;
        private readonly EventController _eventController;
        private float _amountHealthPercentage = 1f;
        private const float TransferFromInterest = 100f;
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public Health(float health, ValueCountStorage<float> healthValue, IDie<T> objectHealth)
        {
            MaxHealth = CurrentHealth = health;
            _healthValue = healthValue;
            _objectHealth = objectHealth;

            _healthValue.SetValue(1f);
        }

        public void SetDamage(float value)
        {
            if (value < 0) throw new ArgumentException($"The Argument {nameof(value)} cannot be <0");

            CurrentHealth = Mathf.Clamp(CurrentHealth - value, 0f, MaxHealth);

            _amountHealthPercentage -= value / MaxHealth;
            
            _healthValue.ChangeValue(_amountHealthPercentage);
            
            if (CurrentHealth != 0f) return;
            
            _objectHealth.Died();
        }

        public async UniTaskVoid AddHealth(float value)
        {
            if (value < 0) throw new ArgumentException($"The Argument {nameof(value)} cannot be < 0");

            CurrentHealth = Mathf.Clamp(value * TransferFromInterest, 0f, MaxHealth);

            _amountHealthPercentage = value;
            
            _healthValue.ChangeValue(_amountHealthPercentage);
            
            await UniTask.Yield();
        }

        public void Unsubscribe()
        {
            Dispose();
        }

        public void Subscribe()
        {
            //empty
        }

        public void Dispose()
        {
            CancellationTokenSource?.Dispose();
        }
    }
}