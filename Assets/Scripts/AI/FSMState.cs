
public interface FSMState
{
    void Enter();
    void Exit();
    void Update();
    void SetFSM(FSM fsm);
}
