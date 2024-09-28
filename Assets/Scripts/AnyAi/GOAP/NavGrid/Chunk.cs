using ModestTree;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    private static int DISTANCE_NVP = 15 * 15;
    private static float DISTANCE_TO_FLOOR = 0.7F;
    private static int chunk_max_height = 1;
    
    public static readonly int SXZ = 10;
    public readonly int sy;

    public readonly CIndex cIndex;
    public readonly Array3d<GridPoint> points;
    public readonly List<short> indices = new List<short>();
    public readonly HashSet<CIndex> calcPoints = new();
    private bool pointsIsGenerated = false;

    public Chunk(CIndex cIndex, int sy)
    {
        this.cIndex = cIndex;
        this.sy = sy;

        points = new Array3d<GridPoint>(SXZ, sy, SXZ);
        chunk_max_height = Math.Max(chunk_max_height, sy);
    }

    public bool GeneratePoints(IGenerateCallback generateCallback)
    {
        if (pointsIsGenerated) return false;
        indices.Clear();
        Vector3 pos = cIndex.GetPosition(SXZ, 0);
        float distanceToFloor = -1;
        for (int y = 0, i = 0; y < sy; y++)
        {
            for (int z = 0; z < SXZ; z++)
            {
                for (int x = 0; x < SXZ; x++, i++)
                {
                    var pointPos = new Vector3(x, y, z) + pos;
                    if (generateCallback.PointIsPossibleGenerate(pointPos, out distanceToFloor))
                    {
                        pointPos.y -= distanceToFloor;
                        pointPos.y += DISTANCE_TO_FLOOR;
                        points.Set(i, new GridPoint(this, pointPos));
                        indices.Add((short)i);
                    }
                }
            }
        }
        pointsIsGenerated = true;
        return true;
    }

    public bool CalculateNotVisiblePoints(IGenerateCallback generateCallback, Chunk other)
    {
        if (other == null || calcPoints.Contains(other.cIndex) || other.calcPoints.Contains(this.cIndex)) return false;
        if (!pointsIsGenerated || !other.pointsIsGenerated) return false;
        const string counterName = "CalculateNotVisiblePoints";
        for (int a = 0; a < indices.Count; a++)
        {
            for (int b = 0; b < other.indices.Count; b++)
            {
                var pa = points.Get(indices[a]);
                var pb = other.points.Get(other.indices[b]);
                if ((pa.position - pb.position).sqrMagnitude > DISTANCE_NVP) continue;
                if (/*FIX*/pa != pb && generateCallback.PointsIsNotVisible(pa.position, pb.position))
                {
                    pa.notVisiblePoints.Add(pb);
                    pb.notVisiblePoints.Add(pa);
                }
            }
        }
        calcPoints.Add(other.cIndex);
        if (other != this)
        {
            other.calcPoints.Add(this.cIndex);
        }
        return true;
    }

    public bool CalculateNotVisiblePoints(IGenerateCallback generateCallback)
    {
        return CalculateNotVisiblePoints(generateCallback, this);
    }

    public override bool Equals(object obj)
    {
        return obj is Chunk chunk &&
               EqualityComparer<CIndex>.Default.Equals(cIndex, chunk.cIndex);
    }

    public override int GetHashCode()
    {
        return cIndex.GetHashCode();
    }

    public override string ToString()
    {
        return cIndex.ToString() + " Points:" + indices.Count;
    }

    public bool PointsIsCreated()
    {
        return pointsIsGenerated;
    }

    public static int GetChunkMaxHeight()
    {
        return chunk_max_height;
    }

    public bool NVPIsGenerated(CIndex cindex)
    {
        return calcPoints.Contains(cindex);
    }
}
