using System.Collections.Generic;
using Customs;
using Game.Player.AnyScripts;
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

        protected PlayerOrientation(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        { }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            AimIsFreeze(Player.PlayerComponents.transform.rotation);
            KalmanFilter();
            ComputeRelativeSpeed();
            ComputeRelativeSpeeds();
            UpdateAnimatorParameters();
        }

        protected virtual void ComputeRelativeSpeeds()
        {
            _relativeMaximum = Player.PlayerComponents.transform.TransformVector(Vector3.one);
            
            _remappedSpeed.x = MathfExtensions.Remap(_relativeSpeed.x, 0f, Data.CurrentSpeed, 0f, _relativeMaximum.x);
            _remappedSpeed.y = MathfExtensions.Remap(_relativeSpeed.y, 0f, Data.CurrentSpeed, 0f, _relativeMaximum.y);
            _remappedSpeed.z = MathfExtensions.Remap(_relativeSpeed.z, 0f, Data.CurrentSpeed, 0f, _relativeMaximum.z);
            _remappedSpeed.Normalize();
            
            _positionLastFrame = Player.PlayerComponents.transform.position;
        }

        private void ComputeRelativeSpeed()
        {
            _relativeSpeed = Player.PlayerComponents.PlayerModelRotate.transform.InverseTransformVector(_newSpeed);
        }

        private void KalmanFilter()
        {
            if (Time.deltaTime == 0f) return;
            
            _speedBuffer.Enqueue((Player.PlayerComponents.transform.position - _positionLastFrame) / Time.deltaTime);
            while (_speedBuffer.Count > BufferSize)
                _speedBuffer.Dequeue();
            _newSpeed = Vector3.zero;
            
            foreach (var speed in _speedBuffer)
                _newSpeed += speed;
            
            _newSpeed /= _speedBuffer.Count;
        }


        private void UpdateAnimatorParameters()
        {
            Player.PlayerAnimation.AnimatorController.SetFloatParameters(Player.PlayerAnimation.AnimatorController.NameRemappedForwardSpeedNormalizedParameter, _remappedSpeed.z);
            Player.PlayerAnimation.AnimatorController.SetFloatParameters(Player.PlayerAnimation.AnimatorController.NameRemappedLateralSpeedNormalizedParameter, _remappedSpeed.x);
            Player.PlayerAnimation.AnimatorController.SetFloatParameters(Player.PlayerAnimation.AnimatorController.NameRemappedSpeedNormalizedParameter, _remappedSpeed.magnitude);
        }
    }
}