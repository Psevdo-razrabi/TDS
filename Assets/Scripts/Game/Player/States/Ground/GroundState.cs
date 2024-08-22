using System;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class GroundState : PlayerBehaviour
    {
        private float _obstacleHeight;
        protected PlayerConfigs PlayerConfig;
        protected PlayerParkourConfig ParkourConfig;
        protected Vector3 TargetPosition;
        
        protected GroundState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        { }
        
        public override void OnEnter()
        {
            base.OnEnter();
            ParkourConfig = Player.PlayerConfigs.MovementConfigsProvider.ParkourConfig;
            PlayerConfig = Player.PlayerConfigs;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            CheckObstacle();
        }

        protected void OnExitAimState()
        {
            var config = Player.PlayerConfigs.AnyPlayerConfigs.FowConfig;
            Player.PlayerComponents.RadiusChanger.ChangerRadius(config.EndValueRadius, config.TimeToMaxRadius);
        }
        
        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();
            Player.PlayerInputStorage.InputSystem.OnClimb += OnClimb;
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            Player.PlayerInputStorage.InputSystem.OnClimb -= OnClimb;
        }

        private void CheckObstacle()
        {
            bool isRaycast = Physics.Raycast(Player.PlayerView.ModelRotate.transform.position,
                Player.PlayerView.ModelRotate.transform.forward, out RaycastHit hit, ParkourConfig.RayCastDistance,
                ParkourConfig.LayerMask);
            
            if (isRaycast)
            {
                _obstacleHeight = hit.collider.bounds.size.y;
                TargetPosition = GetRaisedEndPoint(hit);
                Data.Rotation = Quaternion.LookRotation(-hit.normal);
                Data.IsLookAtObstacle.Value = true;
                DrawDebugLine(hit.point, TargetPosition, hit.normal);
            }
            else
            {
                Data.IsLookAtObstacle.Value = false;
            }
        }
        
        private void DrawDebugLine(Vector3 startPoint, Vector3 endPoint, Vector3 normal)
        {
            Debug.DrawLine(Player.PlayerView.ModelRotate.transform.position, startPoint, Color.red);
            Debug.DrawLine(startPoint, endPoint, Color.cyan);
            Debug.DrawLine(Player.PlayerView.ModelRotate.transform.position, Player.PlayerView.ModelRotate.transform.rotation * normal, new Color(255,170,32,1));
        }

        private Vector3 GetRaisedEndPoint(RaycastHit hit)
        {
            var smallObstacle = PlayerConfig.ObstacleConfigsProvider.SmallObstacle;
            var middleObstacle = PlayerConfig.ObstacleConfigsProvider.MiddleObstacle;
            var largeObstacle = PlayerConfig.ObstacleConfigsProvider.LargeObstacle;
            if (_obstacleHeight >= smallObstacle.RangeAndCorrectionForClimb.From && _obstacleHeight <= smallObstacle.RangeAndCorrectionForClimb.Before)
            {
                InitClimbParameters(smallObstacle, Player.PlayerAnimation.AnimatorController.NameIsStepParameter);
            }
            else if (_obstacleHeight >= middleObstacle.RangeAndCorrectionForClimb.From && _obstacleHeight <= middleObstacle.RangeAndCorrectionForClimb.Before)
            {
                InitClimbParameters(middleObstacle, Player.PlayerAnimation.AnimatorController.NameIsClimbParameter);
                InitLandingParameters(Player.PlayerAnimation.AnimatorController.NameIsLandingMiddle);
            }
            else if (_obstacleHeight >= largeObstacle.RangeAndCorrectionForClimb.From && _obstacleHeight <= largeObstacle.RangeAndCorrectionForClimb.Before)
            {
                InitClimbParameters(largeObstacle, Player.PlayerAnimation.AnimatorController.NameIsClimbToWallParameter);
                InitLandingParameters(Player.PlayerAnimation.AnimatorController.NameIsLandingLarge);
            }
            else
            {
                throw new Exception("Obstacle height out of range");
            }
            
            return hit.point + Vector3.up * (hit.collider.bounds.size.y - Data.Climb.correctionHeight);
        }

        private void InitClimbParameters(ObstacleParametersConfig config, string nameAnimation)
        {
            Data.ObstacleConfig = config;
            Data.Climb.correctionHeight = config.RangeAndCorrectionForClimb.HeightCorrection;
            Data.Climb.animationTriggerName = nameAnimation;
            Data.Climb.animationClipDuration =
                Player.PlayerAnimation.AnimatorController.dictionaryAnimationClips[Data.Climb.animationTriggerName].length;
        }

        private void InitLandingParameters(string nameTriggerParameters)
        {
            Data.Landing.animationTriggerName = nameTriggerParameters;
            Data.Landing.animationClipDuration = 2f;
        }
        
        private void OnClimb()
        {
            Data.IsClimbing.Value = true;
        }

        //private void OnJumpPressedKey() => StateMachine.PlayerStateMachine.SwitchStates<>(); //Jump
    }
    
}