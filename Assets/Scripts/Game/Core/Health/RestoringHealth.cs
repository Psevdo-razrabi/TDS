using System;
using Customs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.Core.Health
{
    public class RestoringHealth : IHealthStats, IEnemyState, IDisposable
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public bool IsHealthRestoring { get; set; } = false;
        public bool IsEnemyDie { get; set; } = false;

        private float g;

        private IHealthStats _healthStats;
        private readonly PlayerHealthConfig _playerHealthConfig;
        private readonly EventController _eventController;

        public RestoringHealth(IHealthStats healthStats, PlayerHealthConfig playerHealthConfig, EventController eventController)
        {
            MaxHealth = CurrentHealth = playerHealthConfig.MaxHealth;
            _healthStats = healthStats;
            _playerHealthConfig = playerHealthConfig;
            _eventController = eventController;

            _eventController.ShootHit += AddHealth;
        }

        public void SetDamage(float value) => _healthStats.SetDamage(value);
        
        public async UniTaskVoid AddHealth(float value = 0f)
        {
            var restoringHealth = MaxHealth * _playerHealthConfig.CoefficientRecoveryHealth * 0.01f;
            IsHealthRestoring = true;


            if (IsEnemyDie)
                _healthStats.AddHealth(MaxHealth * _playerHealthConfig.CoefficientRecoveryHealthAfterEnemyDead * 0.01f);
            
            //await DOTween
                //.To(() => 0f, x 
                    //=> _healthStats.AddHealth(), restoringHealth, _playerHealthConfig.TimeRecoveryHealth)
                    //.SetEase(Ease.Linear);
            
            IsHealthRestoring = false;
        }

        public void Dispose()
        {
            _eventController.ShootHit -= AddHealth;
        }
    }
}