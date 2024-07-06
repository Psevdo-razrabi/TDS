using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.StateHandle;
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
            _crouchAndStandConfig = Player.PlayerConfigs.SitDownCrouch;
            Data.IsPlayerSitDown = true;
            Debug.Log("вошел в crouchSitDown");
            Player.InputSystemMouse.mouseRightClickUpHandler?.Invoke();
            Player.InputSystemMouse.OnUnsubscribeRightMouseClickUp();
            Player.InputSystemMouse.OnUnsubscribeRightMouseClickDown();
            await PlayerSitDown();
            Player.StateChain.HandleState<PlayerMoveCrouchHandle>();
            Player.StateChain.HandleState<PlayerIdleCrouchHandle>();
            Player.StateChain.HandleState<PlayerStandUpCrouchHandler>();
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("вышел из crouchSitDown");
            Data.IsPlayerSitDown = false;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
            Debug.LogWarning(Data.IsAim);
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