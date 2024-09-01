using Cysharp.Threading.Tasks;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.StateHandle;
using UniRx;

namespace Game.Player.States.Crouching
{
    public class PlayerSitsDown : BaseCrouching
    {
        private CrouchAndStandConfig _crouchAndStandConfig;
        public PlayerSitsDown(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void OnEnter()
        {
            Data.Speed = Player.PlayerConfigs.CrouchConfigsProvider.CrouchMovement.Speed;
            _crouchAndStandConfig = Player.PlayerConfigs.CrouchConfigsProvider.SitDownCrouch;
            Data.GetValue<ReactiveProperty<bool>>(Name.IsAim).Value = false;
            CreateTokenAndDelete();
            PlayerSitDown().Forget();
            ChangeState();
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
                _crouchAndStandConfig.HeightOfCharacterController, _crouchAndStandConfig.TimeToCrouch,
                _crouchAndStandConfig.CurveToCrouch, Cancellation.Token);
            
            var centerChange = InterpolatedVector3WithEase(Player.PlayerComponents.CharacterController.center,
                x => Player.PlayerComponents.CharacterController.center = x,
                _crouchAndStandConfig.CenterCharacterController, _crouchAndStandConfig.TimeToCrouch,
                _crouchAndStandConfig.CurveToCrouch, Cancellation.Token);

            await UniTask.WhenAll(heightChange, centerChange);
        }

        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerMoveCrouchHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerIdleCrouchHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerStandUpCrouchHandler>(); 
        }
    }
}