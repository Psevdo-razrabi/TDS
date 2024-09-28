using System;
using Game.Player.AnyScripts;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace GOAP
{
    [RequireComponent(typeof(SphereCollider))]
    public class AttackSensor : MonoBehaviour, ISensor
    {
        [SerializeField] private float _radiusDetect;
        [SerializeField] private float _timeInterval;
        
        public Vector3? Target => _target ? _target.transform.position : Vector3.zero;
        public bool IsTargetInSensor => Target != Vector3.zero;

        public Subject<Unit> OnTargetChange { get; } = new();
        private GameObject _target;
        private Vector3 _lastKnownPosition;
        private SphereCollider _sphereCollider;
        private CompositeDisposable _compositeDisposable = new();


        private void OnEnable()
        {
            SubTriggers();
            SubTimer();
        }

        private void SubTimer()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(_timeInterval), TimeSpan.FromSeconds(_timeInterval))
                .Subscribe(_ => UpdateTargetPosition(_target))
                .AddTo(_compositeDisposable);
        }

        private void SubTriggers()
        {
            _sphereCollider.OnTriggerEnterAsObservable().Subscribe(trigger =>
            {
                if (trigger.TryGetComponent(out PlayerComponents player))
                {
                    UpdateTargetPosition(trigger.gameObject);
                }
            }).AddTo(_compositeDisposable);
            
            _sphereCollider.OnTriggerExitAsObservable().Subscribe(trigger =>
            {
                if (trigger.TryGetComponent(out PlayerComponents player))
                {
                    UpdateTargetPosition();
                }
            }).AddTo(_compositeDisposable);
        }

        private void OnDisable()
        {
            _compositeDisposable?.Clear();
            _compositeDisposable?.Dispose();
        }

        private void Awake()
        {
            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.isTrigger = true;
            _sphereCollider.radius = _radiusDetect;
        }

        private void UpdateTargetPosition(GameObject target = null)
        {
            _target = target;

            if (!IsTargetInSensor || (_lastKnownPosition == Target && _lastKnownPosition == Vector3.zero)) return;
            if (Target != null) _lastKnownPosition = Target.Value;
            OnTargetChange.OnNext(Unit.Default);
        }
    }
}