using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SimpleEnemy : MonoBehaviour
{
    [SerializeField] private NavGrid _navGrid;
    private GeneratorChunksRound _generatorChunksRound;
    private float timer = 0;
    private float timeToNextChangePosition;
    private NavMeshAgent _meshAgent;

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
        _generatorChunksRound = new GeneratorChunksRound(_navGrid.GetPointMap(), transform, 1);

        timeToNextChangePosition = Random.Range(3, 7);
    }

    private void Update()
    {
        _generatorChunksRound.Update();

        timer += Time.deltaTime;

        if (!(timer > timeToNextChangePosition)) return;
        
        ChangePosition();
        timer = 0;
        timeToNextChangePosition = Random.Range(3, 7);
    }

    private void ChangePosition()
    {
        var pm = _navGrid.GetPointMap();
        var point = pm.GetPointAtPosition(transform.position);
        if(point != null)
        {
            var np = pm.FindFreePointAtPointRandomDist(point, 3, 10);
            if(np != null)
            {
                _meshAgent.destination = np.Position;
            }
        }
    }
}
