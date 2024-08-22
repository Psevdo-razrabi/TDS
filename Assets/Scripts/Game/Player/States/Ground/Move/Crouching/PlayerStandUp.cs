using Cysharp.Threading.Tasks;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class PlayerStandUp : BaseCrouching
    {
        private CrouchAndStandConfig _standUp;
        public PlayerStandUp(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            _standUp = Player.PlayerConfigs.CrouchConfigsProvider.StandUpCrouch;
            CreateTokenAndDelete();
            PlayerSitDown().Forget();
            Player.PlayerStateMachine.StateChain.HandleState();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsCrouch.Value = false;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
        }
        
        private async UniTask PlayerSitDown()
        {
            var heightChange = InterpolatedFloatWithEase(Player.PlayerComponents.CharacterController.height,
                x =>
                {
                    Player.PlayerComponents.CharacterController.height = x;
                    Player.PlayerIK.IKSystem.ChangeColliderInitHeight(x);
                },
                _standUp.HeightOfCharacterController, _standUp.TimeToCrouch, _standUp.CurveToCrouch, Cancellation.Token);
            
            var centerChange = InterpolatedVector3WithEase(Player.PlayerComponents.CharacterController.center,
                x => Player.PlayerComponents.CharacterController.center = x,
                _standUp.CenterCharacterController, _standUp.TimeToCrouch, _standUp.CurveToCrouch, Cancellation.Token);

            await UniTask.WhenAll(heightChange, centerChange);
        }
    }
}