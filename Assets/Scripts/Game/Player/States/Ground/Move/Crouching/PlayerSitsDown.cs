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

        public override void OnEnter()
        {
            Data.Speed = Player.PlayerConfigs.CrouchMovement.Speed;
            _crouchAndStandConfig = Player.PlayerConfigs.SitDownCrouch;
            Data.IsAim.Value = false;
            Debug.Log("вошел в crouchSitDown");
            PlayerSitDown().Forget();
            Player.StateChain.HandleState<PlayerMoveCrouchHandle>();
            Player.StateChain.HandleState<PlayerIdleCrouchHandle>();
            Player.StateChain.HandleState<PlayerStandUpCrouchHandler>();
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
        }
        
        private async UniTask PlayerSitDown()
        {
            var heightChange = InterpolatedFloatWithEase(Player.CharacterController.height,
                x =>
                {
                    Player.CharacterController.height = x;
                    Player.IKSystem.ChangeColliderInitHeight(x);
                },
                _crouchAndStandConfig.HeightOfCharacterController, _crouchAndStandConfig.TimeToCrouch,
                _crouchAndStandConfig.CurveToCrouch);
            
            var centerChange = InterpolatedVector3WithEase(Player.CharacterController.center,
                x => Player.CharacterController.center = x,
                _crouchAndStandConfig.CenterCharacterController, _crouchAndStandConfig.TimeToCrouch,
                _crouchAndStandConfig.CurveToCrouch);

            await UniTask.WhenAll(heightChange, centerChange);
        }
    }
}