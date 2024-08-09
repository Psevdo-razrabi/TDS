using Cysharp.Threading.Tasks;
using Game.Player.PlayerStateMashine;
using Game.Player.PlayerStateMashine.Configs;
using Game.Player.States.StateHandle;
using UnityEngine;

namespace Game.Player.States.Crouching
{
    public class PlayerCrouch : BaseCrouching
    {
        protected CrouchMovement CrouchMovement;
        public PlayerCrouch(InitializationStateMachine stateMachine, Player player, StateMachineData stateMachineData) : base(stateMachine, player, stateMachineData)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("вошел в crouchMove");
            CrouchMovement = Player.PlayerConfigs.CrouchMovement;
            Data.IsMove.Value = true;
            
            InterpolatedFloatWithEase(Data.Speed, x => Data.Speed = x, CrouchMovement.Speed,
                CrouchMovement.TimeToMaxSpeed, CrouchMovement.CurveToMaxSpeed).Forget();
        }

        public override void OnExit()
        {
            base.OnExit();
            Data.IsMove.Value = false;
            Debug.Log("вышел из crouchMove");
            Data.Speed = 0f;
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
            Player.CharacterController.Move(Data.Speed * Time.deltaTime * direction);
        }
    }
}