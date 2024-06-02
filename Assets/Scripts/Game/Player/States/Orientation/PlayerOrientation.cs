using System.Collections.Generic;
using Customs;
using Game.Player.PlayerStateMashine;
using UnityEngine;

namespace Game.Player.States.Orientation
{
    public abstract class PlayerOrientation : BaseMove
    {
        private Vector3 _relativeMaximum;
        private Vector3 _remappedSpeed;
        private Vector3 _relativeSpeed;
        private Vector3 _newSpeed;
        private Vector3 _positionLastFrame;
        private readonly Queue<Vector3> _speedBuffer = new ();
        private const int BufferSize = 10;

        protected PlayerOrientation(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        { }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            KalmanFilter();
            ComputeRelativeSpeed();
            ComputeRelativeSpeeds();
            UpdateAnimatorParameters();
        }

        protected virtual void ComputeRelativeSpeeds()
        {
            _relativeMaximum = Player.transform.TransformVector(Vector3.one);
            
            _remappedSpeed.x = MathfExtensions.Remap(_relativeSpeed.x, 0f, Data.CurrentSpeed, 0f, _relativeMaximum.x);
            _remappedSpeed.y = MathfExtensions.Remap(_relativeSpeed.y, 0f, Data.CurrentSpeed, 0f, _relativeMaximum.y);
            _remappedSpeed.z = MathfExtensions.Remap(_relativeSpeed.z, 0f, Data.CurrentSpeed, 0f, _relativeMaximum.z);
            _remappedSpeed.Normalize();
            
            _positionLastFrame = Player.transform.position;
        }

        private void ComputeRelativeSpeed()
        {
            _relativeSpeed = Player.PlayerModelRotate.transform.InverseTransformVector(_newSpeed);
        }

        private void KalmanFilter()
        {
            if (Time.deltaTime == 0f) return;
            
            _speedBuffer.Enqueue((Player.transform.position - _positionLastFrame) / Time.deltaTime);
            while (_speedBuffer.Count > BufferSize)
                _speedBuffer.Dequeue();
            _newSpeed = Vector3.zero;
            
            foreach (var speed in _speedBuffer)
                _newSpeed += speed;
            
            _newSpeed /= _speedBuffer.Count;
        }


        private void UpdateAnimatorParameters()
        {
            Player.AnimatorController.SetFloatParameters(Player.AnimatorController.NameRemappedForwardSpeedNormalizedParameter, _remappedSpeed.z);
            Player.AnimatorController.SetFloatParameters(Player.AnimatorController.NameRemappedLateralSpeedNormalizedParameter, _remappedSpeed.x);
            Player.AnimatorController.SetFloatParameters(Player.AnimatorController.NameRemappedSpeedNormalizedParameter, _remappedSpeed.magnitude);
        }
    }
}