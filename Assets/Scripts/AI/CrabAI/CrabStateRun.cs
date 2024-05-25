using ModestTree;
using UnityEngine;
using UnityEngine.AI;

public class CrabStateRun : FSMStateAdapter
{

    private Vector3 _targetPosition;

    public CrabStateRun(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid)
        : base(agent, currentGameObject, targetGameObject, sensors, navGrid)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();

        FindTargetPosition();
        _agent.SetDestination(_targetPosition);

        Debug.DrawLine(_currentGameObject.transform.position, _targetPosition);
    }

    private void FindTargetPosition()
    {
        _targetPosition = _navGrid.FindHiddenMinPosition(_targetGameObject.transform.position);
    }
}
