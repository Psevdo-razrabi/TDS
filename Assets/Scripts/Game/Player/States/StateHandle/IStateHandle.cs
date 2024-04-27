using Game.Player.PlayerStateMashine;
using Zenject;

namespace Game.Player.States.StateHandle
{
    public interface IStateHandle
    {
        InitializationStateMachine StateMachine { get; }
        bool CanHandle();
        void Handle();
    }
}