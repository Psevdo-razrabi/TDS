using UnityEngine;
using UnityEngine.AI;

public class MeleStateAttack : FSMStateAdapter
{
    private float countAttackPerSec = 0.5F;
    private float damage = 1F;

    public MeleStateAttack(NavMeshAgent agent, GameObject currentGameObject, GameObject targetGameObject, AISensors sensors, NavGrid navGrid) :
        base(agent, currentGameObject, targetGameObject, sensors, navGrid)
    {

    }

    public override void Update()
    {
        base.Update();


    }
}