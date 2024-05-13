using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UI.Storage;

namespace Game.Core.Health
{
    public class RestoringHealth : IHealthStats, IHealthRestoring, IDisposable
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public bool IsHealthRestoringAfterHitEnemy { get; set; } = false;
        public bool IsHealthRestoringAfterDieEnemy { get; set; } = false;

        private float _previouslyValue;
        private float _currencyValue;

        private readonly IHealthStats _healthStats;
        private readonly PlayerHealthConfig _playerHealthConfig;
        private readonly EventController _eventController;
        private readonly ValueCountStorage<float> _healthValue;
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public RestoringHealth(IHealthStats healthStats, PlayerHealthConfig playerHealthConfig, EventController eventController, ValueCountStorage<float> healthValue)
        {
            MaxHealth = CurrentHealth = playerHealthConfig.MaxHealth;
            _healthStats = healthStats;
            _playerHealthConfig = playerHealthConfig;
            _eventController = eventController;
            _healthValue = healthValue;

            _eventController.ShootHited += AddHealth;
            _eventController.EnemyDie += EnemyDie;
            _eventController.EnemyHitBullet += EnemyHitBullet;
        }

        public void SetDamage(float value) => _healthStats.SetDamage(value);
        
        public async UniTaskVoid AddHealth(float value = 0f)
        {
            var restoringHealth = MaxHealth * _playerHealthConfig.CoefficientRecoveryHealth * 0.01f;
            IsHealthRestoringAfterHitEnemy = true;
            _previouslyValue = _healthValue.GetValue();
            CancellationTokenSource = new CancellationTokenSource();
            
            if (IsHealthRestoringAfterDieEnemy)
                _healthStats.AddHealth(MaxHealth * _playerHealthConfig.CoefficientRecoveryHealthAfterEnemyDead * 0.01f);

            await DOTween
                .To(() => 0f, x
                        =>
                    {
                        _healthValue.SetValue(x + _previouslyValue);
                        _currencyValue = _healthValue.GetValue();
                    },
                    restoringHealth, _playerHealthConfig.TimeRecoveryHealth)
                .SetEase(Ease.Linear)
                .OnComplete(async () =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.7f));
                    IsHealthRestoringAfterHitEnemy = false;
                })
                .OnKill(async () =>
                {
                    _healthStats.AddHealth(_currencyValue - _previouslyValue);
                    await UniTask.Delay(TimeSpan.FromSeconds(0.7f));
                    IsHealthRestoringAfterHitEnemy = false;
                })
                .WithCancellation(CancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _eventController.ShootHited -= AddHealth;
            _eventController.EnemyDie -= EnemyDie;
            _eventController.EnemyHitBullet -= EnemyHitBullet;

        }

        private void EnemyDie()
        {
            IsHealthRestoringAfterDieEnemy = true;
            _healthStats.AddHealth(0f);
            IsHealthRestoringAfterDieEnemy = false;
        }

        private void EnemyHitBullet()
        {
            switch (IsHealthRestoringAfterHitEnemy)
            {
                case true:
                    CancellationTokenSource?.Cancel();
                    break;
                case false:
                    _healthStats.AddHealth(0f);
                    break;
            }
        }
    }
}