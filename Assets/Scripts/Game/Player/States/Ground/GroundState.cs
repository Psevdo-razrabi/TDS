using System;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;

namespace Game.Player.States
{
    public abstract class GroundState : PlayerBehaviour
    {
        private CompositeDisposable _compositeDisposable = new();

        private IDisposable _crouchButtonDown;
        private IDisposable _crouchButtonUp;
        
        protected GroundState(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
            
        }
        
        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();
            SubscribeRightMouseClickUp();
            SubscribeRightMouseClickDown();
        }
        
        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            
            Player.InputSystemMouse.OnUnsubscribeRightMouseClickUp();
            Player.InputSystemMouse.OnUnsubscribeRightMouseClickDown();
        }

        private void SubscribeRightMouseClickUp()
        {
            Player.InputSystemMouse.OnSubscribeRightMouseClickUp(() =>
            {
                if(Player.PlayerConfigs.IsLoadAllConfig == false) return;
                Data.IsAiming.Value = false;
                Player.AnimatorController.OnAnimatorStateSet(ref Data.IsAim, false, Player.AnimatorController.NameAimParameter);
                Data.IsAim = false;
                var config = Player.PlayerConfigs.FowConfig;
                Player.RadiusChanger.ChangerRadius(config.StartValueRadius, config.EndValueRadius, config.TimeToMaxRadius);
                Player.StateChain.HandleState();
            });
        }

        private void SubscribeRightMouseClickDown()
        {
            Player.InputSystemMouse.OnSubscribeRightMouseClickDown(() =>
            {
                if(Player.PlayerConfigs.IsLoadAllConfig == false) return;
                Data.IsAiming.Value = true;
                Player.AnimatorController.OnAnimatorStateSet(ref Data.IsAim, true, Player.AnimatorController.NameAimParameter);
                Data.IsAim = true;
                var config = Player.PlayerConfigs.FowConfig;
                Player.RadiusChanger.ChangerRadius(config.EndValueRadius, config.StartValueRadius, config.TimeToMaxRadius);
                Player.StateChain.HandleState();
            });
        }

        //private void OnJumpPressedKey() => StateMachine.PlayerStateMachine.SwitchStates<>(); //Jump
    }
}