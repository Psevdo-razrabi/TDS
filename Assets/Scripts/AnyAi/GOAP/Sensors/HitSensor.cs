using System;
using Game.Player.AnyScripts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GOAP
{
    public class HitSensor : MonoBehaviour, ISensor
    {
        public Vector3? Target { get; private set; }

        public bool IsTargetInSensor { get; private set; }

        [SerializeField] private PlayerComponents _playerComponents;
        [SerializeField] private float _timeAggression;
        private CompositeDisposable _compositeDisposable = new();
        private CompositeDisposable _compositeDisposableRemoved = new();

        private void OnEnable()
        {
            SubscribeTriggers();
        }

        private void SubscribeTimer()
        {
            Target = _playerComponents.transform.position;
            IsTargetInSensor = true;

            Observable.Timer(TimeSpan.FromSeconds(_timeAggression)).Subscribe(_ =>
            {
                Target = null;
                IsTargetInSensor = false;
            }).AddTo(_compositeDisposableRemoved);
        }

        private void UnsubscribeUpdate()
        {
            _compositeDisposableRemoved.Clear();
            _compositeDisposableRemoved.Dispose();
        }

        private void OnDisable()
        {
            UnsubscribeTriggers();
            UnsubscribeUpdate();
        }

        private void SubscribeTriggers()
        {
            this.OnTriggerEnterAsObservable().Subscribe(collider =>
            {
                UnsubscribeUpdate();
                if (!collider.TryGetComponent(out PlayerComponents playerComponents) &&
                    !collider.TryGetComponent(out Bullet bullet)) return;
                _compositeDisposableRemoved = new CompositeDisposable();
                SubscribeTimer();

            }).AddTo(_compositeDisposable);
        }

        private void UnsubscribeTriggers()
        {
            _compositeDisposable.Clear();
            _compositeDisposable.Dispose();
        }
    }
}