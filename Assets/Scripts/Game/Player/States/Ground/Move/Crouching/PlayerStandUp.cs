﻿using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class PlayerStandUp : BaseCrouching
    {
        private CrouchAndStandConfig _standUp;
        public PlayerStandUp(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override async void OnEnter()
        {
            base.OnEnter();
            _standUp = Player.PlayerConfigs.StandUpCrouch;
            Debug.Log("вошел в crouchStandUp");
            await PlayerSitDown();
            Player.StateChain.HandleState();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsCrouch = false;
            Debug.Log("вышел из crouchStandUp");
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
                _standUp.HeightOfCharacterController, _standUp.TimeToCrouch, _standUp.CurveToCrouch);
            
            var centerChange = InterpolatedVector3WithEase(Player.CharacterController.center,
                x => Player.CharacterController.center = x,
                _standUp.CenterCharacterController, _standUp.TimeToCrouch, _standUp.CurveToCrouch);

            await UniTask.WhenAll(heightChange, centerChange);
            Data.IsPlayerCrouch = true;
        }
    }
}