using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Game.Player.States;
using Game.Player.States.Air;
using Game.Player.States.Crouching;
using Game.Player.States.Dash;
using Game.Player.States.Parkour;
using Game.Player.States.StateHandle;
using Zenject;

namespace Game.Player.AnyScripts
{
    public class PlayerStateMachine : IStateData, IInitializable
    {
        public StateHandleChain StateChain { get; private set; }
        public StateMachineData Data { get; private set; }
        public StateMachine StateMachine { get; private set; }
        public Player Player { get; private set; }
        private PlayerConfigs _playerConfigs;

        public PlayerStateMachine(StateMachineData stateMachineData)
        {
            Data = stateMachineData;
        }
        
        public async void Initialize()
        {
            await Player.AsyncWorker.AwaitLoadConfigs(Player.PlayerConfigs);
            StateMachine = new StateMachine(new PlayerIdle(this), new PlayerAimIdle(this),
                new PlayerMove(this), new PlayerMoveInAim(this), new PlayerDash(this),
                new PlayerCrouch(this), new PlayerCrouchIdle(this), new PlayerSitsDown(this),
                new PlayerStandUp(this), new PlayerClimbToObstacle(this), new PlayerFalling(this)); //инициализация стейт машины, в конструктрое надо указать все состояния и переключиться в базовое состояние
            
            StateMachine.SwitchStates<PlayerIdle>();
        }
        public void InitStateChain(StateHandleChain stateHandleChain) => StateChain = stateHandleChain;
        public void InitPlayer(Player player) => Player = player;
    }
}