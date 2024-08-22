using Game.Player.AnyScripts;

namespace Game.Player.States.StateHandle
{
    public interface IStateHandle
    {
        PlayerStateMachine StateMachine { get; }
        bool CanHandle();
        void Handle();
    }
}