using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class PlayerSitsDown : BaseCrouching
    {
        private CrouchAndStandConfig _crouchAndStandConfig;
        public PlayerSitsDown(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override async void OnEnter()
        {
            base.OnEnter();
            _crouchAndStandConfig = Player.PlayerConfigs.SitDownCrouch;
            Data.IsPlayerSitDown = true;
            await PlayerSitDown();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsPlayerSitDown = false;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
        }
        
        private async UniTask PlayerSitDown()
        {
            var heightChange = InterpolatedFloatWithEase(Player.CharacterController.height,
                x => Player.CharacterController.height = x,
                _crouchAndStandConfig.HeightOfCharacterController, _crouchAndStandConfig.TimeToCrouch,
                _crouchAndStandConfig.CurveToCrouch);
            
            var centerChange = InterpolatedVector3WithEase(Player.CharacterController.center,
                x => Player.CharacterController.center = x,
                _crouchAndStandConfig.CenterCharacterController, _crouchAndStandConfig.TimeToCrouch,
                _crouchAndStandConfig.CurveToCrouch);

            await UniTask.WhenAll(heightChange, centerChange);
            Data.IsPlayerCrouch = true;
        }

        protected async UniTask InterpolatedFloatWithEase(float startValue, Action<float> setter, float endValue, float duration, AnimationCurve curve)
        {
            await DOTween
                .To(() => startValue, x => setter(x), endValue, duration)
                .SetEase(curve);
        }
        
        protected async UniTask InterpolatedVector3WithEase(Vector3 startValue, Action<Vector3> setter, Vector3 endValue, float duration, AnimationCurve curve)
        {
            await DOTween
                .To(() => startValue, x => setter(x), endValue, duration)
                .SetEase(curve);
        }

        /*private async UniTask ChangeHeight()
        {
            await DOTween
                .To(() => Player.CharacterController.height, x => Player.CharacterController.height = x, 
                _crouchAndStandConfig.HeightOfCharacterController, _crouchAndStandConfig.TimeToCrouch)
                .SetEase(_crouchAndStandConfig.CurveToCrouch);
        }

        private async UniTask ChangeCenterCharacterController()
        {
            await DOTween
                .To(() => Player.CharacterController.center, x => Player.CharacterController.center = x,
                    _crouchAndStandConfig.CenterCharacterController, _crouchAndStandConfig.TimeToCrouch)
                .SetEase(_crouchAndStandConfig.CurveToCrouch);
        }*/
    }
}