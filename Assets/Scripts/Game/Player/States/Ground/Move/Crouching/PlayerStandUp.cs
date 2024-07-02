using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;

namespace Game.Player.States.Crouching
{
    public class PlayerStandUp : PlayerSitsDown
    {
        private CrouchAndStandConfig _standUp;
        public PlayerStandUp(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override async void OnEnter()
        {
            base.OnEnter();
            _standUp = Player.PlayerConfigs.StandUpCrouch;
            await PlayerSitDown();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsPlayerSitDown = true;
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