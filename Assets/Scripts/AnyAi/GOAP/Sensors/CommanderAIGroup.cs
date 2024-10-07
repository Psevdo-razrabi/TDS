using System;
using System.Linq;
using Game.Player.AnyScripts;
using UniRx;
using UnityEngine;

namespace GOAP
{
    public class CommanderAIGroup : MonoBehaviour
    {
        public Subject<Unit> IsTargetDetect { get; private set; } = new();
        
        [field: SerializeField] private HitSensor[] _hitSensorInGroopAI;
        [field: SerializeField] private EyesSensor[] _eyesSensorsInGroopAI;
        [field: SerializeField] private PlayerComponents _playerComponents;

        private IDisposable _disposable;

        public bool IsTargetAttack => _hitSensorInGroopAI.Any(hit => hit.IsActivate.Value) ||
                                      _eyesSensorsInGroopAI.Any(eyes => eyes.IsActivate.Value);

        private void OnEnable()
        {
            SubscribeTarget();
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }
        
        private void SubscribeTarget()
        {
            _disposable = IsTargetDetect.Subscribe(_ =>
            {
                foreach (var hitSensor in _hitSensorInGroopAI)
                {
                    hitSensor.SetTarget(_playerComponents.transform.position);
                    hitSensor.SetIsTargetTrigger(true);
                }

                foreach (var eyesSensor in _eyesSensorsInGroopAI)
                {
                    eyesSensor.SetTarget(_playerComponents.transform.position);
                    eyesSensor.SetIsTargetTrigger(true);
                }
            });
        }
    }
}