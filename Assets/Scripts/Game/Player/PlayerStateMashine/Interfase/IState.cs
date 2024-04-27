namespace Game.Player.PlayerStateMashine.Interfase
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void OnUpdateBehaviour();
    }
}