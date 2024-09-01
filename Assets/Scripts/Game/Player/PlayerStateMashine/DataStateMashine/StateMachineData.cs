using System;
using BlackboardScripts;
using Game.AsyncWorker.Interfaces;
using Game.Player.AnimatorScripts;
using Game.Player.PlayerStateMashine.Configs;
using UniRx;
using UnityEngine;
using IInitializable = Zenject.IInitializable;

namespace Game.Player.PlayerStateMashine
{
    public class StateMachineData : IInitializable, IDisposable, ILoadable
    {
        public PlayerConfigs PlayerConfigs { get; private set; }
        public bool IsLoaded { get; private set; }
        private readonly AnimatorController _animatorController;
        private readonly CompositeDisposable _compositeDisposable = new();
        private readonly Blackboard _blackboard = new ();

        public StateMachineData(PlayerConfigs configs, AnimatorController animatorController)
        {
            PlayerConfigs = configs ?? throw new ArgumentNullException($"{configs} is null");
            _animatorController = animatorController ?? throw new ArgumentException($"Animator controller is null");
        }
        
        private float _xInput;
        private float _yInput;
        private float _speed;
        private float _currentSpeed = 1f;
        private int _dashCount;
        private ObstacleParametersConfig _obstacle;
        private PlayerMoveConfig _playerMoveConfig;
        
        public float Speed
        {
            get => _speed;
            set
            {
                if (value < 0 || value > PlayerConfigs.CrouchConfigsProvider.CrouchMovement.Speed)
                    throw new ArgumentOutOfRangeException($"{value} is out of range");

                _speed = value;
            }
        }
        
        public float XInput
        {
            get => _xInput;
            set
            {
                if (value is < -1 or > 1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _xInput = value;
            }
        }
        
        public float YInput
        {
            get => _yInput;
            set
            {
                if (value is < -1 or > 1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _yInput = value;
            }
        }

        public float CurrentSpeed
        {
            get => _currentSpeed;
            set
            {
                if (_currentSpeed < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _currentSpeed = value;
            }
        }

        public int DashCount
        {
            get => _dashCount;
            set
            {
                if (value < 0 - 1 || value > PlayerConfigs.MovementConfigsProvider.DashConfig.NumberChargesDash + 1)
                    throw new ArgumentOutOfRangeException();

                _dashCount = value;
            }
        }

        public bool IsInputZero() => _xInput == 0 && _yInput == 0;

        public T GetValue<T>(string key)
        {
            _blackboard.TryGetValue<T>(_blackboard.GetOrRegisterKey(key), out var value);
            return value;
        }

        public void SetValue<T>(string key, T value)
        {
            _blackboard.SetValue(_blackboard.GetOrRegisterKey(key), value);
        }
        
        public void Initialize()
        {
            // AddInBlackboard((Name.IsCrouch, new ReactiveProperty<bool>(), typeof(ReactiveProperty<bool>)), (Name.IsMove, new ReactiveProperty<bool>(), typeof(ReactiveProperty<bool>)),
            //     (Name.IsAim, new ReactiveProperty<bool>(), typeof(ReactiveProperty<bool>)), (Name.IsAiming, new ReactiveProperty<bool>(), typeof(ReactiveProperty<bool>)),
            //     (Name.IsDashing, new ReactiveProperty<bool>(), typeof(ReactiveProperty<bool>)), (Name.IsLookAtObstacle, new ReactiveProperty<bool>(), typeof(ReactiveProperty<bool>)),
            //     (Name.IsClimbing, new ReactiveProperty<bool>(), typeof(ReactiveProperty<bool>)), (Name.IsGrounded, new ReactiveProperty<bool>(), typeof(ReactiveProperty<bool>)),
            //     (Name.IsLockAim, new bool(), typeof(bool)), (Name.IsPlayerInObstacle, new bool(), typeof(bool)),
            //     (Name.Climb, new ClimbParameters(), typeof(ClimbParameters)), (Name.Landing, new LandingParameters(), typeof(LandingParameters)),
            //     (Name.ObstacleConfig, _obstacle, typeof(ObstacleParametersConfig)), (Name.PlayerMoveConfig, _playerMoveConfig, typeof(PlayerMoveConfig)), (Name.Rotation, new Quaternion(), typeof(Quaternion)),
            //     (Name.Movement, new Vector3(), typeof(Vector3)), (Name.TargetDirectionY, new float(), typeof(float)));
            
            _blackboard.AddKeyValuePair(Name.IsCrouch, new ReactiveProperty<bool>());
            _blackboard.AddKeyValuePair(Name.IsMove, new ReactiveProperty<bool>());
            _blackboard.AddKeyValuePair(Name.IsAim, new ReactiveProperty<bool>());
            _blackboard.AddKeyValuePair(Name.IsAiming, new ReactiveProperty<bool>());
            _blackboard.AddKeyValuePair(Name.IsDashing, new ReactiveProperty<bool>());
            _blackboard.AddKeyValuePair(Name.IsGrounded, new ReactiveProperty<bool>());
            _blackboard.AddKeyValuePair(Name.IsLookAtObstacle, new ReactiveProperty<bool>());
            _blackboard.AddKeyValuePair(Name.IsClimbing, new ReactiveProperty<bool>());
            _blackboard.AddKeyValuePair(Name.IsLockAim, new bool());
            _blackboard.AddKeyValuePair(Name.IsPlayerInObstacle, new bool());
            _blackboard.AddKeyValuePair(Name.Climb, new ClimbParameters());
            _blackboard.AddKeyValuePair(Name.Landing, new LandingParameters());
            _blackboard.AddKeyValuePair(Name.ObstacleConfig, _obstacle);
            _blackboard.AddKeyValuePair(Name.PlayerMoveConfig, _playerMoveConfig);
            _blackboard.AddKeyValuePair(Name.Rotation, new Quaternion());
            _blackboard.AddKeyValuePair(Name.Movement, new Vector3());
            _blackboard.AddKeyValuePair(Name.TargetDirectionY, new float());
            
            _blackboard.Debug();

            SubscribesProperties(("IsCrouch", _animatorController.NameIsCrouchParameter),
                ("IsMove", _animatorController.NameMoveParameter), ("IsAim", _animatorController.NameAimParameter),
                ("IsGrounded", _animatorController.NameIsGroundParameter));

            IsLoaded = true;
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable?.Clear();
        }
        
        private void SubscribeProperty(string key, string nameAnimatorParameters)
        {
            var property = GetValue<ReactiveProperty<bool>>(key);

            property
                .Subscribe(_ =>
                    _animatorController.OnAnimatorStateSet(property, nameAnimatorParameters))
                .AddTo(_compositeDisposable);
        }

        private void AddInBlackboard<T>(params (string, IValueType<T>) [] parameters)
        {
            foreach (var (key, value) in parameters)
            {
                _blackboard.AddKeyValuePair(key, value);
            }
        }

        private void SubscribesProperties(params (string, string)[] subscribers)
        {
            foreach (var (key, nameAnimatorParameter) in subscribers)
            {
                SubscribeProperty(key, nameAnimatorParameter);
            }
        }
    }
    
    public class ClimbParameters
    {
        public float correctionHeight;
        public string animationTriggerName;
        public float animationClipDuration;
    }
        
    public class LandingParameters
    {
        public string animationTriggerName;
        public float animationClipDuration;
    }
    
    public interface IValueType<out T>
    {
        T value { get; }
    }

    public struct ValueBool : IValueType<bool>
    {
        public bool value { get; }
    }
}