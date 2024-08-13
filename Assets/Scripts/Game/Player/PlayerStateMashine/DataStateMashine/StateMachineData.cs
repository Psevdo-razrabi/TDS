using System;
using Cysharp.Threading.Tasks;
using Game.Player.AnimatorScripts;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States;
using UniRx;
using UnityEngine;
using IInitializable = Zenject.IInitializable;

namespace Game.Player.PlayerStateMashine
{
    public class StateMachineData : IInitializable, IDisposable
    {
        private readonly PlayerConfigs _playerConfigs;
        private readonly AnimatorController _animatorController;
        private readonly CompositeDisposable _compositeDisposable = new();

        public StateMachineData(PlayerConfigs configs, AnimatorController animatorController)
        {
            _playerConfigs = configs ?? throw new ArgumentNullException($"{configs} is null");
            _animatorController = animatorController ?? throw new ArgumentException($"Animator controller is null");
        }
        
        public readonly ReactiveProperty<bool> IsCrouch = new();
        public readonly ReactiveProperty<bool> IsMove = new();
        public readonly ReactiveProperty<bool> IsAim = new();
        public readonly ReactiveProperty<bool> IsAiming = new();
        public readonly ReactiveProperty<bool> IsDashing = new();
        public readonly ReactiveProperty<bool> IsLookAtObstacle = new();
        public readonly ReactiveProperty<bool> IsClimbing = new();
        public readonly ReactiveProperty<bool> IsGrounded = new();
        public bool IsAir;
        public bool IsLockAim;
        public bool IsPlayerInObstacle;
        public ClimbParameters Climb = new();
        public LandingParameters Landing = new();
        public ObstacleParametersConfig ObstacleConfig;
        public Quaternion Rotation;
        
        private float _xInput;
        private float _yInput;
        private float _speed;
        private float _currentSpeed = 1f;
        private int _dashCount;
        private Vector2 _mouseDirection;

        public float TargetDirectionY { get; set; }

        public float Speed
        {
            get => _speed;
            set
            {
                if (value < 0 || value > _playerConfigs.CrouchMovement.Speed)
                    throw new ArgumentOutOfRangeException($"{value} is out of range");

                _speed = value;
            }
        }
        
        public Vector2 MouseDirection
        {
            get => _mouseDirection;
            set
            {
                if (value.x + float.Epsilon < -1 || value.x + float.Epsilon > 1 || value.y + float.Epsilon < -1 || value.y + float.Epsilon > 1)
                    throw new ArgumentOutOfRangeException();

                _mouseDirection = value;
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
                if (value < 0 - 1 || value > _playerConfigs.DashConfig.NumberChargesDash + 1)
                    throw new ArgumentOutOfRangeException();

                _dashCount = value;
            }
        }

        public bool IsInputZero() => _xInput == 0 && _yInput == 0;
        
        public void Initialize()
        {
            IsCrouch
                .Subscribe(_ =>
                _animatorController.OnAnimatorStateSet(IsCrouch, _animatorController.NameIsCrouchParameter))
                .AddTo(_compositeDisposable);

            IsAim
                .Subscribe(_ => _animatorController.OnAnimatorStateSet(IsAim, _animatorController.NameAimParameter))
                .AddTo(_compositeDisposable);
            
            IsMove
                .Subscribe(_ => _animatorController.OnAnimatorStateSet(IsMove, _animatorController.NameMoveParameter))
                .AddTo(_compositeDisposable);
            IsGrounded
                .Subscribe(_ =>
                _animatorController.OnAnimatorStateSet(IsGrounded, _animatorController.NameIsGroundParameter))
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable?.Clear();
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
    }
}