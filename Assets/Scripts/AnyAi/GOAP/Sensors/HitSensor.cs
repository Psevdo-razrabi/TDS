using System;
using Game.Player.AnyScripts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GOAP
{
    public class HitSensor : MonoBehaviour, ISensor, ISensorTriggerCommander
    {
        [SerializeField] private PlayerComponents _playerComponents;
        [SerializeField] private float _timeAggression;
        [SerializeField] private CommanderAIGroup commanderAI;
        public Vector3? Target { get; private set; }
        public bool IsTargetInSensor { get; private set; }
        private CompositeDisposable _compositeDisposable = new();
        private CompositeDisposable _compositeDisposableRemoved = new();

        public void SetTarget(Vector3 target)
        {
            SubscribeTimer();
        }

        public void SetIsTargetTrigger(bool isTargetDetected)
        {
            SubscribeTimer();
        }

        private void OnEnable()
        {
            SubscribeTriggers();
        }

        private void SubscribeTimer()
        {
            Target = _playerComponents.transform.position;
            IsTargetInSensor = true;
            commanderAI.IsTargetDetect.OnNext(Unit.Default);

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