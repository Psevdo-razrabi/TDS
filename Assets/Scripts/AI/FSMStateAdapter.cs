using ModestTree;
using UnityEngine;
using UnityEngine.AI;

public class FSMStateAdapter : FSMState
{
    protected FSM _fsm;
    protected NavMeshAgent _agent;
    protected GameObject _currentGameObject;
    protected GameObject _targetGameObject;
    protected AISensors _sensors;
    protected NavGrid _navGrid;

    public FSMStateAdapter(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid)
    {
        _agent = agent;
        _currentGameObject = currentGameObject;
        _targetGameObject = targetGameObject;
        _sensors = sensors;
        _navGrid = navGrid;
    }

    public float GetSqrDistancePlayer()
    {
        return Vector3.SqrMagnitude(_currentGameObject.transform.position - _targetGameObject.transform.position);
    }

    virtual public void Enter()
    {
        Log.Info("ENTER => " + GetType().ToString());
    }

    virtual public void Exit()
    {
        Log.Info("EXIT => " + GetType().ToString());
    }

    virtual public void SetFSM(FSM fsm)
    {
        _fsm = fsm;
    }

    virtual public void Update()
    {

    }
}
