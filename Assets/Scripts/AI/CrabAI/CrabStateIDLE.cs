using UnityEngine;
using UnityEngine.AI;

public class CrabStateIDLE : FSMStateAdapter
{
    public CrabStateIDLE(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid) 
        : base(agent, currentGameObject, targetGameObject, sensors, navGrid)
    {
    }

    public override void Update()
    {
        base.Update();
        if (_sensors.IsTriggeredFirst().Item1)
        {
            _fsm.ChangeState<CrabStateRun>();
        }
    }
}
