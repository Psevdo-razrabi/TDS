using ModestTree;
using UnityEngine;
using UnityEngine.AI;

public class RangerStateRun : FSMStateAdapter
{

    Vector3 prevTakedPoint = Vector3.zero;
    private GridPoint currPoint = null;

    public RangerStateRun(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid)
        : base(agent, currentGameObject, targetGameObject, sensors, navGrid)
    {

    }

    public override void Update()
    {
        base.Update();

        if (_targetMovedTriger)
        {
            _targetMovedTriger = false;
            Vector3 pos = FindPosition();
            _agent.SetDestination(pos);
        }
    }

    private Vector3 FindPosition()
    {
        var point = _navGrid._pointMap.GetPointAtPosition(_targetGameObject.transform.position);
        if (point == null)
        {
            point = _navGrid._pointMap.FindFreeNearestPoint(_targetGameObject.transform.position);
            if (point == null) return _currentGameObject.transform.position;
        }
        if (currPoint != null)
        {
            currPoint.isFree = true;
        }
        GridPoint nvp = point.FindFreeNVPointMinDistance(_currentGameObject.transform.position);
        if (nvp != null)
        {
            nvp.isFree = false;
            currPoint = nvp;
            return nvp.position;
        }
        return _currentGameObject.transform.position;
    }

}
