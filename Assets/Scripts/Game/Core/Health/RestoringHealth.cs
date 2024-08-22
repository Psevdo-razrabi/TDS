using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enemy;
using Enemy.interfaces;
using Game.Core.Health;
using Game.Player.AnyScripts;
using UI.Storage;
using UniRx;
using UnityEngine;

namespace Game.Core.Health
{
    public class RestoringHealth<T> : IHealthStats, IHealthRestoring, IDisposable where T : MonoBehaviour
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public bool IsHealthRestoringAfterHitEnemy { get; set; } = false;
        public bool IsHealthRestoringAfterDieEnemy { get; set; } = false;

        private float _previouslyValue;
        private float _currencyValue;

        private readonly IHealthStats _healthStats;
        private readonly PlayerHealthConfig _playerHealthConfig;
        private readonly ValueCountStorage<float> _healthValue;
        private readonly OperationWithHealth<T> _operationWithHealth;
        private CompositeDisposable _compositeDisposable = new();
        private const float TransferToInterest = 0.01f;
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public RestoringHealth(IHealthStats healthStats, PlayerHealthConfig playerHealthConfig,
            ValueCountStorage<float> healthValue)
        {
            MaxHealth = CurrentHealth = playerHealthConfig.MaxHealth;
            _healthStats = healthStats;
            _playerHealthConfig = playerHealthConfig;
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
                _healthStats.AddHealth(MaxHealth * _playerHealthConfig.CoefficientRecoveryHealthAfterEnemyDead *
                                       TransferToInterest);

            await DOTween
                .To(() => 0f, x =>
                    {
                        _healthStats.AddHealth(x + _previouslyValue);
                        _currencyValue = _healthValue.GetValue();
                    },
                    restoringHealth, _playerHealthConfig.TimeRecoveryHealth)
                .SetEase(Ease.Linear)
                .OnComplete(() => { IsHealthRestoringAfterHitEnemy = false; })
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
        }

        public void Dispose()
        {
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }
    }
}

public class OperationWithHealth<T> where T : MonoBehaviour
{
    private readonly IHit _hit;
    private readonly IDie _die;
    private readonly CompositeDisposable _compositeDisposable = new();
    private RestoringHealth<T> _restoringHealth;

    public OperationWithHealth(IDie die, IHit hit, RestoringHealth<T> restoringHealth) 
    {
        _die = die;
        _hit = hit;
        _restoringHealth = restoringHealth;
    }

    public void SubscribeHit(Action hit)
    { 
        _hit.Hit.Subscribe(_ => hit()).AddTo(_compositeDisposable);
    }

    public void SubscribeDead(Action dead)
    {
        _die.DieAction.Subscribe(_ => dead()).AddTo(_compositeDisposable);
    }
    
    public void EnemyDie()
    {
        _restoringHealth.IsHealthRestoringAfterDieEnemy = true;
        _restoringHealth.AddHealth().Forget();
        _restoringHealth.IsHealthRestoringAfterDieEnemy = false;
    }

    public void EnemyHitBullet()
    {
        switch (_restoringHealth.IsHealthRestoringAfterHitEnemy)
        {
            case true:
                _restoringHealth.CancellationTokenSource?.Cancel();
                break;
            case false:
                _restoringHealth.AddHealth().Forget();
                break;
        }
    }
}