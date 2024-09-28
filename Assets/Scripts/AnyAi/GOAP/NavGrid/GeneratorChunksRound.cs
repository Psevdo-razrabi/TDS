using System;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratorChunksRound
{
    public readonly PointMap map;
    public readonly Transform position;
    public readonly float distanceChunkGenerate;
    private CIndex prevCIndex = new CIndex(int.MaxValue, int.MaxValue);

    public GeneratorChunksRound(PointMap map, Transform position, float distanceChunkGenerate)
    {
        this.map = map;
        this.position = position;
        this.distanceChunkGenerate = Math.Max(distanceChunkGenerate, 1);
    }

    public void Update()
    {
        CIndex cindex = map.GetCIndexAtPosition(position.position);
        if (!cindex.Equals(prevCIndex))
        {
            prevCIndex = cindex;
            GenerateRoundChunks();
        }
    }

    private void GenerateRoundChunks()
    {
        int countChunks = (int)Math.Max(distanceChunkGenerate / Chunk.SXZ, 2);
        for (int z = prevCIndex.z - countChunks; z <= prevCIndex.z + countChunks; z++)
        {
            for (int x = prevCIndex.x - countChunks; x <= prevCIndex.x + countChunks; x++)
            {
                map.CreateGenerateChunkTack(new CIndex(x, z));
            }
        }
    }
}
