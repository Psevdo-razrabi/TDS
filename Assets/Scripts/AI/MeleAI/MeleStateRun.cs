using UnityEngine;
using UnityEngine.AI;

public class MeleStateRun : FSMStateAdapter
{
    private float meleDistanceAttack = Mathf.Pow(1.5F, 2);

    public MeleStateRun(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid) :
        base(agent, currentGameObject, targetGameObject, sensors, navGrid)
    {

    }

    public override void Update()
    {
        base.Update();

        if (_targetMovedTriger)
        {
            _targetMovedTriger = false;
            _agent.destination = _targetGameObject.transform.position;
        }

        if ((_targetGameObject.transform.position - _currentGameObject.transform.position).sqrMagnitude < meleDistanceAttack)
        {
            _fsm.ChangeState<MeleStateAttack>();
        }
    }
}
