using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAICrab : MonoBehaviour
{
    private GameObject _hero;
    private FSM _fsm;

    void Start()
    {
        _hero = GameObject.Find("Player");
        GameObject ground = GameObject.Find("Ground");
        if (!_hero)
        {
            throw new Exception("Can't find Player");
        }

        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        AISensors sensors = new AISensors();
        sensors.AddSensor(new AISensorSound(gameObject, _hero, 5));
        sensors.AddSensor(new AISensorVision(gameObject, _hero, 60, 5, 12));
        List<FSMState> states = new()
        {
            new CrabStateIDLE(navMeshAgent, this.gameObject, _hero, sensors, ground.GetComponent<NavGrid>()),
            new CrabStateRun(navMeshAgent, this.gameObject, _hero, sensors, ground.GetComponent < NavGrid >()),
        };
        _fsm = new FSM(states);
    }

    void Update()
    {
        _fsm.Update();

    }

}
