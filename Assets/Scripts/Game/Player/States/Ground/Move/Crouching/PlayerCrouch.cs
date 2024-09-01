using Cysharp.Threading.Tasks;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.StateHandle;
using UniRx;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class PlayerCrouch : BaseCrouching
    {
        protected CrouchMovement CrouchMovement;
        public PlayerCrouch(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CrouchMovement = Player.PlayerConfigs.CrouchConfigsProvider.CrouchMovement;
            Data.GetValue<ReactiveProperty<bool>>(Name.IsMove).Value = true;
            CreateTokenAndDelete();
            
            InterpolatedFloatWithEase(Data.Speed, x => Data.Speed = x, CrouchMovement.Speed,
                CrouchMovement.TimeToMaxSpeed, CrouchMovement.CurveToMaxSpeed, Cancellation.Token).Forget();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.GetValue<ReactiveProperty<bool>>(Name.IsMove).Value = false;
            Data.Speed = 0f;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            Move();
            ChangeState();
        }

        protected override void Move()
        {
            var direction = new Vector3(Data.GetValue<Vector3>(Name.Movement).x, Data.GetValue<float>(Name.TargetDirectionY), Data.GetValue<Vector3>(Name.Movement).z);
            Player.PlayerComponents.CharacterController.Move(Data.Speed * Time.deltaTime * direction);
        }

        private void ChangeState()
        {
            Player.PlayerStateMachine.StateChain.HandleState<PlayerIdleCrouchHandle>();
            Player.PlayerStateMachine.StateChain.HandleState<PlayerStandUpCrouchHandler>();
        }
    }
}