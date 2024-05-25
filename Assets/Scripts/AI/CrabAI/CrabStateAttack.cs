using System;
using UnityEngine;
using UnityEngine.AI;

public class CrabStateAttack : FSMStateAdapter
{
    public CrabStateAttack(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid) 
        : base(agent, currentGameObject, targetGameObject, sensors, navGrid)
    {
    }

    public override void Update()
    {
        base.Update();

        Vector3 targetPos = GetTargetPos();

        //_agent.CalculatePath(targetPos, )
    }

    private Vector3 GetTargetPos()
    {
        return new(0, 0, 0);
    }
}
