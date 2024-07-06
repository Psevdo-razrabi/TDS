using Game.Player.States;
using Game.Player.States.Crouching;
using Game.Player.States.Dash;
using Game.Player.States.Orientation;
using Zenject;

namespace Game.Player.PlayerStateMashine
{
    public class InitializationStateMachine : IInitializable
    {
        public StateMachine PlayerStateMachine { get; private set; }
        [Inject] public StateMachineData Data { get; private set; }
        [Inject] private Player _player;

        public void Initialize()
        {
            PlayerStateMachine = new StateMachine(new PlayerIdle(this, _player, Data), new PlayerAimIdle(this, _player, Data),
                new PlayerMove(this, _player, Data), new PlayerMoveInAim(this, _player, Data), new PlayerDash(this, _player, Data),
                new PlayerCrouch(this, _player, Data), new PlayerCrouchIdle(this, _player, Data), new PlayerSitsDown(this, _player, Data),
                new PlayerStandUp(this, _player, Data)); //инициализация стейт машины, в конструктрое надо указать все состояния и переключиться в базовое состояние
            
            PlayerStateMachine.SwitchStates<PlayerIdle>();
        }
    }
}