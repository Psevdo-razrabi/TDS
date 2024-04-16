using System;

namespace Game.Player.PlayerStateMashine
{
    public class StateMachineData
    {
        public bool IsMove;
        public bool IsAim;
        public bool IsDashing;
        
        private float _xInput;
        private float _yInput;
        private float _currentSpeed;

        public float XInput
        {
            get => _xInput;
            set
            {
                if (value < -1 || value > 1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _xInput = value;
            }
        }
        
        public float YInput
        {
            get => _yInput;
            set
            {
                if (value < -1 || value > 1)
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
        
        public bool IsInputZero() => _xInput == 0 && _yInput == 0;
    }
}