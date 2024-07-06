using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class PlayerCrouch : BaseCrouching
    {
        private float _speed;
        protected CrouchMovement CrouchMovement;
        public PlayerCrouch(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override async void OnEnter()
        {
            base.OnEnter();
            Debug.Log("вошел в crouchMove");
            Data.IsPlayerCrouch = true;
            CrouchMovement = Player.PlayerConfigs.CrouchMovement;
            await InterpolatedFloatWithEase(_speed, x => _speed = x, CrouchMovement.Speed,
                CrouchMovement.TimeToMaxSpeed, CrouchMovement.CurveToMaxSpeed);
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("вышел из crouchMove");
            Data.IsPlayerCrouch = false;
            _speed = 0f;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            Debug.Log("обновляю crouchMove");
            Move();
            Player.StateChain.HandleState<PlayerIdleCrouchHandle>();
            Player.StateChain.HandleState<PlayerStandUpCrouchHandler>();
        }

        protected override void Move()
        {
            var direction = new Vector3(Movement.x, Data.TargetDirectionY, Movement.z);
            Player.CharacterController.Move(_speed * Time.deltaTime * direction);
        }
    }
}