using ModestTree;
using System;
using UnityEngine;
using UnityEngine.AI;

public class RangerStatePatrol : FSMStateAdapter
{
    public static float TIME_CHANGE_POINT_MIN = 2;
    public static float TIME_CHANGE_POINT_MAX = 4;

    private float timeChangePoint = 4;
    private float elapsedTime;
    private enum StatePath { MOVE, END };
    private StatePath statePath = StatePath.END;
    private GridPoint lastPoint;

    public RangerStatePatrol(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid)
        : base(agent, currentGameObject, targetGameObject, sensors, navGrid)
    {
    }


    public override void Update()
    {
        base.Update();

        if (_sensors.IsTriggeredFirst().Item1)
        {
            _fsm.ChangeState<RangerStateRun>();
        }
        else
        {
            if (statePath == StatePath.END) elapsedTime += UnityEngine.Time.deltaTime;
            if (elapsedTime >= timeChangePoint)
            {
                ChangePoint();

                elapsedTime = 0;
            }
            else if (lastPoint != null)
            {
                if ((_currentGameObject.transform.position - lastPoint.position).sqrMagnitude < 1 * 1)
                {
                    statePath = StatePath.END;
                }
            }
        }
    }

    private void ChangePoint()
    {
        var point = _navGrid._pointMap.GetPointAtPosition(_currentGameObject.transform.position);
        var newPoint = _navGrid._pointMap.FindFreePointAtPointRandomDist(point, 2F, 6F);
        ChangePoint(newPoint);
        MoveToPoint();
        lastPoint = newPoint;
        
        statePath = StatePath.MOVE;
    }

}
