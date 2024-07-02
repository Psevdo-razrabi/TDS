using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class PlayerCrouch : PlayerSitsDown
    {
        private float _speed;
        protected CrouchMovement CrouchMovement;
        public PlayerCrouch(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override async void OnEnter()
        {
            base.OnEnter();
            Data.IsPlayerCrouch = true;
            CrouchMovement = Player.PlayerConfigs.CrouchMovement;
            await InterpolatedFloatWithEase(_speed, x => _speed = x, CrouchMovement.Speed,
                CrouchMovement.TimeToMaxSpeed, CrouchMovement.CurveToMaxSpeed);
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsPlayerCrouch = false;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            GravityForce();
        }

        protected override void Move()
        {
            var direction = new Vector3(Movement.x, 0f, 0f);
            Player.CharacterController.Move(_speed * Time.deltaTime * direction);
        }
    }
}