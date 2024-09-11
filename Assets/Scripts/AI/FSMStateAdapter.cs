using ModestTree;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class FSMStateAdapter : FSMState
{
    public static readonly float DESTANCE_GENERATE_CHUNKS = 1;

    protected FSM _fsm;
    protected NavMeshAgent _agent;
    protected GameObject _currentGameObject;
    protected GameObject _targetGameObject;
    protected AISensors _sensors;
    protected NavGrid _navGrid;
    protected GridPoint _point;

    protected bool _targetMovedTriger = true;
    protected float _targetMovedDistTriger = 1;
    protected Vector3 _targerLastPosition;

    protected GeneratorChunksRound _generatorChunksRound;

    public FSMStateAdapter(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid)
    {
        _agent = agent;
        _currentGameObject = currentGameObject;
        _targetGameObject = targetGameObject;
        _sensors = sensors;
        _navGrid = navGrid;

        _targerLastPosition = _targetGameObject.transform.position;

        _generatorChunksRound = new GeneratorChunksRound(_navGrid.GetPointMap(), _currentGameObject.transform, DESTANCE_GENERATE_CHUNKS);
    }

    public float GetSqrDistancePlayer()
    {
        return Vector3.SqrMagnitude(_currentGameObject.transform.position - _targetGameObject.transform.position);
    }

    virtual public void Enter()
    {
        _point = _navGrid._pointMap.GetPointAtPosition(_currentGameObject.transform.position);
        if (_point != null)
        {
            if (_point.isFree)
            {
                _point.isFree = false;
            }
            else
            {
                _point = _navGrid._pointMap.FindFreePointAtPointMinDist(_point);
                if (_point != null)
                {
                    _point.isFree = false;
                }
            }
        }
    }

    virtual public void Exit()
    {
        if (_point != null)
        {
            _point.isFree = true;
        }
    }

    public void ChangePoint(GridPoint point)
    {
        if (_point != null) _point.isFree = true;
        _point = point;
        if (_point != null) _point.isFree = false;
    }

    public void MoveToPoint()
    {
        if (_point != null)
        {
            _agent.destination = _point.position;
        }
    }

    virtual public void SetFSM(FSM fsm)
    {
        _fsm = fsm;
    }

    virtual public void Update()
    {
        _generatorChunksRound.Update();

        if ((_targetGameObject.transform.position - _targerLastPosition).sqrMagnitude > _targetMovedDistTriger)
        {
            _targetMovedTriger = true;
            _targerLastPosition = _targetGameObject.transform.position;
        }
    }
}
