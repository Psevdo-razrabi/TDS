using System;

namespace Game.Player.PlayerStateMashine
{
    public class StateMachineData
    {
        private PlayerConfigs _playerConfigs;

        public StateMachineData(PlayerConfigs configs)
        {
            _playerConfigs = configs ?? throw new ArgumentNullException($"{configs} is null");
        }
        
        public bool IsMove;
        public bool IsAim;
        public bool IsDashing;
        
        private float _xInput;
        private float _yInput;
        private float _currentSpeed;
        private int _dashCount;

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
    }
}