﻿using System;
using UniRx;
using UnityEngine;

namespace Game.Player.PlayerStateMashine
{
    public class StateMachineData
    {
        private readonly PlayerConfigs _playerConfigs;

        public StateMachineData(PlayerConfigs configs)
        {
            _playerConfigs = configs ?? throw new ArgumentNullException($"{configs} is null");
        }
        
        public bool IsMove;
        public bool IsAim;
        public readonly ReactiveProperty<bool> IsAiming = new();
        public readonly ReactiveProperty<bool> IsDashing = new();
        public bool IsAir;
        public bool IsPlayerSitDown;
        public bool IsCrouch;
        public bool IsPlayerCrouch;
        
        private float _xInput;
        private float _yInput;
        private float _currentSpeed = 1f;
        private int _dashCount;
        private Vector2 _mouseDirection;

        public float TargetDirectionY { get; set; }

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
        public bool IsInputZeroX() => _xInput == 0;
        public bool IsInputNotZeroZ() => _yInput > 0;
    }
}