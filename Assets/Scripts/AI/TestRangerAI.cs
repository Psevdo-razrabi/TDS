using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAICrab : MonoBehaviour
{
    public GameObject _target;

    private FSM _fsm;

    void Start()
    {
        if (_target == null)
        {
            _target = GameObject.Find("Player");
        }
        if (!_target)
        {
            throw new Exception("Can't find Player");
        }

        GameObject ground = GameObject.Find("Ground");

        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        AISensors sensors = new AISensors();
        sensors.AddSensor(new AISensorSound(gameObject, _target, 5));
        sensors.AddSensor(new AISensorVision(gameObject, _target, 60, 5, 12));
        List<FSMState> states = new()
        {
            new RangerStatePatrol(navMeshAgent, this.gameObject, _target, sensors, ground.GetComponent<NavGrid>()),
            new RangerStateRun(navMeshAgent, this.gameObject, _target, sensors, ground.GetComponent < NavGrid >()),
        };
        _fsm = new FSM(states);
    }

    void Update()
    {
        _fsm.Update();
    }

}
