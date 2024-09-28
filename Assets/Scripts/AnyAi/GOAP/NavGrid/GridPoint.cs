using System;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint
{
    public readonly Chunk chunk;
    public readonly List<GridPoint> notVisiblePoints = new List<GridPoint>();
    public readonly Vector3 position;
    public bool isFree = true;

    public GridPoint(Chunk chunk, Vector3 position)
    {
        this.chunk = chunk;
        this.position = position;
    }

    public override string ToString()
    {
        return chunk.ToString() + " | " + position.ToString() + " | " + isFree.ToString();
    }

    public GridPoint FindFreeNVPointMinDistance(Vector3 position)
    {
        float distance = float.MaxValue;
        GridPoint res = null;
        for (int i = 0, l = notVisiblePoints.Count; i < l; i++)
        {
            GridPoint p = notVisiblePoints[i];
            if (p == null || !p.isFree)
            {
                continue;
            }
            float dist = (p.position - position).sqrMagnitude;
            if (dist < distance)
            {
                distance = dist;
                res = p;
            }
        }
        return res;
    }
}