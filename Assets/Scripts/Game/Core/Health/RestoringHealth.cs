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
        private const float TransferToInterest = 0.01f;
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public RestoringHealth(IHealthStats healthStats, PlayerHealthConfig playerHealthConfig, EventController eventController, ValueCountStorage<float> healthValue)
        {
            MaxHealth = CurrentHealth = playerHealthConfig.MaxHealth;
            _healthStats = healthStats;
            _playerHealthConfig = playerHealthConfig;
            _eventController = eventController;
            _healthValue = healthValue;
        }

        public void SetDamage(float value) => _healthStats.SetDamage(value);
        
        public async UniTaskVoid AddHealth(float value = 0f)
        {
            var restoringHealth = MaxHealth * _playerHealthConfig.CoefficientRecoveryHealth * TransferToInterest;
            IsHealthRestoringAfterHitEnemy = true;
            _previouslyValue = _healthValue.GetValue();
            CancellationTokenSource = new CancellationTokenSource();
            
            if (IsHealthRestoringAfterDieEnemy)
                _healthStats.AddHealth(MaxHealth * _playerHealthConfig.CoefficientRecoveryHealthAfterEnemyDead * TransferToInterest);

            await DOTween
                .To(() => 0f, x =>
                    {
                        _healthStats.AddHealth(x + _previouslyValue);
                        _currencyValue = _healthValue.GetValue();
                    },
                    restoringHealth, _playerHealthConfig.TimeRecoveryHealth)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    IsHealthRestoringAfterHitEnemy = false;
                })
                .OnKill(async () =>
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.7f));
                    IsHealthRestoringAfterHitEnemy = false;
                })
                .WithCancellation(CancellationTokenSource.Token);
        }

        public void Unsubscribe()
        {
            _healthStats.Unsubscribe();
            Dispose();
        }

        public void Subscribe()
        {
            _healthStats.Subscribe();
            _eventController.EnemyDie += EnemyDie;
            _eventController.EnemyHitBullet += EnemyHitBullet;
        }

        public void Dispose()
        {
            _eventController.EnemyDie -= EnemyDie;
            _eventController.EnemyHitBullet -= EnemyHitBullet;
        }

        private void EnemyDie()
        {
            IsHealthRestoringAfterDieEnemy = true;
            AddHealth().Forget();
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
                    AddHealth().Forget();
                    break;
            }
        }
    }
}